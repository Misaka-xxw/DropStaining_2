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
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string[] ActiveBatchStatuses =
    [
        RuntimeLedgerStatus.Pending,
        RuntimeLedgerStatus.Running,
        RuntimeLedgerStatus.Paused,
        RuntimeLedgerStatus.Faulted
    ];

    public Task<ChannelBatchWorkflowResponse> SelectWorkflowAsync(
        SelectChannelWorkflowRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "channel.workflow.select",
            request,
            actor,
            async () =>
            {
                var now = DateTimeOffset.UtcNow;
                var drawerCode = NormalizeDrawerCode(request.DrawerCode);
                var drawer = await dbContext.Drawers.SingleOrDefaultAsync(x => x.Code == drawerCode, cancellationToken);
                if (drawer is null)
                {
                    throw new BusinessRuleException("drawer_not_found", "Drawer was not found.", StatusCodes.Status404NotFound);
                }

                var version = await LoadPublishedWorkflowVersionAsync(request.WorkflowVersionId, cancellationToken);
                var experimentType = version.WorkflowDefinition!.WorkflowType;
                var snapshot = JsonSerializer.Serialize(WorkflowSnapshotFactory.Create(version), JsonOptions);

                var batch = await LoadActiveBatchAsync(drawer.Id, cancellationToken);
                if (batch is null)
                {
                    batch = new ChannelBatch
                    {
                        DrawerId = drawer.Id,
                        DrawerCode = drawer.Code,
                        Status = RuntimeLedgerStatus.Pending,
                        CreatedAtUtc = now
                    };
                    dbContext.ChannelBatches.Add(batch);
                }

                EnsureBatchCanChangeWorkflow(batch);
                await EnsureExistingSlidesAreCompatibleAsync(batch, experimentType, version.Id, cancellationToken);

                var isInitialSelection = string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId);
                var isSameSelection = batch.ExperimentType == experimentType
                    && batch.SelectedWorkflowVersionId == version.Id
                    && batch.WorkflowSelectionStatus == WorkflowSelectionStatus.Selected;
                if (!isInitialSelection && !isSameSelection && string.IsNullOrWhiteSpace(request.Reason))
                {
                    throw new BusinessRuleException("workflow_change_reason_required", "Reason is required when changing a pre-start channel workflow.", StatusCodes.Status400BadRequest);
                }

                if (!isSameSelection)
                {
                    AddHistory(
                        batch,
                        isInitialSelection ? WorkflowAssignmentAction.InitialSelection : WorkflowAssignmentAction.PreStartChange,
                        actor,
                        request.CommandId,
                        request.Reason ?? (isInitialSelection ? "Initial channel workflow selection." : string.Empty),
                        experimentType,
                        version.Id,
                        snapshot,
                        now);
                }

                batch.ExperimentType = experimentType;
                batch.SelectedWorkflowVersionId = version.Id;
                batch.WorkflowSnapshotJson = snapshot;
                batch.WorkflowSelectionStatus = WorkflowSelectionStatus.Selected;
                batch.WorkflowSelectedAtUtc = now;
                batch.WorkflowSelectedByUserId = actor.UserId;
                foreach (var task in batch.SlideTasks.Select(x => x.StainingTask).Where(x => x is not null).Cast<StainingTask>())
                {
                    task.WorkflowDefinitionId = version.WorkflowDefinitionId;
                    task.WorkflowVersionId = version.Id;
                    task.WorkflowSnapshotJson = snapshot;
                }

                AddAudit(actor, isInitialSelection ? "channel.workflow.select" : "channel.workflow.change", batch.Id, new
                {
                    batch.DrawerCode,
                    batch.ExperimentType,
                    workflowVersionId = version.Id,
                    request.Reason,
                    preflightInvalidated = !isInitialSelection
                });

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
                        isInitialSelection ? "Channel workflow selected." : "Channel workflow changed and preflight must be rerun."),
                    "ChannelBatch",
                    batch.Id);
            },
            cancellationToken);
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

    private async Task<ChannelBatch?> LoadActiveBatchAsync(string drawerId, CancellationToken cancellationToken)
    {
        var batches = await dbContext.ChannelBatches
            .Include(x => x.SlideTasks)
            .ThenInclude(x => x.StainingTask)
            .Where(x => x.DrawerId == drawerId && ActiveBatchStatuses.Contains(x.Status))
            .ToListAsync(cancellationToken);
        return batches.OrderByDescending(x => x.CreatedAtUtc).FirstOrDefault();
    }

    private static void EnsureBatchCanChangeWorkflow(ChannelBatch batch)
    {
        if (batch.WorkflowLockedAtUtc is not null || batch.StartedAtUtc is not null || !string.IsNullOrWhiteSpace(batch.MachineRunId))
        {
            throw new BusinessRuleException("channel_workflow_locked", "Channel workflow is locked after run start.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus == WorkflowSelectionStatus.NeedsManualResolution)
        {
            throw new BusinessRuleException("channel_batch_needs_manual_resolution", "Channel batch needs manual workflow resolution.", StatusCodes.Status409Conflict);
        }
    }

    private async Task EnsureExistingSlidesAreCompatibleAsync(
        ChannelBatch batch,
        string experimentType,
        string workflowVersionId,
        CancellationToken cancellationToken)
    {
        var tasks = batch.SlideTasks.Select(x => x.StainingTask).Where(x => x is not null).Cast<StainingTask>().ToList();
        if (tasks.Count == 0)
        {
            return;
        }

        if (tasks.Any(x => x.TaskType != experimentType))
        {
            throw new BusinessRuleException("channel_experiment_type_mismatch", "All slides in a channel must share the same experiment type.", StatusCodes.Status409Conflict);
        }

        if (experimentType == StainingTaskType.He)
        {
            return;
        }

        foreach (var task in tasks)
        {
            if (string.IsNullOrWhiteSpace(task.PrimaryAntibodyCode))
            {
                throw new BusinessRuleException("primary_antibody_required", "Existing IHC slides must have a primary antibody code before workflow change.", StatusCodes.Status409Conflict);
            }
        }

        var antibodyCodes = tasks.Select(x => x.PrimaryAntibodyCode!).Distinct().ToList();
        var compatibleCodes = await dbContext.PrimaryAntibodyWorkflowMappings
            .AsNoTracking()
            .Where(x => x.IsEnabled
                && x.WorkflowVersionId == workflowVersionId
                && antibodyCodes.Contains(x.PrimaryAntibodyCode))
            .Select(x => x.PrimaryAntibodyCode)
            .Distinct()
            .ToListAsync(cancellationToken);
        var incompatible = antibodyCodes.Except(compatibleCodes, StringComparer.OrdinalIgnoreCase).ToList();
        if (incompatible.Count > 0)
        {
            throw new BusinessRuleException(
                "channel_workflow_incompatible",
                $"Existing IHC slides are not compatible with the selected workflow: {string.Join(", ", incompatible)}.",
                StatusCodes.Status409Conflict);
        }
    }

    private async Task<WorkflowVersion> LoadPublishedWorkflowVersionAsync(string workflowVersionId, CancellationToken cancellationToken)
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

        return version;
    }

    private void AddHistory(
        ChannelBatch batch,
        string action,
        AuthenticatedUser actor,
        string commandId,
        string reason,
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
            ActionType = action,
            ActorUserId = actor.UserId,
            CreatedAtUtc = now,
            Reason = reason,
            CommandId = commandId
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

    private static string NormalizeDrawerCode(string drawerCode)
    {
        var normalized = (drawerCode ?? string.Empty).Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("drawer_code_required", "drawerCode is required.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }
}
