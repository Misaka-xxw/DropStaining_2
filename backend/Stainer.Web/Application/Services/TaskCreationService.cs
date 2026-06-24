using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class TaskCreationService(StainerDbContext dbContext, CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

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
                var slot = await LoadIdleSlotAsync(request.SlotCode, cancellationToken);
                var version = await LoadPublishedWorkflowVersionAsync(request.WorkflowVersionId, StainingTaskType.He, cancellationToken);
                var task = CreateTask(
                    request.CommandId,
                    StainingTaskType.He,
                    slot,
                    version,
                    actor,
                    inputMode: "ManualHE",
                    rawCode: null,
                    normalizedCode: null,
                    primaryAntibodyCode: null,
                    candidateResults: Array.Empty<object>());
                dbContext.StainingTasks.Add(task);
                AddAudit(actor, "task.create_he", task.Id, new { task.TaskCode, slot = slot.Code, workflowVersionId = version.Id });
                return new CommandExecutionResult<TaskCreationResponse>(
                    CreatedResponse(request.CommandId, false, task, "HE task created."),
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
        var resolution = await ResolveIhcSelectionAsync(request, cancellationToken);
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
                resolution.CandidateWorkflows));
        }

        return await idempotencyService.RunAsync(
            request.CommandId,
            "task.create_ihc",
            request,
            actor,
            async () =>
            {
                var slot = await LoadIdleSlotAsync(request.SlotCode, cancellationToken);
                var version = await LoadPublishedWorkflowVersionAsync(resolution.WorkflowVersionId!, StainingTaskType.Ihc, cancellationToken);
                var task = CreateTask(
                    request.CommandId,
                    StainingTaskType.Ihc,
                    slot,
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
                            candidatePrimaryAntibodyCodes = resolution.CandidatePrimaryAntibodyCodes,
                            candidateWorkflows = resolution.CandidateWorkflows,
                            selectedPrimaryAntibodyCode = resolution.PrimaryAntibodyCode,
                            selectedWorkflowVersionId = resolution.WorkflowVersionId
                        }
                    });
                dbContext.StainingTasks.Add(task);
                AddAudit(actor, "task.create_ihc", task.Id, new
                {
                    task.TaskCode,
                    slot = slot.Code,
                    task.RawCode,
                    task.NormalizedCode,
                    task.PrimaryAntibodyCode,
                    workflowVersionId = version.Id
                });
                return new CommandExecutionResult<TaskCreationResponse>(
                    CreatedResponse(request.CommandId, false, task, "IHC task created."),
                    "StainingTask",
                    task.Id);
            },
            cancellationToken);
    }

    private async Task<IhcResolution> ResolveIhcSelectionAsync(CreateIhcTaskRequest request, CancellationToken cancellationToken)
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
                    candidates,
                    []);
            }

            var selected = string.IsNullOrWhiteSpace(request.SelectedPrimaryAntibodyCode)
                ? candidates.Single()
                : request.SelectedPrimaryAntibodyCode.Trim();
            if (!candidates.Contains(selected, StringComparer.OrdinalIgnoreCase))
            {
                throw new BusinessRuleException("invalid_primary_antibody_selection", "Selected primary antibody code is not in the LIS candidate list.");
            }

            return await ResolveWorkflowSelectionAsync(request, normalized, selected, candidates, cancellationToken);
        }

        var directCode = (request.RawCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(directCode))
        {
            throw new BusinessRuleException("primary_antibody_required", "Primary antibody code is required.");
        }

        return await ResolveWorkflowSelectionAsync(request, directCode, directCode, [directCode], cancellationToken);
    }

    private async Task<IhcResolution> ResolveWorkflowSelectionAsync(
        CreateIhcTaskRequest request,
        string normalizedCode,
        string primaryAntibodyCode,
        IReadOnlyList<string> antibodyCandidates,
        CancellationToken cancellationToken)
    {
        var workflowRows = await dbContext.PrimaryAntibodyWorkflowMappings
            .AsNoTracking()
            .Where(x => x.IsEnabled && x.PrimaryAntibodyCode == primaryAntibodyCode)
            .Include(x => x.WorkflowVersion)
            .ThenInclude(x => x!.WorkflowDefinition)
            .Select(x => new
            {
                x.WorkflowVersionId,
                x.WorkflowVersion!.WorkflowDefinitionId,
                WorkflowCode = x.WorkflowVersion.WorkflowDefinition!.Code,
                WorkflowName = x.WorkflowVersion.WorkflowDefinition.Name,
                x.WorkflowVersion.VersionLabel,
                x.WorkflowVersion.Status,
                WorkflowType = x.WorkflowVersion.WorkflowDefinition.WorkflowType
            })
            .OrderBy(x => x.WorkflowCode)
            .ThenBy(x => x.VersionLabel)
            .ToListAsync(cancellationToken);

        var workflowCandidates = workflowRows
            .Where(x => x.Status == WorkflowVersionStatus.Published && x.WorkflowType == StainingTaskType.Ihc)
            .Select(x => new TaskWorkflowCandidateResponse(
                x.WorkflowVersionId,
                x.WorkflowDefinitionId,
                x.WorkflowCode,
                x.WorkflowName,
                x.VersionLabel))
            .ToList();

        if (workflowCandidates.Count == 0)
        {
            throw new BusinessRuleException("ihc_workflow_not_found", "No published IHC workflow is mapped to the selected primary antibody code.", StatusCodes.Status404NotFound);
        }

        if (workflowCandidates.Count > 1 && string.IsNullOrWhiteSpace(request.SelectedWorkflowVersionId))
        {
            return IhcResolution.Selection(
                "Multiple IHC workflows are mapped to this primary antibody code. Operator selection is required.",
                normalizedCode,
                antibodyCandidates,
                workflowCandidates);
        }

        var workflowVersionId = string.IsNullOrWhiteSpace(request.SelectedWorkflowVersionId)
            ? workflowCandidates.Single().WorkflowVersionId
            : request.SelectedWorkflowVersionId.Trim();
        if (!workflowCandidates.Any(x => x.WorkflowVersionId == workflowVersionId))
        {
            throw new BusinessRuleException("invalid_workflow_selection", "Selected workflow version is not in the candidate list.");
        }

        return IhcResolution.Final(
            normalizedCode,
            primaryAntibodyCode,
            workflowVersionId,
            antibodyCandidates,
            workflowCandidates);
    }

    private async Task<PhysicalSlot> LoadIdleSlotAsync(string slotCode, CancellationToken cancellationToken)
    {
        var normalized = (slotCode ?? string.Empty).Trim();
        var slot = await dbContext.PhysicalSlots.SingleOrDefaultAsync(x => x.Code == normalized, cancellationToken);
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

    private static StainingTask CreateTask(
        string commandId,
        string taskType,
        PhysicalSlot slot,
        WorkflowVersion version,
        AuthenticatedUser actor,
        string? inputMode,
        string? rawCode,
        string? normalizedCode,
        string? primaryAntibodyCode,
        object candidateResults)
    {
        return new StainingTask
        {
            TaskCode = $"{taskType}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..28],
            TaskType = taskType,
            Status = StainingTaskStatus.Confirmed,
            PhysicalSlotId = slot.Id,
            WorkflowDefinitionId = version.WorkflowDefinitionId,
            WorkflowVersionId = version.Id,
            WorkflowSnapshotJson = JsonSerializer.Serialize(ToWorkflowSnapshot(version), JsonOptions),
            InputMode = inputMode,
            RawCode = rawCode,
            NormalizedCode = normalizedCode,
            PrimaryAntibodyCode = primaryAntibodyCode,
            CandidateResultsJson = JsonSerializer.Serialize(candidateResults, JsonOptions),
            CreatedByUserId = actor.UserId,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private static object ToWorkflowSnapshot(WorkflowVersion version)
    {
        return new
        {
            workflowDefinitionId = version.WorkflowDefinitionId,
            workflowCode = version.WorkflowDefinition?.Code,
            workflowName = version.WorkflowDefinition?.Name,
            workflowType = version.WorkflowDefinition?.WorkflowType,
            workflowVersionId = version.Id,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            steps = version.Steps.OrderBy(x => x.StepNo).Select(x => new
            {
                x.StepNo,
                x.MajorStepCode,
                x.StepName,
                x.ActionType,
                x.ReagentCode,
                x.VolumeUl,
                x.DurationSeconds,
                x.TargetTemperatureDeciC,
                x.FailureStrategy
            }),
            reagentRequirements = version.ReagentRequirements.OrderBy(x => x.ReagentCode).Select(x => new
            {
                x.ReagentCode,
                x.RequiredVolumeUl,
                x.IsRequired
            })
        };
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = "StainingTask",
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static TaskCreationResponse CreatedResponse(string commandId, bool replayed, StainingTask task, string message)
    {
        return new TaskCreationResponse(true, commandId, replayed, false, message, task.Id, task.TaskCode, [], []);
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

    private sealed record IhcResolution(
        bool RequiresSelection,
        string Message,
        string NormalizedCode,
        string? PrimaryAntibodyCode,
        string? WorkflowVersionId,
        IReadOnlyList<string> CandidatePrimaryAntibodyCodes,
        IReadOnlyList<TaskWorkflowCandidateResponse> CandidateWorkflows)
    {
        public static IhcResolution Selection(
            string message,
            string normalizedCode,
            IReadOnlyList<string> candidatePrimaryAntibodyCodes,
            IReadOnlyList<TaskWorkflowCandidateResponse> candidateWorkflows)
        {
            return new IhcResolution(true, message, normalizedCode, null, null, candidatePrimaryAntibodyCodes, candidateWorkflows);
        }

        public static IhcResolution Final(
            string normalizedCode,
            string primaryAntibodyCode,
            string workflowVersionId,
            IReadOnlyList<string> candidatePrimaryAntibodyCodes,
            IReadOnlyList<TaskWorkflowCandidateResponse> candidateWorkflows)
        {
            return new IhcResolution(false, string.Empty, normalizedCode, primaryAntibodyCode, workflowVersionId, candidatePrimaryAntibodyCodes, candidateWorkflows);
        }
    }
}
