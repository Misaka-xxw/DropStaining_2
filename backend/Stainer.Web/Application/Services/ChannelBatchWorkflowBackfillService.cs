using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class ChannelBatchWorkflowBackfillService(
    StainerDbContext dbContext,
    ILogger<ChannelBatchWorkflowBackfillService> logger)
{
    public async Task<ChannelBatchWorkflowBackfillReport> BackfillAsync(CancellationToken cancellationToken = default)
    {
        var batches = await dbContext.ChannelBatches
            .Include(x => x.SlideTasks)
            .ThenInclude(x => x.StainingTask)
            .ToListAsync(cancellationToken);

        var backfilled = 0;
        var manual = 0;
        var reasonCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var batch in batches)
        {
            var analysis = Analyze(batch);
            if (analysis.IsSafe)
            {
                if (ApplySafeBackfill(batch, analysis))
                {
                    backfilled++;
                }

                continue;
            }

            if (analysis.IsEmptyBatch)
            {
                ApplyEmptyBatchNormalization(batch, analysis);
                continue;
            }

            manual++;
            foreach (var reason in analysis.Reasons)
            {
                reasonCounts[reason] = reasonCounts.GetValueOrDefault(reason) + 1;
            }

            ApplyManualResolution(batch, analysis.Reasons);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var report = new ChannelBatchWorkflowBackfillReport(
            batches.Count,
            backfilled,
            manual,
            reasonCounts);
        logger.LogInformation(
            "ChannelBatch workflow backfill scanned {ScannedChannelBatchCount} batch(es), backfilled {BackfilledCount}, needs manual resolution {NeedsManualResolutionCount}. Reasons: {ReasonCounts}",
            report.ScannedChannelBatchCount,
            report.BackfilledCount,
            report.NeedsManualResolutionCount,
            string.Join(", ", report.ReasonCounts.Select(x => $"{x.Key}={x.Value}")));
        return report;
    }

    private static bool ApplySafeBackfill(ChannelBatch batch, BatchWorkflowAnalysis analysis)
    {
        var changed = false;
        var desiredStatus = IsLocked(batch)
            ? WorkflowSelectionStatus.Locked
            : WorkflowSelectionStatus.Selected;

        if (batch.ExperimentType != analysis.ExperimentType)
        {
            batch.ExperimentType = analysis.ExperimentType;
            changed = true;
        }

        if (batch.SelectedWorkflowVersionId != analysis.WorkflowVersionId)
        {
            batch.SelectedWorkflowVersionId = analysis.WorkflowVersionId;
            changed = true;
        }

        if (batch.WorkflowSnapshotJson != analysis.WorkflowSnapshotJson)
        {
            batch.WorkflowSnapshotJson = analysis.WorkflowSnapshotJson ?? "{}";
            changed = true;
        }

        if (batch.WorkflowSelectionStatus != desiredStatus)
        {
            batch.WorkflowSelectionStatus = desiredStatus;
            changed = true;
        }

        if (batch.NeedsManualResolution)
        {
            batch.NeedsManualResolution = false;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(batch.ManualResolutionReason))
        {
            batch.ManualResolutionReason = string.Empty;
            changed = true;
        }

        return changed;
    }

    private static bool ApplyEmptyBatchNormalization(ChannelBatch batch, BatchWorkflowAnalysis analysis)
    {
        var changed = false;
        var desiredStatus = analysis.HasCompleteWorkflowSelection
            ? IsLocked(batch) ? WorkflowSelectionStatus.Locked : WorkflowSelectionStatus.Selected
            : WorkflowSelectionStatus.Unselected;

        if (batch.WorkflowSelectionStatus != desiredStatus)
        {
            batch.WorkflowSelectionStatus = desiredStatus;
            changed = true;
        }

        if (batch.NeedsManualResolution)
        {
            batch.NeedsManualResolution = false;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(batch.ManualResolutionReason))
        {
            batch.ManualResolutionReason = string.Empty;
            changed = true;
        }

        if (!analysis.HasCompleteWorkflowSelection)
        {
            if (!string.IsNullOrWhiteSpace(batch.ExperimentType))
            {
                batch.ExperimentType = null;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId))
            {
                batch.SelectedWorkflowVersionId = null;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(batch.WorkflowSnapshotJson))
            {
                batch.WorkflowSnapshotJson = "{}";
                changed = true;
            }
        }

        return changed;
    }

    private static void ApplyManualResolution(ChannelBatch batch, IReadOnlyList<string> reasons)
    {
        var reasonText = string.Join("; ", reasons);
        batch.NeedsManualResolution = true;
        batch.ManualResolutionReason = reasonText;
        batch.WorkflowSelectionStatus = WorkflowSelectionStatus.NeedsManualResolution;
    }

    private static BatchWorkflowAnalysis Analyze(ChannelBatch batch)
    {
        if (batch.SlideTasks.Count == 0)
        {
            return AnalyzeEmptyBatch(batch);
        }

        var tasks = batch.SlideTasks.Select(x => x.StainingTask).ToList();
        var reasons = new List<string>();
        if (tasks.Any(x => x is null))
        {
            reasons.Add("MissingStainingTask");
        }

        var existingTasks = tasks.Where(x => x is not null).Cast<StainingTask>().ToList();
        var experimentTypes = existingTasks
            .Select(x => Normalize(x.TaskType))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (experimentTypes.Count == 0)
        {
            reasons.Add("CannotDetermineExperimentType");
        }
        else if (experimentTypes.Count > 1)
        {
            reasons.Add("MixedExperimentType");
        }

        var missingWorkflowVersion = existingTasks.Any(x => string.IsNullOrWhiteSpace(x.WorkflowVersionId));
        if (missingWorkflowVersion)
        {
            reasons.Add("MissingWorkflowVersion");
        }

        var workflowVersionIds = existingTasks
            .Select(x => Normalize(x.WorkflowVersionId))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (workflowVersionIds.Count > 1)
        {
            reasons.Add("MultipleWorkflowVersions");
        }

        var snapshots = existingTasks
            .Select(x => NormalizeSnapshot(x.WorkflowSnapshotJson))
            .Where(x => x is not null)
            .Cast<string>()
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (snapshots.Count == 0)
        {
            reasons.Add("MissingWorkflowSnapshot");
        }
        else if (snapshots.Count > 1)
        {
            reasons.Add("WorkflowSnapshotConflict");
        }

        if (reasons.Count > 0)
        {
            return BatchWorkflowAnalysis.Manual(reasons.Distinct(StringComparer.Ordinal).ToList());
        }

        return BatchWorkflowAnalysis.Safe(experimentTypes.Single(), workflowVersionIds.Single(), snapshots.Single());
    }

    private static BatchWorkflowAnalysis AnalyzeEmptyBatch(ChannelBatch batch)
    {
        var hasExperimentType = !string.IsNullOrWhiteSpace(batch.ExperimentType);
        var hasWorkflowVersion = !string.IsNullOrWhiteSpace(batch.SelectedWorkflowVersionId);
        var hasWorkflowSnapshot = NormalizeSnapshot(batch.WorkflowSnapshotJson) is not null;
        var hasAnyWorkflowSelection = hasExperimentType || hasWorkflowVersion || hasWorkflowSnapshot;

        if (!hasAnyWorkflowSelection)
        {
            return BatchWorkflowAnalysis.Empty(false);
        }

        if (hasExperimentType && hasWorkflowVersion && hasWorkflowSnapshot)
        {
            return BatchWorkflowAnalysis.Empty(true);
        }

        return BatchWorkflowAnalysis.Manual(["IncompleteChannelWorkflowSelection"]);
    }

    private static bool IsLocked(ChannelBatch batch)
    {
        return !string.IsNullOrWhiteSpace(batch.MachineRunId) || batch.WorkflowLockedAtUtc is not null;
    }

    private static string? NormalizeSnapshot(string? snapshot)
    {
        var normalized = Normalize(snapshot);
        return string.IsNullOrWhiteSpace(normalized) || normalized == "{}" ? null : normalized;
    }

    private static string Normalize(string? value)
    {
        return (value ?? string.Empty).Trim();
    }

    private sealed record BatchWorkflowAnalysis(
        bool IsSafe,
        bool IsEmptyBatch,
        bool HasCompleteWorkflowSelection,
        string? ExperimentType,
        string? WorkflowVersionId,
        string? WorkflowSnapshotJson,
        IReadOnlyList<string> Reasons)
    {
        public static BatchWorkflowAnalysis Safe(string experimentType, string workflowVersionId, string workflowSnapshotJson)
        {
            return new BatchWorkflowAnalysis(true, false, true, experimentType, workflowVersionId, workflowSnapshotJson, []);
        }

        public static BatchWorkflowAnalysis Empty(bool hasCompleteWorkflowSelection)
        {
            return new BatchWorkflowAnalysis(false, true, hasCompleteWorkflowSelection, null, null, null, []);
        }

        public static BatchWorkflowAnalysis Manual(IReadOnlyList<string> reasons)
        {
            return new BatchWorkflowAnalysis(false, false, false, null, null, null, reasons);
        }
    }
}

public sealed record ChannelBatchWorkflowBackfillReport(
    int ScannedChannelBatchCount,
    int BackfilledCount,
    int NeedsManualResolutionCount,
    IReadOnlyDictionary<string, int> ReasonCounts);
