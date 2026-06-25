using Microsoft.EntityFrameworkCore;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;

namespace Stainer.Tests;

public sealed class DatabaseInfrastructureTests
{
    [Fact]
    public async Task Initializer_enables_sqlite_pragmas_and_creates_parent_directory()
    {
        var databasePath = CreateTempDatabasePath();
        var connectionString = $"Data Source={databasePath}";

        await DatabaseInitializer.InitializeAsync(connectionString);
        var checker = new DatabaseHealthChecker(connectionString);
        var report = await checker.CheckAsync();

        Assert.True(File.Exists(databasePath));
        Assert.True(report.ForeignKeysEnabled);
        Assert.Equal("wal", report.JournalMode, ignoreCase: true);
        Assert.True(report.BusyTimeoutMilliseconds >= DatabaseInitializer.MinimumBusyTimeoutMilliseconds);
        Assert.True(report.CanReadWrite);
    }

    [Fact]
    public async Task Health_checker_reports_database_path_and_sqlite_version()
    {
        var databasePath = CreateTempDatabasePath();
        var connectionString = $"Data Source={databasePath}";
        var checker = new DatabaseHealthChecker(connectionString);

        var report = await checker.CheckAsync();

        Assert.Equal(Path.GetFullPath(databasePath), report.DatabasePath);
        Assert.False(string.IsNullOrWhiteSpace(report.SqliteVersion));
        Assert.True(report.CanReadWrite);
    }

    [Fact]
    public async Task Channel_batch_workflow_assignment_migration_backfills_consistent_batches_and_flags_conflicts()
    {
        var databasePath = CreateTempDatabasePath();
        var connectionString = $"Data Source={databasePath}";

        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
        await using var dbContext = CreateContext(connectionString);
        await dbContext.Database.MigrateAsync("20260624073422_RuntimeLedgerAndMockExecutor");
        await dbContext.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO drawers (id, code, name, sort_order, heat_board_id, is_enabled, created_at_utc) VALUES
                ('drawer-a', 'A', 'Drawer A', 1, 1, 1, '2026-06-25T00:00:00+00:00'),
                ('drawer-b', 'B', 'Drawer B', 2, 2, 1, '2026-06-25T00:00:00+00:00');

            INSERT INTO physical_slots (id, drawer_id, code, slot_no, vertical_order_from_bottom, heat_point_id, is_enabled, created_at_utc) VALUES
                ('slot-a-01', 'drawer-a', 'A-01', 1, 1, 1, 1, '2026-06-25T00:00:00+00:00'),
                ('slot-a-02', 'drawer-a', 'A-02', 2, 2, 2, 1, '2026-06-25T00:00:00+00:00'),
                ('slot-b-01', 'drawer-b', 'B-01', 1, 1, 1, 1, '2026-06-25T00:00:00+00:00'),
                ('slot-b-02', 'drawer-b', 'B-02', 2, 2, 2, 1, '2026-06-25T00:00:00+00:00');

            INSERT INTO workflow_definitions (id, code, name, workflow_type, description, is_enabled, created_at_utc) VALUES
                ('wf-he', 'HE-MIG', 'HE Migration Workflow', 'HE', 'migration test', 1, '2026-06-25T00:00:00+00:00'),
                ('wf-ihc', 'IHC-MIG', 'IHC Migration Workflow', 'IHC', 'migration test', 1, '2026-06-25T00:00:00+00:00');

            INSERT INTO workflow_versions (id, workflow_definition_id, version_no, version_label, status, change_note, published_at_utc, created_at_utc) VALUES
                ('wf-he-v1', 'wf-he', 1, '1.0', 'Published', 'migration test', '2026-06-25T00:00:00+00:00', '2026-06-25T00:00:00+00:00'),
                ('wf-ihc-v1', 'wf-ihc', 1, '1.0', 'Published', 'migration test', '2026-06-25T00:00:00+00:00', '2026-06-25T00:00:00+00:00');

            INSERT INTO machine_runs (id, run_code, status, pause_requested, stop_requested, created_at_utc, started_at_utc, completed_at_utc) VALUES
                ('run-safe', 'RUN-SAFE', 'Completed', 0, 0, '2026-06-25T00:00:00+00:00', '2026-06-25T00:01:00+00:00', '2026-06-25T00:02:00+00:00'),
                ('run-conflict', 'RUN-CONFLICT', 'Completed', 0, 0, '2026-06-25T00:03:00+00:00', '2026-06-25T00:04:00+00:00', '2026-06-25T00:05:00+00:00');

            INSERT INTO channel_batches (id, machine_run_id, drawer_id, drawer_code, status, created_at_utc) VALUES
                ('batch-safe', 'run-safe', 'drawer-a', 'A', 'Completed', '2026-06-25T00:00:00+00:00'),
                ('batch-conflict', 'run-conflict', 'drawer-b', 'B', 'Completed', '2026-06-25T00:03:00+00:00');

            INSERT INTO staining_tasks (id, task_code, task_type, status, physical_slot_id, workflow_definition_id, workflow_version_id, workflow_snapshot_json, input_mode, candidate_results_json, created_at_utc) VALUES
                ('task-safe-1', 'TASK-SAFE-1', 'HE', 'Confirmed', 'slot-a-01', 'wf-he', 'wf-he-v1', '{{"workflowVersionId":"wf-he-v1"}}', 'migration', '[]', '2026-06-25T00:00:00+00:00'),
                ('task-safe-2', 'TASK-SAFE-2', 'HE', 'Confirmed', 'slot-a-02', 'wf-he', 'wf-he-v1', '{{"workflowVersionId":"wf-he-v1"}}', 'migration', '[]', '2026-06-25T00:00:00+00:00'),
                ('task-conflict-1', 'TASK-CONFLICT-1', 'HE', 'Confirmed', 'slot-b-01', 'wf-he', 'wf-he-v1', '{{"workflowVersionId":"wf-he-v1"}}', 'migration', '[]', '2026-06-25T00:03:00+00:00'),
                ('task-conflict-2', 'TASK-CONFLICT-2', 'IHC', 'Confirmed', 'slot-b-02', 'wf-ihc', 'wf-ihc-v1', '{{"workflowVersionId":"wf-ihc-v1"}}', 'migration', '[]', '2026-06-25T00:03:00+00:00');

            INSERT INTO slide_tasks (id, channel_batch_id, staining_task_id, physical_slot_id, slot_code, task_type, status, created_at_utc) VALUES
                ('slide-safe-1', 'batch-safe', 'task-safe-1', 'slot-a-01', 'A-01', 'HE', 'WaitingUnload', '2026-06-25T00:00:00+00:00'),
                ('slide-safe-2', 'batch-safe', 'task-safe-2', 'slot-a-02', 'A-02', 'HE', 'WaitingUnload', '2026-06-25T00:00:00+00:00'),
                ('slide-conflict-1', 'batch-conflict', 'task-conflict-1', 'slot-b-01', 'B-01', 'HE', 'WaitingUnload', '2026-06-25T00:03:00+00:00'),
                ('slide-conflict-2', 'batch-conflict', 'task-conflict-2', 'slot-b-02', 'B-02', 'IHC', 'WaitingUnload', '2026-06-25T00:03:00+00:00');
            """);

        await dbContext.Database.MigrateAsync();

        var safe = await dbContext.ChannelBatches.SingleAsync(x => x.Id == "batch-safe");
        Assert.Equal(StainingTaskType.He, safe.ExperimentType);
        Assert.Equal("wf-he-v1", safe.SelectedWorkflowVersionId);
        Assert.Equal(WorkflowSelectionStatus.Locked, safe.WorkflowSelectionStatus);
        Assert.Equal("{\"workflowVersionId\":\"wf-he-v1\"}", safe.WorkflowSnapshotJson);
        Assert.NotNull(safe.WorkflowLockedAtUtc);

        var conflict = await dbContext.ChannelBatches.SingleAsync(x => x.Id == "batch-conflict");
        Assert.Equal(WorkflowSelectionStatus.NeedsManualResolution, conflict.WorkflowSelectionStatus);
        Assert.Null(conflict.SelectedWorkflowVersionId);

        Assert.True(await dbContext.WorkflowAssignmentHistory.AnyAsync(x => x.ChannelBatchId == "batch-safe" && x.ActionType == WorkflowAssignmentAction.Backfill));
        Assert.True(await dbContext.WorkflowAssignmentHistory.AnyAsync(x => x.ChannelBatchId == "batch-conflict" && x.ActionType == WorkflowAssignmentAction.ManualResolutionRequired));
        Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "migration.channel_batch_workflow_backfill" && x.Message.Contains("\"needsManualResolution\":1")));
    }

    private static string CreateTempDatabasePath()
    {
        return Path.Combine(Path.GetTempPath(), "stainer-tests", Guid.NewGuid().ToString("N"), "stainer.db");
    }

    private static StainerDbContext CreateContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite(connectionString)
            .Options;
        return new StainerDbContext(options);
    }
}
