using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class ChannelBatchWorkflowService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    IRuntimeEventPublisher eventPublisher)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string[] ActiveBatchStatuses =
    [
        RuntimeLedgerStatus.Pending,
        RuntimeLedgerStatus.Running,
        RuntimeLedgerStatus.Paused,
        RuntimeLedgerStatus.Faulted
    ];

    public Task<ChannelBatchWorkflowResponse> SelectInitialWorkflowAsync(
        SelectChannelWorkflowRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "channel.workflow.initial_select",
            request,
            actor,
            async () =>
            {
                var now = DateTimeOffset.UtcNow;
                var experimentType = NormalizeExperimentType(request.ExperimentType);
                var version = await LoadPublishedWorkflowVersionAsync(request.WorkflowVersionId, experimentType, cancellationToken);
                var snapshot = JsonSerializer.Serialize(WorkflowSnapshotFactory.Create(version), JsonOptions);

                var batch = await LoadTargetBatchAsync(request, cancellationToken);
                EnsureBatchCanInitialSelect(batch);
                AddInitialSelectionHistory(batch, actor, request.CommandId, experimentType, version.Id, snapshot, now);

                batch.ExperimentType = experimentType;
                batch.SelectedWorkflowVersionId = version.Id;
                batch.WorkflowSnapshotJson = snapshot;
                batch.WorkflowSelectionStatus = WorkflowSelectionStatus.Selected;
                batch.WorkflowSelectedAtUtc = now;
                batch.WorkflowSelectedByUserId = actor.UserId;

                AddAudit(actor, "channel.workflow.select", batch.Id, new
                {
                    batch.DrawerCode,
                    batch.ExperimentType,
                    workflowVersionId = version.Id,
                    commandId = request.CommandId,
                    correlationId = request.CommandId
                });
                PublishChannelBatchChanged(batch, "workflowSelected");

                return new CommandExecutionResult<ChannelBatchWorkflowResponse>(
                    new ChannelBatchWorkflowResponse(
                        true,
                        request.CommandId,
                        false,
                        batch.Id,
                        batch.DrawerCode,
                        experimentType,
                        version.Id,
                        batch.WorkflowSelectionStatus,
                        batch.WorkflowSelectedAtUtc,
                        "Channel workflow selected."),
                    "ChannelBatch",
                    batch.Id);
            },
            cancellationToken);
    }

    public Task<ChannelBatchActivationResponse> EnsureActiveBatchAsync(
        EnsureChannelBatchRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "channel_batch.ensure_active",
            request,
            actor,
            async () =>
            {
                var drawerCode = NormalizeDrawerCode(request.DrawerCode);
                var drawer = await dbContext.Drawers.SingleOrDefaultAsync(x => x.Code == drawerCode, cancellationToken);
                if (drawer is null)
                {
                    throw new BusinessRuleException("drawer_not_found", "Drawer was not found.", StatusCodes.Status404NotFound);
                }

                var activeBatches = await dbContext.ChannelBatches
                    .Where(x => x.DrawerId == drawer.Id && ActiveBatchStatuses.Contains(x.Status))
                    .ToListAsync(cancellationToken);
                var existing = activeBatches
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .FirstOrDefault();
                var batch = existing ?? new ChannelBatch
                {
                    DrawerId = drawer.Id,
                    DrawerCode = drawer.Code,
                    Status = RuntimeLedgerStatus.Pending,
                    WorkflowSnapshotJson = "{}",
                    WorkflowSelectionStatus = WorkflowSelectionStatus.Unselected,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                };

                if (existing is null)
                {
                    dbContext.ChannelBatches.Add(batch);
                    AddAudit(actor, "channel_batch.ensure_active", batch.Id, new
                    {
                        batch.DrawerCode,
                        commandId = request.CommandId
                    });
                    PublishChannelBatchChanged(batch, "batchCreated");
                }

                return new CommandExecutionResult<ChannelBatchActivationResponse>(
                    new ChannelBatchActivationResponse(
                        true,
                        request.CommandId,
                        false,
                        batch.Id,
                        batch.DrawerCode,
                        batch.Status,
                        batch.WorkflowSelectionStatus,
                        existing is null ? "Channel batch created." : "Active channel batch exists."),
                    "ChannelBatch",
                    batch.Id);
            },
            cancellationToken);
    }

    public Task<ChannelBatchWorkflowResponse> SelectWorkflowAsync(
        SelectChannelWorkflowRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return SelectInitialWorkflowAsync(request, actor, cancellationToken);
    }

    public async Task<ChannelBatch?> GetActiveBatchAsync(string drawerCode, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeDrawerCode(drawerCode);
        var batches = await dbContext.ChannelBatches
            .Include(x => x.SlideTasks)
            .ThenInclude(x => x.StainingTask)
            .Include(x => x.SelectedWorkflowVersion)
            .ThenInclude(x => x!.WorkflowDefinition)
            .Where(x => x.DrawerCode == normalized && ActiveBatchStatuses.Contains(x.Status))
            .ToListAsync(cancellationToken);
        return batches.OrderByDescending(x => x.CreatedAtUtc).FirstOrDefault();
    }

    public async Task<ChannelBatch> RequireSelectedActiveBatchAsync(string drawerCode, CancellationToken cancellationToken = default)
    {
        var batch = await GetActiveBatchAsync(drawerCode, cancellationToken);
        if (batch is null || string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId))
        {
            throw new BusinessRuleException("channel_workflow_required", "Select a channel workflow before adding slides.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus == WorkflowSelectionStatus.NeedsManualResolution)
        {
            throw new BusinessRuleException("channel_batch_needs_manual_resolution", "Channel batch needs manual workflow resolution before it can be used.", StatusCodes.Status409Conflict);
        }

        return batch;
    }

    public static bool IsActiveStatus(string status)
    {
        return ActiveBatchStatuses.Contains(status);
    }

    private async Task<ChannelBatch> LoadTargetBatchAsync(SelectChannelWorkflowRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.ChannelBatchId))
        {
            var batch = await dbContext.ChannelBatches
                .Include(x => x.SlideTasks)
                .SingleOrDefaultAsync(x => x.Id == request.ChannelBatchId.Trim(), cancellationToken);
            return batch ?? throw new BusinessRuleException("channel_batch_not_found", "Channel batch was not found.", StatusCodes.Status404NotFound);
        }

        var drawerCode = NormalizeDrawerCode(request.DrawerCode);
        var batches = await dbContext.ChannelBatches
            .Include(x => x.SlideTasks)
            .Where(x => x.DrawerCode == drawerCode && ActiveBatchStatuses.Contains(x.Status))
            .ToListAsync(cancellationToken);
        var activeBatch = batches.OrderByDescending(x => x.CreatedAtUtc).FirstOrDefault();
        if (activeBatch is null)
        {
            throw new BusinessRuleException("channel_batch_not_found", "Channel batch was not found.", StatusCodes.Status404NotFound);
        }

        return activeBatch;
    }

    private static void EnsureBatchCanInitialSelect(ChannelBatch batch)
    {
        if (batch.NeedsManualResolution || batch.WorkflowSelectionStatus == WorkflowSelectionStatus.NeedsManualResolution)
        {
            throw new BusinessRuleException("channel_batch_needs_manual_resolution", "Channel batch needs manual workflow resolution.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowLockedAtUtc is not null || batch.StartedAtUtc is not null || !string.IsNullOrWhiteSpace(batch.MachineRunId))
        {
            throw new BusinessRuleException("channel_workflow_locked", "Channel workflow is locked after run start.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus != WorkflowSelectionStatus.Unselected
            || !string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId)
            || !string.IsNullOrWhiteSpace(batch.ExperimentType)
            || (!string.IsNullOrWhiteSpace(batch.WorkflowSnapshotJson) && batch.WorkflowSnapshotJson != "{}"))
        {
            throw new BusinessRuleException("channel_workflow_already_selected", "Channel workflow has already been selected.", StatusCodes.Status409Conflict);
        }

        if (batch.SlideTasks.Count > 0)
        {
            throw new BusinessRuleException("channel_batch_not_empty", "Initial workflow selection is only allowed for an empty channel batch.", StatusCodes.Status409Conflict);
        }
    }

    private async Task<WorkflowVersion> LoadPublishedWorkflowVersionAsync(string workflowVersionId, string experimentType, CancellationToken cancellationToken)
    {
        var normalized = (workflowVersionId ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("workflow_version_required", "workflowVersionId is required.", StatusCodes.Status400BadRequest);
        }

        var version = await dbContext.WorkflowVersions
            .Include(x => x.WorkflowDefinition)
            .Include(x => x.Steps)
            .Include(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Id == normalized, cancellationToken);
        if (version is null)
        {
            throw new BusinessRuleException("workflow_version_not_found", "Workflow version was not found.", StatusCodes.Status404NotFound);
        }

        if (version.Status != WorkflowVersionStatus.Published || version.WorkflowDefinition is null)
        {
            throw new BusinessRuleException("workflow_version_not_published", "Selected workflow version must be published.", StatusCodes.Status409Conflict);
        }

        if (version.WorkflowDefinition.WorkflowType is not (StainingTaskType.He or StainingTaskType.Ihc))
        {
            throw new BusinessRuleException("workflow_type_not_supported", "Only HE and IHC workflows are supported.", StatusCodes.Status409Conflict);
        }

        if (version.WorkflowDefinition.WorkflowType != experimentType)
        {
            throw new BusinessRuleException("workflow_type_mismatch", "Workflow experiment type does not match the requested experiment type.", StatusCodes.Status409Conflict);
        }

        return version;
    }

    private void AddInitialSelectionHistory(
        ChannelBatch batch,
        AuthenticatedUser actor,
        string commandId,
        string newExperimentType,
        string newWorkflowVersionId,
        string newSnapshot,
        DateTimeOffset now)
    {
        dbContext.WorkflowAssignmentHistory.Add(new WorkflowAssignmentHistory
        {
            ChannelBatch = batch,
            OldExperimentType = batch.ExperimentType,
            OldWorkflowVersionId = batch.SelectedWorkflowVersionId,
            OldWorkflowSnapshotJson = batch.WorkflowSnapshotJson,
            NewExperimentType = newExperimentType,
            NewWorkflowVersionId = newWorkflowVersionId,
            NewWorkflowSnapshotJson = newSnapshot,
            ActionType = WorkflowAssignmentAction.InitialSelection,
            ActorUserId = actor.UserId,
            OperatorUserId = actor.UserId,
            CreatedAtUtc = now,
            Reason = "Initial channel workflow selection.",
            CommandId = commandId,
            CorrelationId = commandId
        });
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = "ChannelBatch",
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private void PublishChannelBatchChanged(ChannelBatch batch, string reason)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.ChannelBatchChanged,
            batch.MachineRunId,
            "ChannelBatch",
            batch.Id,
            null,
            new Dictionary<string, object?>
            {
                ["channelBatchId"] = batch.Id,
                ["drawerCode"] = batch.DrawerCode,
                ["workflowSelectionStatus"] = batch.WorkflowSelectionStatus,
                ["experimentType"] = batch.ExperimentType,
                ["workflowVersionId"] = batch.SelectedWorkflowVersionId,
                ["reason"] = reason
            }));
    }

    private static string NormalizeDrawerCode(string? drawerCode)
    {
        var normalized = (drawerCode ?? string.Empty).Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("drawer_code_required", "drawerCode is required.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }

    private static string NormalizeExperimentType(string? experimentType)
    {
        var normalized = (experimentType ?? string.Empty).Trim().ToUpperInvariant();
        if (normalized is not (StainingTaskType.He or StainingTaskType.Ihc))
        {
            throw new BusinessRuleException("experiment_type_invalid", "ExperimentType must be HE or IHC.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }
}
