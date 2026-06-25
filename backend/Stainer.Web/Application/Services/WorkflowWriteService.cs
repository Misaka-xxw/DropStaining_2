using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class WorkflowWriteService(StainerDbContext dbContext, CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<WorkflowDraftMutationResponse> CreateDraftAsync(
        CreateWorkflowDraftRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "workflow.draft.create",
            request,
            actor,
            async () =>
            {
                var result = string.IsNullOrWhiteSpace(request.SourceWorkflowId)
                    ? await CreateBlankDraftAsync(request, actor, cancellationToken)
                    : await CopyLatestVersionAsDraftAsync(request, actor, cancellationToken);

                return new CommandExecutionResult<WorkflowDraftMutationResponse>(
                    result,
                    "WorkflowVersion",
                    result.WorkflowVersionId);
            },
            cancellationToken);
    }

    private async Task<WorkflowDraftMutationResponse> CreateBlankDraftAsync(
        CreateWorkflowDraftRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var code = RequireValue(request.Code, "code");
        var name = RequireValue(request.Name, "name");
        var workflowType = NormalizeWorkflowType(request.WorkflowType);
        var description = OptionalValue(request.Description, "管理员新建流程草稿。");
        var versionLabel = OptionalValue(request.VersionLabel, "0.1");
        var changeNote = OptionalValue(request.ChangeNote, "创建空白草稿。");

        if (await dbContext.WorkflowDefinitions.AnyAsync(x => x.Code == code, cancellationToken))
        {
            throw new BusinessRuleException("workflow_code_exists", "Workflow code already exists.", StatusCodes.Status409Conflict);
        }

        var definition = new WorkflowDefinition
        {
            Code = code,
            Name = name,
            WorkflowType = workflowType,
            Description = description,
            CreatedAtUtc = now
        };
        var version = new WorkflowVersion
        {
            WorkflowDefinition = definition,
            VersionNo = 1,
            VersionLabel = versionLabel,
            Status = WorkflowVersionStatus.Draft,
            ChangeNote = changeNote,
            CreatedAtUtc = now
        };
        definition.Versions.Add(version);
        dbContext.WorkflowDefinitions.Add(definition);

        AddAudit(actor, "workflow.draft.create", version.Id, new
        {
            sourceWorkflowId = (string?)null,
            workflowDefinitionId = definition.Id,
            definition.Code,
            definition.Name,
            definition.WorkflowType,
            version.VersionNo,
            version.VersionLabel,
            version.Status
        });

        return ToResponse(request.CommandId, definition, version, "Workflow draft created.");
    }

    private async Task<WorkflowDraftMutationResponse> CopyLatestVersionAsDraftAsync(
        CreateWorkflowDraftRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken)
    {
        var sourceWorkflowId = RequireValue(request.SourceWorkflowId, "sourceWorkflowId");
        var workflow = await dbContext.WorkflowDefinitions
            .Include(x => x.Versions)
            .ThenInclude(x => x.Steps)
            .Include(x => x.Versions)
            .ThenInclude(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Id == sourceWorkflowId, cancellationToken);

        if (workflow is null)
        {
            throw new BusinessRuleException("workflow_not_found", "Workflow was not found.", StatusCodes.Status404NotFound);
        }

        var sourceVersion = workflow.Versions
            .OrderByDescending(x => x.VersionNo)
            .FirstOrDefault();
        var versionNo = workflow.Versions
            .Select(x => x.VersionNo)
            .DefaultIfEmpty(0)
            .Max() + 1;
        var versionLabel = OptionalValue(request.VersionLabel, NextVersionLabel(sourceVersion, versionNo));

        if (workflow.Versions.Any(x => x.VersionLabel == versionLabel))
        {
            throw new BusinessRuleException("workflow_version_label_exists", "Workflow version label already exists.", StatusCodes.Status409Conflict);
        }

        var now = DateTimeOffset.UtcNow;
        var version = new WorkflowVersion
        {
            WorkflowDefinitionId = workflow.Id,
            VersionNo = versionNo,
            VersionLabel = versionLabel,
            Status = WorkflowVersionStatus.Draft,
            ChangeNote = OptionalValue(request.ChangeNote, $"Copied from {sourceVersion?.VersionLabel ?? "empty workflow"}."),
            CreatedAtUtc = now
        };

        if (sourceVersion is not null)
        {
            foreach (var step in sourceVersion.Steps.OrderBy(x => x.StepNo))
            {
                version.Steps.Add(new WorkflowStep
                {
                    StepNo = step.StepNo,
                    MajorStepCode = step.MajorStepCode,
                    StepName = step.StepName,
                    ActionType = step.ActionType,
                    ReagentCode = step.ReagentCode,
                    VolumeUl = step.VolumeUl,
                    DurationSeconds = step.DurationSeconds,
                    TargetTemperatureDeciC = step.TargetTemperatureDeciC,
                    MixParametersJson = step.MixParametersJson,
                    WashParametersJson = step.WashParametersJson,
                    LegacyParametersJson = step.LegacyParametersJson,
                    FailureStrategy = step.FailureStrategy,
                    CreatedAtUtc = now
                });
            }

            foreach (var requirement in sourceVersion.ReagentRequirements.OrderBy(x => x.ReagentCode))
            {
                version.ReagentRequirements.Add(new WorkflowReagentRequirement
                {
                    ReagentCode = requirement.ReagentCode,
                    RequiredVolumeUl = requirement.RequiredVolumeUl,
                    IsRequired = requirement.IsRequired,
                    CreatedAtUtc = now
                });
            }
        }

        workflow.Versions.Add(version);
        AddAudit(actor, "workflow.draft.copy", version.Id, new
        {
            sourceWorkflowId = workflow.Id,
            sourceWorkflowVersionId = sourceVersion?.Id,
            workflowDefinitionId = workflow.Id,
            workflow.Code,
            workflow.Name,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            stepCount = version.Steps.Count,
            reagentRequirementCount = version.ReagentRequirements.Count
        });

        return ToResponse(request.CommandId, workflow, version, "Workflow draft copied.");
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = "WorkflowVersion",
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static WorkflowDraftMutationResponse ToResponse(
        string commandId,
        WorkflowDefinition definition,
        WorkflowVersion version,
        string message)
    {
        return new WorkflowDraftMutationResponse(
            true,
            commandId,
            false,
            definition.Id,
            version.Id,
            definition.Code,
            definition.Name,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            message);
    }

    private static string NextVersionLabel(WorkflowVersion? sourceVersion, int versionNo)
    {
        if (sourceVersion is null)
        {
            return "0.1";
        }

        var label = sourceVersion.VersionLabel.Trim();
        var parts = label.Split('.', 2);
        return int.TryParse(parts[0], out var major)
            ? $"{major + 1}.0"
            : versionNo.ToString();
    }

    private static string NormalizeWorkflowType(string? value)
    {
        var workflowType = RequireValue(value, "workflowType").ToUpperInvariant();
        if (workflowType is not (StainingTaskType.He or StainingTaskType.Ihc))
        {
            throw new BusinessRuleException("workflow_type_invalid", "Workflow type must be HE or IHC.");
        }

        return workflowType;
    }

    private static string RequireValue(string? value, string fieldName)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException($"{fieldName}_required", $"{fieldName} is required.");
        }

        return normalized;
    }

    private static string OptionalValue(string? value, string fallback)
    {
        var normalized = value?.Trim() ?? string.Empty;
        return string.IsNullOrWhiteSpace(normalized) ? fallback : normalized;
    }
}
