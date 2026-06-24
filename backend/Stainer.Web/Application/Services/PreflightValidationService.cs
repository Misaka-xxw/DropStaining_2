using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class PreflightValidationService(StainerDbContext dbContext)
{
    public async Task<PreflightValidationReportResponse> ValidateAsync(CancellationToken cancellationToken = default)
    {
        var issues = new List<PreflightValidationIssueResponse>();
        var tasks = await dbContext.StainingTasks
            .AsNoTracking()
            .Where(x => x.Status == StainingTaskStatus.Confirmed)
            .ToListAsync(cancellationToken);

        if (tasks.Count == 0)
        {
            issues.Add(new PreflightValidationIssueResponse("Tasks", "no_confirmed_tasks", "No confirmed staining tasks were found."));
        }

        var workflowVersionIds = tasks.Select(x => x.WorkflowVersionId).Distinct().ToList();
        var workflowVersions = await dbContext.WorkflowVersions
            .AsNoTracking()
            .Where(x => workflowVersionIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (var task in tasks)
        {
            if (!workflowVersions.TryGetValue(task.WorkflowVersionId, out var version))
            {
                issues.Add(new PreflightValidationIssueResponse("Workflow", "workflow_version_missing", $"Workflow version is missing for task {task.TaskCode}."));
            }
            else if (version.Status != WorkflowVersionStatus.Published)
            {
                issues.Add(new PreflightValidationIssueResponse("Workflow", "workflow_version_not_published", $"Workflow version is not published for task {task.TaskCode}."));
            }
        }

        var scanSessions = await dbContext.ReagentScanSessions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var latestScan = scanSessions
            .OrderByDescending(x => x.CompletedAtUtc ?? x.StartedAtUtc)
            .FirstOrDefault();
        if (latestScan is null)
        {
            issues.Add(new PreflightValidationIssueResponse("Reagents", "scan_missing", "No reagent scan confirmation was found."));
        }
        else
        {
            var invalidItems = await dbContext.ReagentScanItems
                .AsNoTracking()
                .Where(x => x.ReagentScanSessionId == latestScan.Id && x.ScanResult == ReagentScanResult.Invalid)
                .CountAsync(cancellationToken);
            if (invalidItems > 0)
            {
                issues.Add(new PreflightValidationIssueResponse("Reagents", "scan_has_invalid_items", $"Latest reagent scan contains {invalidItems} invalid item(s)."));
            }
        }

        var requirements = await dbContext.WorkflowReagentRequirements
            .AsNoTracking()
            .Where(x => workflowVersionIds.Contains(x.WorkflowVersionId) && x.IsRequired)
            .GroupBy(x => x.ReagentCode)
            .Select(x => new
            {
                ReagentCode = x.Key,
                RequiredVolumeUl = x.Sum(r => r.RequiredVolumeUl ?? 0)
            })
            .ToListAsync(cancellationToken);

        if (requirements.Count > 0)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var bottles = await dbContext.ReagentRackPlacements
                .AsNoTracking()
                .Where(x => x.RemovedAtUtc == null)
                .Include(x => x.ReagentBottle)
                .Select(x => x.ReagentBottle!)
                .Where(x => x.Status == "Available")
                .ToListAsync(cancellationToken);

            foreach (var requirement in requirements)
            {
                var matching = bottles.Where(x => x.ReagentCode == requirement.ReagentCode).ToList();
                if (matching.Count == 0)
                {
                    issues.Add(new PreflightValidationIssueResponse("Reagents", "required_reagent_missing", $"Required reagent is missing: {requirement.ReagentCode}."));
                    continue;
                }

                if (matching.Any(x => x.ExpirationDate < today))
                {
                    issues.Add(new PreflightValidationIssueResponse("Reagents", "required_reagent_expired", $"Required reagent is expired: {requirement.ReagentCode}."));
                }

                var availableVolume = matching.Where(x => x.ExpirationDate >= today).Sum(x => x.RemainingVolumeUl);
                if (availableVolume < requirement.RequiredVolumeUl)
                {
                    issues.Add(new PreflightValidationIssueResponse(
                        "Reagents",
                        "required_reagent_volume_insufficient",
                        $"Required reagent volume is insufficient: {requirement.ReagentCode}, required {requirement.RequiredVolumeUl} ul, available {availableVolume} ul."));
                }
            }
        }

        return new PreflightValidationReportResponse(issues.Count == 0, tasks.Count, issues.Count, issues);
    }
}
