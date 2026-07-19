using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class ChannelBatchWorkflowBackfillServiceTests
{
    [Fact]
    public async Task Single_he_script_channel_can_be_backfilled()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var heVersion = await CreateWorkflowAsync(dbContext, "HE-BACKFILL", StainingTaskType.He);
        var snapshot = Snapshot(heVersion.Id);
        var batch = await CreateBatchAsync(dbContext, "A", [(StainingTaskType.He, heVersion, snapshot), (StainingTaskType.He, heVersion, snapshot)]);

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(1, report.ScannedChannelBatchCount);
        Assert.Equal(1, report.BackfilledCount);
        Assert.Equal(0, report.NeedsManualResolutionCount);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.False(batch.NeedsManualResolution);
        Assert.Equal(StainingTaskType.He, batch.ExperimentType);
        Assert.Equal(heVersion.Id, batch.SelectedWorkflowVersionId);
        Assert.Equal(snapshot, batch.WorkflowSnapshotJson);
        Assert.Equal(WorkflowSelectionStatus.Selected, batch.WorkflowSelectionStatus);
    }

    [Fact]
    public async Task Single_ihc_script_channel_can_be_backfilled_from_unique_non_empty_snapshot()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var ihcVersion = await CreateWorkflowAsync(dbContext, "IHC-BACKFILL", StainingTaskType.Ihc);
        var snapshot = Snapshot(ihcVersion.Id);
        var batch = await CreateBatchAsync(dbContext, "B", [(StainingTaskType.Ihc, ihcVersion, snapshot), (StainingTaskType.Ihc, ihcVersion, "{}")]);

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(1, report.BackfilledCount);
        Assert.Equal(0, report.NeedsManualResolutionCount);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.False(batch.NeedsManualResolution);
        Assert.Equal(StainingTaskType.Ihc, batch.ExperimentType);
        Assert.Equal(ihcVersion.Id, batch.SelectedWorkflowVersionId);
        Assert.Equal(snapshot, batch.WorkflowSnapshotJson);
    }

    [Fact]
    public async Task Mixed_he_ihc_channel_is_marked_needs_manual_resolution()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var heVersion = await CreateWorkflowAsync(dbContext, "HE-MIXED-BACKFILL", StainingTaskType.He);
        var snapshot = Snapshot(heVersion.Id);
        var batch = await CreateBatchAsync(dbContext, "C", [(StainingTaskType.He, heVersion, snapshot), (StainingTaskType.Ihc, heVersion, snapshot)]);

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(0, report.BackfilledCount);
        Assert.Equal(1, report.NeedsManualResolutionCount);
        Assert.Equal(1, report.ReasonCounts["MixedExperimentType"]);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.True(batch.NeedsManualResolution);
        Assert.Equal(WorkflowSelectionStatus.NeedsManualResolution, batch.WorkflowSelectionStatus);
        Assert.Contains("MixedExperimentType", batch.ManualResolutionReason);
        Assert.Null(batch.SelectedWorkflowVersionId);
    }

    [Fact]
    public async Task Multiple_workflow_versions_are_marked_needs_manual_resolution()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var heVersionOne = await CreateWorkflowAsync(dbContext, "HE-WF1-BACKFILL", StainingTaskType.He);
        var heVersionTwo = await CreateWorkflowAsync(dbContext, "HE-WF2-BACKFILL", StainingTaskType.He);
        var snapshot = Snapshot("shared-shape");
        var batch = await CreateBatchAsync(dbContext, "D", [(StainingTaskType.He, heVersionOne, snapshot), (StainingTaskType.He, heVersionTwo, snapshot)]);

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(0, report.BackfilledCount);
        Assert.Equal(1, report.NeedsManualResolutionCount);
        Assert.Equal(1, report.ReasonCounts["MultipleWorkflowVersions"]);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.True(batch.NeedsManualResolution);
        Assert.Contains("MultipleWorkflowVersions", batch.ManualResolutionReason);
        Assert.Null(batch.SelectedWorkflowVersionId);
    }

    [Fact]
    public async Task Empty_unselected_channel_is_not_marked_needs_manual_resolution()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var batch = await CreateBatchAsync(dbContext, "A", []);
        batch.NeedsManualResolution = true;
        batch.ManualResolutionReason = "NoSlideTasks; CannotDetermineExperimentType";
        batch.WorkflowSelectionStatus = WorkflowSelectionStatus.NeedsManualResolution;
        await dbContext.SaveChangesAsync();

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(1, report.ScannedChannelBatchCount);
        Assert.Equal(0, report.BackfilledCount);
        Assert.Equal(0, report.NeedsManualResolutionCount);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.False(batch.NeedsManualResolution);
        Assert.Equal(string.Empty, batch.ManualResolutionReason);
        Assert.Equal(WorkflowSelectionStatus.Unselected, batch.WorkflowSelectionStatus);
        Assert.Null(batch.ExperimentType);
        Assert.Null(batch.SelectedWorkflowVersionId);
        Assert.Equal("{}", batch.WorkflowSnapshotJson);
    }

    [Fact]
    public async Task Empty_selected_channel_keeps_channel_workflow_selection()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var heVersion = await CreateWorkflowAsync(dbContext, "HE-EMPTY-SELECTED-BACKFILL", StainingTaskType.He);
        var snapshot = Snapshot(heVersion.Id);
        var batch = await CreateBatchAsync(dbContext, "B", []);
        batch.ExperimentType = StainingTaskType.He;
        batch.SelectedWorkflowVersionId = heVersion.Id;
        batch.WorkflowSnapshotJson = snapshot;
        batch.WorkflowSelectionStatus = WorkflowSelectionStatus.NeedsManualResolution;
        batch.NeedsManualResolution = true;
        batch.ManualResolutionReason = "NoSlideTasks";
        await dbContext.SaveChangesAsync();

        var report = await CreateService(dbContext).BackfillAsync();

        Assert.Equal(1, report.ScannedChannelBatchCount);
        Assert.Equal(0, report.BackfilledCount);
        Assert.Equal(0, report.NeedsManualResolutionCount);
        await dbContext.Entry(batch).ReloadAsync();
        Assert.False(batch.NeedsManualResolution);
        Assert.Equal(string.Empty, batch.ManualResolutionReason);
        Assert.Equal(WorkflowSelectionStatus.Selected, batch.WorkflowSelectionStatus);
        Assert.Equal(StainingTaskType.He, batch.ExperimentType);
        Assert.Equal(heVersion.Id, batch.SelectedWorkflowVersionId);
        Assert.Equal(snapshot, batch.WorkflowSnapshotJson);
    }

    [Fact]
    public async Task Backfill_is_idempotent_for_correctly_backfilled_batches()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var heVersion = await CreateWorkflowAsync(dbContext, "HE-IDEMPOTENT-BACKFILL", StainingTaskType.He);
        var snapshot = Snapshot(heVersion.Id);
        var batch = await CreateBatchAsync(dbContext, "A", [(StainingTaskType.He, heVersion, snapshot)]);
        var service = CreateService(dbContext);

        var first = await service.BackfillAsync();
        var afterFirst = await dbContext.ChannelBatches.AsNoTracking().SingleAsync(x => x.Id == batch.Id);
        var second = await service.BackfillAsync();
        var afterSecond = await dbContext.ChannelBatches.AsNoTracking().SingleAsync(x => x.Id == batch.Id);

        Assert.Equal(1, first.BackfilledCount);
        Assert.Equal(0, second.BackfilledCount);
        Assert.Equal(afterFirst.ExperimentType, afterSecond.ExperimentType);
        Assert.Equal(afterFirst.SelectedWorkflowVersionId, afterSecond.SelectedWorkflowVersionId);
        Assert.Equal(afterFirst.WorkflowSnapshotJson, afterSecond.WorkflowSnapshotJson);
        Assert.Equal(afterFirst.WorkflowSelectionStatus, afterSecond.WorkflowSelectionStatus);
        Assert.Equal(afterFirst.NeedsManualResolution, afterSecond.NeedsManualResolution);
        Assert.Equal(afterFirst.ManualResolutionReason, afterSecond.ManualResolutionReason);
    }

    private static ChannelBatchWorkflowBackfillService CreateService(StainerDbContext dbContext)
    {
        return new ChannelBatchWorkflowBackfillService(dbContext, NullLogger<ChannelBatchWorkflowBackfillService>.Instance);
    }

    private static async Task<StainerDbContext> CreateMigratedContextAsync()
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-channel-backfill-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var connectionString = $"Data Source={databasePath}";
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite(connectionString)
            .Options;
        var dbContext = new StainerDbContext(options);
        await dbContext.Database.MigrateAsync();
        await new ReferenceDataSeeder(dbContext).SeedAsync();
        return dbContext;
    }

    private static async Task<WorkflowVersion> CreateWorkflowAsync(StainerDbContext dbContext, string code, string workflowType)
    {
        var definition = new WorkflowDefinition
        {
            Code = code,
            Name = $"{code} workflow",
            WorkflowType = workflowType,
            Description = "Backfill test workflow.",
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        var version = new WorkflowVersion
        {
            WorkflowDefinition = definition,
            VersionNo = 1,
            VersionLabel = "1.0",
            Status = WorkflowVersionStatus.Published,
            ChangeNote = "Backfill test.",
            PublishedAtUtc = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        dbContext.WorkflowDefinitions.Add(definition);
        dbContext.WorkflowVersions.Add(version);
        await dbContext.SaveChangesAsync();
        return version;
    }

    private static async Task<ChannelBatch> CreateBatchAsync(
        StainerDbContext dbContext,
        string drawerCode,
        IReadOnlyList<(string TaskType, WorkflowVersion Version, string Snapshot)> slides)
    {
        var drawer = await dbContext.Drawers.SingleAsync(x => x.Code == drawerCode);
        var slots = await dbContext.PhysicalSlots
            .Where(x => x.DrawerId == drawer.Id)
            .OrderBy(x => x.SlotNo)
            .Take(slides.Count)
            .ToListAsync();
        var batch = new ChannelBatch
        {
            DrawerId = drawer.Id,
            DrawerCode = drawer.Code,
            Status = RuntimeLedgerStatus.Pending,
            WorkflowSnapshotJson = "{}",
            WorkflowSelectionStatus = WorkflowSelectionStatus.Unselected,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        dbContext.ChannelBatches.Add(batch);

        for (var index = 0; index < slides.Count; index++)
        {
            var slide = slides[index];
            var slot = slots[index];
            var task = new StainingTask
            {
                TaskCode = $"TASK-{drawerCode}-{index}-{Guid.NewGuid():N}"[..28],
                TaskType = slide.TaskType,
                Status = StainingTaskStatus.Confirmed,
                PhysicalSlotId = slot.Id,
                WorkflowDefinitionId = slide.Version.WorkflowDefinitionId,
                WorkflowVersionId = slide.Version.Id,
                WorkflowSnapshotJson = slide.Snapshot,
                CandidateResultsJson = "[]",
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.StainingTasks.Add(task);
            dbContext.SlideTasks.Add(new SlideTask
            {
                ChannelBatch = batch,
                StainingTask = task,
                PhysicalSlotId = slot.Id,
                SlotCode = slot.Code,
                TaskType = slide.TaskType,
                Status = RuntimeLedgerStatus.Pending,
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
        }

        await dbContext.SaveChangesAsync();
        return batch;
    }

    private static string Snapshot(string workflowVersionId)
    {
        return $$"""{"workflowVersionId":"{{workflowVersionId}}"}""";
    }
}
