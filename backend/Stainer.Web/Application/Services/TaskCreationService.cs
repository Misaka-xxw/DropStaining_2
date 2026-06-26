using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class TaskCreationService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    ChannelBatchWorkflowService channelBatchWorkflowService,
    IRuntimeEventPublisher eventPublisher)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string CompatibilityCompatible = "Compatible";
    private const string CompatibilityIncompatible = "Incompatible";

    public Task<TaskCreationResponse> CreateHeTaskAsync(
        CreateHeTaskRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "task.create_he",
            request,
            actor,
            async () =>
            {
                var (slot, batch) = await LoadSlotAndBatchAsync(
                    request.SlotCode,
                    request.DrawerCode,
                    request.ChannelBatchId,
                    cancellationToken);
                var legacyWorkflowVersionId = NormalizeOptional(request.WorkflowVersionId);
                EnsureCanAddSlideToBatch(batch, StainingTaskType.He, legacyWorkflowVersionId);
                var version = await LoadChannelWorkflowVersionAsync(batch, StainingTaskType.He, cancellationToken);
                var task = CreateTask(
                    request.CommandId,
                    StainingTaskType.He,
                    slot,
                    batch,
                    version,
                    actor,
                    inputMode: "ManualHE",
                    rawCode: null,
                    normalizedCode: null,
                    primaryAntibodyCode: null,
                    candidateResults: Array.Empty<object>(),
                    rawSampleCode: null,
                    normalizedSampleCode: null,
                    lisQueryLogId: null,
                    lisCandidatePrimaryAntibodyCodes: [],
                    confirmedPrimaryAntibodyCode: null,
                    compatibilityValidationStatus: null,
                    compatibilityValidationMessage: null);
                dbContext.StainingTasks.Add(task);
                var slideTask = AddSlideTask(batch, task, slot, StainingTaskType.He);
                AddAudit(actor, "task.create_he", "StainingTask", task.Id, new
                {
                    task.TaskCode,
                    slot = slot.Code,
                    channelBatchId = batch.Id,
                    drawerCode = batch.DrawerCode,
                    inheritedWorkflowVersionId = batch.SelectedWorkflowVersionId,
                    legacyWorkflowVersionCompatibilityField = legacyWorkflowVersionId is not null,
                    legacyWorkflowVersionId
                });
                PublishSlideTaskCreated(batch, task, slot, slideTask);
                return new CommandExecutionResult<TaskCreationResponse>(
                    CreatedResponse(request.CommandId, false, task, batch, "HE task created."),
                    "StainingTask",
                    task.Id);
            },
            cancellationToken);
    }

    public async Task<TaskCreationResponse> CreateIhcTaskAsync(
        CreateIhcTaskRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        var resolution = await ResolveIhcSampleAsync(request, cancellationToken);
        if (resolution.RequiresSelection)
        {
            throw new BusinessSelectionRequiredException(new TaskCreationResponse(
                false,
                request.CommandId,
                false,
                true,
                resolution.Message,
                null,
                null,
                resolution.CandidatePrimaryAntibodyCodes,
                []));
        }

        var legacyWorkflowVersionId = GetLegacyWorkflowVersionId(request);
        IhcCompatibilityFailureAudit? compatibilityFailureAudit = null;
        try
        {
            return await idempotencyService.RunAsync(
                request.CommandId,
                "task.create_ihc",
                request,
                actor,
                async () =>
                {
                    var (slot, batch) = await LoadSlotAndBatchAsync(
                        request.SlotCode,
                        request.DrawerCode,
                        request.ChannelBatchId,
                        cancellationToken);
                    EnsureCanAddSlideToBatch(batch, StainingTaskType.Ihc, legacyWorkflowVersionId);
                    var version = await LoadChannelWorkflowVersionAsync(batch, StainingTaskType.Ihc, cancellationToken);
                    var compatibility = await ValidatePrimaryAntibodyCompatibleAsync(
                        resolution.PrimaryAntibodyCode!,
                        version.Id,
                        cancellationToken);
                    if (!compatibility.Ok)
                    {
                        compatibilityFailureAudit = new IhcCompatibilityFailureAudit(
                            batch.Id,
                            batch.DrawerCode,
                            slot.Code,
                            request.RawCode,
                            resolution.NormalizedCode,
                            resolution.PrimaryAntibodyCode!,
                            version.Id,
                            compatibility.Message,
                            request.CommandId);
                        throw new BusinessRuleException("ihc_channel_workflow_incompatible", compatibility.Message, StatusCodes.Status409Conflict);
                    }

                    var task = CreateTask(
                        request.CommandId,
                        StainingTaskType.Ihc,
                        slot,
                        batch,
                        version,
                        actor,
                        request.InputMode,
                        request.RawCode,
                        resolution.NormalizedCode,
                        resolution.PrimaryAntibodyCode,
                        new[]
                        {
                            new
                            {
                                lisCandidatePrimaryAntibodyCodes = resolution.CandidatePrimaryAntibodyCodes,
                                confirmedPrimaryAntibodyCode = resolution.PrimaryAntibodyCode,
                                inheritedWorkflowVersionId = version.Id,
                                compatibilityValidationStatus = CompatibilityCompatible,
                                compatibilityValidationMessage = compatibility.Message
                            }
                        },
                        rawSampleCode: request.RawCode,
                        normalizedSampleCode: resolution.NormalizedCode,
                        lisQueryLogId: NormalizeOptional(request.LisQueryLogId),
                        lisCandidatePrimaryAntibodyCodes: resolution.CandidatePrimaryAntibodyCodes,
                        confirmedPrimaryAntibodyCode: resolution.PrimaryAntibodyCode,
                        compatibilityValidationStatus: CompatibilityCompatible,
                        compatibilityValidationMessage: compatibility.Message);
                    dbContext.StainingTasks.Add(task);
                    var slideTask = AddSlideTask(batch, task, slot, StainingTaskType.Ihc);
                    AddAudit(actor, "task.create_ihc", "StainingTask", task.Id, new
                    {
                        task.TaskCode,
                        slot = slot.Code,
                        channelBatchId = batch.Id,
                        drawerCode = batch.DrawerCode,
                        task.RawSampleCode,
                        task.NormalizedSampleCode,
                        task.LisQueryLogId,
                        task.LisCandidatePrimaryAntibodyCodesJson,
                        task.ConfirmedPrimaryAntibodyCode,
                        inheritedWorkflowVersionId = batch.SelectedWorkflowVersionId,
                        task.CompatibilityValidationStatus,
                        task.CompatibilityValidationMessage,
                        legacyWorkflowVersionCompatibilityField = legacyWorkflowVersionId is not null,
                        legacyWorkflowVersionId
                    });
                    PublishSlideTaskCreated(batch, task, slot, slideTask);
                    return new CommandExecutionResult<TaskCreationResponse>(
                        CreatedResponse(request.CommandId, false, task, batch, "IHC task created."),
                        "StainingTask",
                        task.Id);
                },
                cancellationToken);
        }
        catch (BusinessRuleException ex) when (ex.Code == "ihc_channel_workflow_incompatible" && compatibilityFailureAudit is not null)
        {
            await PersistIhcCompatibilityFailureAuditAsync(actor, compatibilityFailureAudit, cancellationToken);
            throw;
        }
    }

    private async Task<IhcResolution> ResolveIhcSampleAsync(CreateIhcTaskRequest request, CancellationToken cancellationToken)
    {
        var inputMode = (request.InputMode ?? string.Empty).Trim();
        if (string.Equals(inputMode, "HospitalBarcode", StringComparison.OrdinalIgnoreCase)
            || string.Equals(inputMode, "Hospital", StringComparison.OrdinalIgnoreCase))
        {
            var normalized = NormalizeHospitalCode(request.RawCode);
            var candidates = await dbContext.HospitalBarcodeMappings
                .AsNoTracking()
                .Where(x => x.IsEnabled && x.HospitalCode == normalized)
                .Select(x => x.PrimaryAntibodyCode)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(cancellationToken);
            if (candidates.Count == 0)
            {
                throw new BusinessRuleException("lis_not_found", "LIS lookup returned no primary antibody code.", StatusCodes.Status404NotFound);
            }

            if (candidates.Count > 1 && string.IsNullOrWhiteSpace(request.SelectedPrimaryAntibodyCode))
            {
                return IhcResolution.Selection(
                    "Multiple primary antibody codes were found. Operator selection is required.",
                    normalized,
                    candidates);
            }

            var selected = string.IsNullOrWhiteSpace(request.SelectedPrimaryAntibodyCode)
                ? candidates.Single()
                : request.SelectedPrimaryAntibodyCode.Trim();
            var candidateMatch = candidates.FirstOrDefault(x => string.Equals(x, selected, StringComparison.OrdinalIgnoreCase));
            if (candidateMatch is null)
            {
                throw new BusinessRuleException("invalid_primary_antibody_selection", "Selected primary antibody code is not in the LIS candidate list.");
            }

            return IhcResolution.Final(normalized, candidateMatch, candidates);
        }

        var directCode = (request.RawCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(directCode))
        {
            throw new BusinessRuleException("primary_antibody_required", "Primary antibody code is required.");
        }

        return IhcResolution.Final(directCode, directCode, [directCode]);
    }

    private async Task<PhysicalSlot> LoadIdleSlotAsync(string slotCode, CancellationToken cancellationToken)
    {
        var normalized = (slotCode ?? string.Empty).Trim();
        var slot = await dbContext.PhysicalSlots
            .Include(x => x.Drawer)
            .SingleOrDefaultAsync(x => x.Code == normalized, cancellationToken);
        if (slot is null)
        {
            throw new BusinessRuleException("slot_not_found", "Physical slot was not found.", StatusCodes.Status404NotFound);
        }

        var occupied = await dbContext.StainingTasks.AnyAsync(
            x => x.PhysicalSlotId == slot.Id && x.Status == StainingTaskStatus.Confirmed,
            cancellationToken);
        if (occupied)
        {
            throw new BusinessRuleException("slot_not_idle", "Selected slot is not idle.", StatusCodes.Status409Conflict);
        }

        return slot;
    }

    private async Task<(PhysicalSlot Slot, ChannelBatch Batch)> LoadSlotAndBatchAsync(
        string slotCode,
        string? drawerCode,
        string? channelBatchId,
        CancellationToken cancellationToken)
    {
        var slot = await LoadIdleSlotAsync(slotCode, cancellationToken);
        var normalizedDrawerCode = NormalizeOptional(drawerCode)?.ToUpperInvariant();
        if (!string.IsNullOrWhiteSpace(channelBatchId))
        {
            var batch = await LoadSelectedBatchByIdAsync(channelBatchId.Trim(), cancellationToken);
            if (!string.Equals(batch.DrawerId, slot.DrawerId, StringComparison.Ordinal))
            {
                throw new BusinessRuleException("channel_slot_mismatch", "Physical slot must belong to the selected channel.", StatusCodes.Status409Conflict);
            }

            if (normalizedDrawerCode is not null && !string.Equals(batch.DrawerCode, normalizedDrawerCode, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessRuleException("channel_slot_mismatch", "drawerCode does not match the selected channel batch.", StatusCodes.Status409Conflict);
            }

            return (slot, batch);
        }

        if (normalizedDrawerCode is null)
        {
            throw new BusinessRuleException("channel_required", "drawerCode or channelBatchId is required.", StatusCodes.Status400BadRequest);
        }

        if (!string.Equals(slot.Drawer?.Code, normalizedDrawerCode, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException("channel_slot_mismatch", "Physical slot must belong to the requested drawer.", StatusCodes.Status409Conflict);
        }

        var activeBatch = await channelBatchWorkflowService.RequireSelectedActiveBatchAsync(normalizedDrawerCode, cancellationToken);
        return (slot, activeBatch);
    }

    private async Task<ChannelBatch> LoadSelectedBatchByIdAsync(string channelBatchId, CancellationToken cancellationToken)
    {
        var batch = await dbContext.ChannelBatches
            .Include(x => x.SlideTasks)
            .ThenInclude(x => x.StainingTask)
            .Include(x => x.SelectedWorkflowVersion)
            .ThenInclude(x => x!.WorkflowDefinition)
            .SingleOrDefaultAsync(x => x.Id == channelBatchId, cancellationToken);
        if (batch is null)
        {
            throw new BusinessRuleException("channel_batch_not_found", "Channel batch was not found.", StatusCodes.Status404NotFound);
        }

        if (!ChannelBatchWorkflowService.IsActiveStatus(batch.Status))
        {
            throw new BusinessRuleException("channel_batch_not_active", "Channel batch is not active.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus == WorkflowSelectionStatus.NeedsManualResolution || batch.NeedsManualResolution)
        {
            throw new BusinessRuleException("channel_batch_needs_manual_resolution", "Channel batch needs manual workflow resolution before it can be used.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus != WorkflowSelectionStatus.Selected || string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId))
        {
            throw new BusinessRuleException("channel_workflow_required", "Select a channel workflow before adding slides.", StatusCodes.Status409Conflict);
        }

        return batch;
    }

    private async Task<WorkflowVersion> LoadPublishedWorkflowVersionAsync(
        string workflowVersionId,
        string workflowType,
        CancellationToken cancellationToken)
    {
        var version = await dbContext.WorkflowVersions
            .Include(x => x.WorkflowDefinition)
            .Include(x => x.Steps)
            .Include(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Id == workflowVersionId, cancellationToken);
        if (version is null)
        {
            throw new BusinessRuleException("workflow_version_not_found", "Workflow version was not found.", StatusCodes.Status404NotFound);
        }

        if (version.Status != WorkflowVersionStatus.Published || version.WorkflowDefinition?.WorkflowType != workflowType)
        {
            throw new BusinessRuleException("workflow_version_not_published", "Selected workflow version must be a published workflow of the requested type.", StatusCodes.Status409Conflict);
        }

        return version;
    }

    private async Task<WorkflowVersion> LoadChannelWorkflowVersionAsync(
        ChannelBatch batch,
        string workflowType,
        CancellationToken cancellationToken)
    {
        if (batch.WorkflowSelectionStatus != WorkflowSelectionStatus.Selected
            || string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId)
            || string.IsNullOrWhiteSpace(batch.WorkflowSnapshotJson)
            || batch.WorkflowSnapshotJson == "{}")
        {
            throw new BusinessRuleException("channel_workflow_required", "Select a channel workflow before adding slides.", StatusCodes.Status409Conflict);
        }

        var version = await LoadPublishedWorkflowVersionAsync(batch.SelectedWorkflowVersionId, workflowType, cancellationToken);
        if (batch.ExperimentType != workflowType)
        {
            throw new BusinessRuleException("channel_experiment_type_mismatch", "Task type must match the selected channel workflow.", StatusCodes.Status409Conflict);
        }

        return version;
    }

    private static StainingTask CreateTask(
        string commandId,
        string taskType,
        PhysicalSlot slot,
        ChannelBatch batch,
        WorkflowVersion version,
        AuthenticatedUser actor,
        string? inputMode,
        string? rawCode,
        string? normalizedCode,
        string? primaryAntibodyCode,
        object candidateResults,
        string? rawSampleCode,
        string? normalizedSampleCode,
        string? lisQueryLogId,
        IReadOnlyList<string> lisCandidatePrimaryAntibodyCodes,
        string? confirmedPrimaryAntibodyCode,
        string? compatibilityValidationStatus,
        string? compatibilityValidationMessage)
    {
        return new StainingTask
        {
            TaskCode = $"{taskType}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..28],
            TaskType = taskType,
            Status = StainingTaskStatus.Confirmed,
            PhysicalSlotId = slot.Id,
            WorkflowDefinitionId = version.WorkflowDefinitionId,
            WorkflowVersionId = batch.SelectedWorkflowVersionId!,
            WorkflowSnapshotJson = batch.WorkflowSnapshotJson,
            InputMode = inputMode,
            RawCode = rawCode,
            NormalizedCode = normalizedCode,
            PrimaryAntibodyCode = primaryAntibodyCode,
            CandidateResultsJson = JsonSerializer.Serialize(candidateResults, JsonOptions),
            RawSampleCode = rawSampleCode,
            NormalizedSampleCode = normalizedSampleCode,
            LisQueryLogId = lisQueryLogId,
            LisCandidatePrimaryAntibodyCodesJson = lisCandidatePrimaryAntibodyCodes.Count == 0
                ? null
                : JsonSerializer.Serialize(lisCandidatePrimaryAntibodyCodes, JsonOptions),
            ConfirmedPrimaryAntibodyCode = confirmedPrimaryAntibodyCode,
            CompatibilityValidationStatus = compatibilityValidationStatus,
            CompatibilityValidationMessage = compatibilityValidationMessage,
            CreatedByUserId = actor.UserId,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private SlideTask AddSlideTask(ChannelBatch batch, StainingTask task, PhysicalSlot slot, string taskType)
    {
        var slideTask = new SlideTask
        {
            ChannelBatch = batch,
            StainingTask = task,
            PhysicalSlotId = slot.Id,
            SlotCode = slot.Code,
            TaskType = taskType,
            Status = RuntimeLedgerStatus.Pending,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        dbContext.SlideTasks.Add(slideTask);
        return slideTask;
    }

    private static void EnsureCanAddSlideToBatch(ChannelBatch batch, string taskType, string? requestedWorkflowVersionId)
    {
        if (batch.WorkflowLockedAtUtc is not null || batch.StartedAtUtc is not null || !string.IsNullOrWhiteSpace(batch.MachineRunId))
        {
            throw new BusinessRuleException("channel_batch_locked", "Cannot add slides after the channel batch has started.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus == WorkflowSelectionStatus.NeedsManualResolution)
        {
            throw new BusinessRuleException("channel_batch_needs_manual_resolution", "Channel batch needs manual workflow resolution.", StatusCodes.Status409Conflict);
        }

        if (batch.WorkflowSelectionStatus != WorkflowSelectionStatus.Selected
            || string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId)
            || string.IsNullOrWhiteSpace(batch.WorkflowSnapshotJson)
            || batch.WorkflowSnapshotJson == "{}")
        {
            throw new BusinessRuleException("channel_workflow_required", "Select a channel workflow before adding slides.", StatusCodes.Status409Conflict);
        }

        if (batch.ExperimentType != taskType)
        {
            throw new BusinessRuleException("channel_experiment_type_mismatch", "All slides in a channel must share the selected experiment type.", StatusCodes.Status409Conflict);
        }

        if (requestedWorkflowVersionId is not null && requestedWorkflowVersionId != batch.SelectedWorkflowVersionId)
        {
            throw new BusinessRuleException("channel_workflow_mismatch", "Slide workflow must match the selected channel workflow.", StatusCodes.Status409Conflict);
        }

        if (batch.SlideTasks.Count >= 4)
        {
            throw new BusinessRuleException("channel_batch_full", "A channel batch can contain at most 4 slides.", StatusCodes.Status409Conflict);
        }
    }

    private async Task<IhcCompatibilityResult> ValidatePrimaryAntibodyCompatibleAsync(
        string primaryAntibodyCode,
        string workflowVersionId,
        CancellationToken cancellationToken)
    {
        var mapping = await dbContext.PrimaryAntibodyWorkflowMappings
            .AsNoTracking()
            .Include(x => x.WorkflowVersion)
            .ThenInclude(x => x!.WorkflowDefinition)
            .SingleOrDefaultAsync(
                x => x.PrimaryAntibodyCode == primaryAntibodyCode && x.WorkflowVersionId == workflowVersionId,
                cancellationToken);
        if (mapping is null)
        {
            return IhcCompatibilityResult.Fail("Selected primary antibody is not mapped to this channel workflow. Select another channel or change the channel workflow before starting the run.");
        }

        if (!mapping.IsEnabled)
        {
            return IhcCompatibilityResult.Fail("Selected primary antibody mapping is disabled. Select another channel or change the channel workflow before starting the run.");
        }

        if (mapping.WorkflowVersion?.Status != WorkflowVersionStatus.Published
            || mapping.WorkflowVersion.WorkflowDefinition?.WorkflowType != StainingTaskType.Ihc)
        {
            return IhcCompatibilityResult.Fail("Selected primary antibody mapping does not point to a published IHC workflow. Select another channel or change the channel workflow before starting the run.");
        }

        return IhcCompatibilityResult.Success("Primary antibody is compatible with the selected channel workflow.");
    }

    private async Task PersistIhcCompatibilityFailureAuditAsync(
        AuthenticatedUser actor,
        IhcCompatibilityFailureAudit failure,
        CancellationToken cancellationToken)
    {
        dbContext.ChangeTracker.Clear();
        AddAudit(actor, "task.ihc.compatibility_failed", "ChannelBatch", failure.ChannelBatchId, new
        {
            failure.DrawerCode,
            failure.SlotCode,
            failure.RawSampleCode,
            failure.NormalizedSampleCode,
            failure.ConfirmedPrimaryAntibodyCode,
            inheritedWorkflowVersionId = failure.WorkflowVersionId,
            compatibilityValidationStatus = CompatibilityIncompatible,
            compatibilityValidationMessage = failure.Message,
            commandId = failure.CommandId
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityType, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private void PublishSlideTaskCreated(ChannelBatch batch, StainingTask task, PhysicalSlot slot, SlideTask slideTask)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.SlideTaskCreated,
            batch.MachineRunId,
            "SlideTask",
            slideTask.Id,
            null,
            new Dictionary<string, object?>
            {
                ["channelBatchId"] = batch.Id,
                ["drawerCode"] = batch.DrawerCode,
                ["slotCode"] = slot.Code,
                ["slideTaskId"] = slideTask.Id,
                ["stainingTaskId"] = task.Id,
                ["taskType"] = task.TaskType,
                ["workflowVersionId"] = batch.SelectedWorkflowVersionId
            }));
    }

    private static TaskCreationResponse CreatedResponse(string commandId, bool replayed, StainingTask task, ChannelBatch batch, string message)
    {
        return new TaskCreationResponse(
            true,
            commandId,
            replayed,
            false,
            message,
            task.Id,
            task.TaskCode,
            [],
            [],
            batch.Id,
            batch.DrawerCode,
            batch.ExperimentType,
            batch.SelectedWorkflowVersionId,
            batch.WorkflowSelectionStatus,
            task.CompatibilityValidationStatus,
            task.CompatibilityValidationMessage);
    }

    private static string NormalizeHospitalCode(string rawCode)
    {
        var normalized = (rawCode ?? string.Empty).Trim(' ', '\r', '\n');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("hospital_code_required", "Hospital barcode is required.");
        }

        return normalized;
    }

    private static string? GetLegacyWorkflowVersionId(CreateIhcTaskRequest request)
    {
        var selectedWorkflowVersionId = NormalizeOptional(request.SelectedWorkflowVersionId);
        var workflowVersionId = NormalizeOptional(request.WorkflowVersionId);
        if (selectedWorkflowVersionId is not null
            && workflowVersionId is not null
            && selectedWorkflowVersionId != workflowVersionId)
        {
            throw new BusinessRuleException("legacy_workflow_version_conflict", "Legacy workflow version fields must match when both are provided.", StatusCodes.Status409Conflict);
        }

        return selectedWorkflowVersionId ?? workflowVersionId;
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private sealed record IhcResolution(
        bool RequiresSelection,
        string Message,
        string NormalizedCode,
        string? PrimaryAntibodyCode,
        IReadOnlyList<string> CandidatePrimaryAntibodyCodes)
    {
        public static IhcResolution Selection(
            string message,
            string normalizedCode,
            IReadOnlyList<string> candidatePrimaryAntibodyCodes)
        {
            return new IhcResolution(true, message, normalizedCode, null, candidatePrimaryAntibodyCodes);
        }

        public static IhcResolution Final(
            string normalizedCode,
            string primaryAntibodyCode,
            IReadOnlyList<string> candidatePrimaryAntibodyCodes)
        {
            return new IhcResolution(false, string.Empty, normalizedCode, primaryAntibodyCode, candidatePrimaryAntibodyCodes);
        }
    }

    private sealed record IhcCompatibilityResult(bool Ok, string Message)
    {
        public static IhcCompatibilityResult Success(string message)
        {
            return new IhcCompatibilityResult(true, message);
        }

        public static IhcCompatibilityResult Fail(string message)
        {
            return new IhcCompatibilityResult(false, message);
        }
    }

    private sealed record IhcCompatibilityFailureAudit(
        string ChannelBatchId,
        string DrawerCode,
        string SlotCode,
        string RawSampleCode,
        string NormalizedSampleCode,
        string ConfirmedPrimaryAntibodyCode,
        string WorkflowVersionId,
        string Message,
        string CommandId);
}
