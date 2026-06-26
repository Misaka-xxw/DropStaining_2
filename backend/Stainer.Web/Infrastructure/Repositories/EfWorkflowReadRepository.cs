using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Repositories;

public sealed class EfWorkflowReadRepository(StainerDbContext dbContext) : IWorkflowReadRepository
{
    public async Task<IReadOnlyList<WorkflowSummaryResponse>> ListWorkflowsAsync(CancellationToken cancellationToken = default)
    {
        var workflows = await dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Include(x => x.Versions)
            .ThenInclude(x => x.Steps)
            .Include(x => x.Versions)
            .ThenInclude(x => x.ReagentRequirements)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return workflows
            .Select(x => new WorkflowSummaryResponse(
                x.Id,
                x.Code,
                x.Name,
                x.WorkflowType,
                x.Description,
                x.IsEnabled,
                x.Versions
                    .OrderBy(v => v.VersionNo)
                    .Select(v => new WorkflowVersionSummaryResponse(
                        v.Id,
                        v.VersionNo,
                        v.VersionLabel,
                        v.Status,
                        v.PublishedAtUtc,
                        v.RetiredAtUtc,
                        v.CreatedAtUtc,
                        v.UpdatedAtUtc,
                        v.Steps.Count,
                        v.ReagentRequirements.Count))
                    .ToList()))
            .ToList();
    }

    public async Task<WorkflowDetailResponse?> GetWorkflowAsync(string id, CancellationToken cancellationToken = default)
    {
        var workflow = await dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Include(x => x.Versions)
            .ThenInclude(x => x.Steps)
            .Include(x => x.Versions)
            .ThenInclude(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (workflow is null)
        {
            return null;
        }

        return new WorkflowDetailResponse(
            workflow.Id,
            workflow.Code,
            workflow.Name,
            workflow.WorkflowType,
            workflow.Description,
            workflow.IsEnabled,
            workflow.Versions
                .OrderBy(x => x.VersionNo)
                .Select(x => new WorkflowVersionDetailResponse(
                    x.Id,
                    x.VersionNo,
                    x.VersionLabel,
                    x.Status,
                    x.ChangeNote,
                    x.PublishedAtUtc,
                    x.RetiredAtUtc,
                    x.CreatedAtUtc,
                    x.UpdatedAtUtc,
                    x.Steps
                        .OrderBy(step => step.StepNo)
                        .Select(step => new WorkflowStepResponse(
                            step.Id,
                            step.StepNo,
                            step.MajorStepCode,
                            step.StepName,
                            step.ActionType,
                            step.ReagentCode,
                            step.VolumeUl,
                            step.DurationSeconds,
                            step.TargetTemperatureDeciC,
                            step.FailureStrategy))
                        .ToList(),
                    x.ReagentRequirements
                        .OrderBy(requirement => requirement.ReagentCode)
                        .Select(requirement => new WorkflowReagentRequirementResponse(
                            requirement.Id,
                            requirement.ReagentCode,
                            null,
                            requirement.RequiredVolumeUl,
                            requirement.IsRequired))
                        .ToList()))
                .ToList());
    }

    public async Task<IReadOnlyList<ProtocolCompatResponse>> ListProtocolCompatAsync(CancellationToken cancellationToken = default)
    {
        var workflows = await dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Include(x => x.Versions)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return workflows
            .Select(x =>
            {
                var version = x.Versions
                    .Where(v => v.Status == "Published")
                    .OrderByDescending(v => v.VersionNo)
                    .FirstOrDefault()
                    ?? x.Versions.OrderByDescending(v => v.VersionNo).FirstOrDefault();
                return new ProtocolCompatResponse(
                    x.Id,
                    x.Code,
                    x.Name,
                    version?.VersionLabel ?? string.Empty,
                    x.Description,
                    version?.Status ?? string.Empty);
            })
            .ToList();
    }
}
