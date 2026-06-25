using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChannelBatchWorkflowAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channel_batches_machine_runs_machine_run_id",
                table: "channel_batches");

            migrationBuilder.DropIndex(
                name: "IX_slide_tasks_physical_slot_id",
                table: "slide_tasks");

            migrationBuilder.DropIndex(
                name: "IX_channel_batches_drawer_id",
                table: "channel_batches");

            migrationBuilder.AlterColumn<string>(
                name: "machine_run_id",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "completed_at_utc",
                table: "channel_batches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "experiment_type",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "selected_workflow_version_id",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "started_at_utc",
                table: "channel_batches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "workflow_locked_at_utc",
                table: "channel_batches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "workflow_selected_at_utc",
                table: "channel_batches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workflow_selected_by_user_id",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workflow_selection_status",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unselected");

            migrationBuilder.AddColumn<string>(
                name: "workflow_snapshot_json",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.CreateTable(
                name: "workflow_assignment_history",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    channel_batch_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    old_experiment_type = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    old_workflow_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    old_workflow_snapshot_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: true),
                    new_experiment_type = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    new_workflow_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    new_workflow_snapshot_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: true),
                    action_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    actor_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    reason = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_assignment_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_assignment_history_channel_batches_channel_batch_id",
                        column: x => x.channel_batch_id,
                        principalTable: "channel_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workflow_assignment_history_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.Sql(
                """
                UPDATE channel_batches
                SET
                    workflow_selection_status = 'Unselected',
                    workflow_snapshot_json = CASE
                        WHEN workflow_snapshot_json IS NULL OR trim(workflow_snapshot_json) = '' THEN '{}'
                        ELSE workflow_snapshot_json
                    END;
                """);

            migrationBuilder.Sql(
                """
                WITH batch_rollup AS (
                    SELECT
                        cb.id,
                        COUNT(st.id) AS slide_count,
                        COUNT(DISTINCT st.task_type) AS type_count,
                        MIN(st.task_type) AS experiment_type,
                        COUNT(DISTINCT st.workflow_version_id) AS workflow_count,
                        MIN(st.workflow_version_id) AS workflow_version_id,
                        COUNT(DISTINCT COALESCE(NULLIF(trim(st.workflow_snapshot_json), ''), '{}')) AS snapshot_count,
                        MIN(st.workflow_snapshot_json) AS snapshot_json
                    FROM channel_batches cb
                    LEFT JOIN slide_tasks sl ON sl.channel_batch_id = cb.id
                    LEFT JOIN staining_tasks st ON st.id = sl.staining_task_id
                    GROUP BY cb.id
                ),
                safe_batches AS (
                    SELECT *
                    FROM batch_rollup
                    WHERE slide_count > 0
                        AND type_count = 1
                        AND workflow_count = 1
                        AND snapshot_count = 1
                        AND workflow_version_id IS NOT NULL
                        AND snapshot_json IS NOT NULL
                        AND trim(snapshot_json) <> ''
                        AND trim(snapshot_json) <> '{}'
                )
                UPDATE channel_batches
                SET
                    experiment_type = (SELECT experiment_type FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    selected_workflow_version_id = (SELECT workflow_version_id FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    workflow_snapshot_json = (SELECT snapshot_json FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    workflow_selection_status = CASE WHEN machine_run_id IS NULL THEN 'Selected' ELSE 'Locked' END,
                    workflow_selected_at_utc = created_at_utc,
                    workflow_locked_at_utc = CASE
                        WHEN machine_run_id IS NULL THEN NULL
                        ELSE COALESCE((SELECT started_at_utc FROM machine_runs WHERE machine_runs.id = channel_batches.machine_run_id), created_at_utc)
                    END,
                    started_at_utc = COALESCE(started_at_utc, (SELECT started_at_utc FROM machine_runs WHERE machine_runs.id = channel_batches.machine_run_id)),
                    completed_at_utc = COALESCE(completed_at_utc, (SELECT completed_at_utc FROM machine_runs WHERE machine_runs.id = channel_batches.machine_run_id))
                WHERE id IN (SELECT id FROM safe_batches);
                """);

            migrationBuilder.Sql(
                """
                WITH batch_rollup AS (
                    SELECT
                        cb.id,
                        COUNT(st.id) AS slide_count,
                        COUNT(DISTINCT st.task_type) AS type_count,
                        COUNT(DISTINCT st.workflow_version_id) AS workflow_count,
                        COUNT(DISTINCT COALESCE(NULLIF(trim(st.workflow_snapshot_json), ''), '{}')) AS snapshot_count,
                        MIN(st.workflow_snapshot_json) AS snapshot_json
                    FROM channel_batches cb
                    LEFT JOIN slide_tasks sl ON sl.channel_batch_id = cb.id
                    LEFT JOIN staining_tasks st ON st.id = sl.staining_task_id
                    GROUP BY cb.id
                ),
                unsafe_batches AS (
                    SELECT *
                    FROM batch_rollup
                    WHERE slide_count > 0
                        AND NOT (
                            type_count = 1
                            AND workflow_count = 1
                            AND snapshot_count = 1
                            AND snapshot_json IS NOT NULL
                            AND trim(snapshot_json) <> ''
                            AND trim(snapshot_json) <> '{}'
                        )
                )
                UPDATE channel_batches
                SET
                    workflow_selection_status = 'NeedsManualResolution',
                    workflow_snapshot_json = CASE
                        WHEN workflow_snapshot_json IS NULL OR trim(workflow_snapshot_json) = '' THEN '{}'
                        ELSE workflow_snapshot_json
                    END
                WHERE id IN (SELECT id FROM unsafe_batches);
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO workflow_assignment_history (
                    id,
                    channel_batch_id,
                    old_experiment_type,
                    old_workflow_version_id,
                    old_workflow_snapshot_json,
                    new_experiment_type,
                    new_workflow_version_id,
                    new_workflow_snapshot_json,
                    action_type,
                    actor_user_id,
                    created_at_utc,
                    reason,
                    command_id
                )
                SELECT
                    lower(hex(randomblob(16))),
                    id,
                    NULL,
                    NULL,
                    NULL,
                    experiment_type,
                    selected_workflow_version_id,
                    workflow_snapshot_json,
                    'Backfill',
                    NULL,
                    COALESCE(workflow_selected_at_utc, created_at_utc),
                    'Migration safely backfilled channel workflow from consistent slide tasks.',
                    'migration-20260625084017'
                FROM channel_batches
                WHERE workflow_selection_status IN ('Selected', 'Locked');
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO workflow_assignment_history (
                    id,
                    channel_batch_id,
                    old_experiment_type,
                    old_workflow_version_id,
                    old_workflow_snapshot_json,
                    new_experiment_type,
                    new_workflow_version_id,
                    new_workflow_snapshot_json,
                    action_type,
                    actor_user_id,
                    created_at_utc,
                    reason,
                    command_id
                )
                SELECT
                    lower(hex(randomblob(16))),
                    id,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    workflow_snapshot_json,
                    'ManualResolutionRequired',
                    NULL,
                    created_at_utc,
                    'Migration detected mixed experiment type, mixed workflow version, missing workflow, or missing/mismatched workflow snapshot.',
                    'migration-20260625084017'
                FROM channel_batches
                WHERE workflow_selection_status = 'NeedsManualResolution';
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO audit_logs (
                    id,
                    actor_user_id,
                    action,
                    entity_type,
                    entity_id,
                    message,
                    created_at_utc
                )
                SELECT
                    lower(hex(randomblob(16))),
                    NULL,
                    'migration.channel_batch_workflow_backfill',
                    'Migration',
                    '20260625084017_ChannelBatchWorkflowAssignment',
                    '{"safeBackfilled":' || (SELECT COUNT(*) FROM channel_batches WHERE workflow_selection_status IN ('Selected', 'Locked')) ||
                        ',"needsManualResolution":' || (SELECT COUNT(*) FROM channel_batches WHERE workflow_selection_status = 'NeedsManualResolution') ||
                        ',"emptyBatches":' || (SELECT COUNT(*) FROM channel_batches WHERE workflow_selection_status = 'Unselected') || '}',
                    strftime('%Y-%m-%dT%H:%M:%f+00:00', 'now');
                """);

            migrationBuilder.CreateIndex(
                name: "IX_slide_tasks_physical_slot_id",
                table: "slide_tasks",
                column: "physical_slot_id",
                unique: true,
                filter: "status in ('Pending', 'Running', 'Paused', 'Faulted', 'WaitingUnload')");

            migrationBuilder.CreateIndex(
                name: "IX_channel_batches_drawer_id",
                table: "channel_batches",
                column: "drawer_id",
                unique: true,
                filter: "status in ('Pending', 'Running', 'Paused', 'Faulted')");

            migrationBuilder.CreateIndex(
                name: "IX_channel_batches_selected_workflow_version_id",
                table: "channel_batches",
                column: "selected_workflow_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_batches_workflow_selected_by_user_id",
                table: "channel_batches",
                column: "workflow_selected_by_user_id");

            migrationBuilder.AddCheckConstraint(
                name: "ck_channel_batches_workflow_selection_status",
                table: "channel_batches",
                sql: "workflow_selection_status in ('Unselected', 'Selected', 'Locked', 'NeedsManualResolution')");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_assignment_history_actor_user_id",
                table: "workflow_assignment_history",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_assignment_history_channel_batch_id_created_at_utc",
                table: "workflow_assignment_history",
                columns: new[] { "channel_batch_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_assignment_history_command_id",
                table: "workflow_assignment_history",
                column: "command_id");

            migrationBuilder.AddForeignKey(
                name: "FK_channel_batches_machine_runs_machine_run_id",
                table: "channel_batches",
                column: "machine_run_id",
                principalTable: "machine_runs",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_channel_batches_users_workflow_selected_by_user_id",
                table: "channel_batches",
                column: "workflow_selected_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_channel_batches_workflow_versions_selected_workflow_version_id",
                table: "channel_batches",
                column: "selected_workflow_version_id",
                principalTable: "workflow_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channel_batches_machine_runs_machine_run_id",
                table: "channel_batches");

            migrationBuilder.DropForeignKey(
                name: "FK_channel_batches_users_workflow_selected_by_user_id",
                table: "channel_batches");

            migrationBuilder.DropForeignKey(
                name: "FK_channel_batches_workflow_versions_selected_workflow_version_id",
                table: "channel_batches");

            migrationBuilder.DropTable(
                name: "workflow_assignment_history");

            migrationBuilder.DropIndex(
                name: "IX_slide_tasks_physical_slot_id",
                table: "slide_tasks");

            migrationBuilder.DropIndex(
                name: "IX_channel_batches_drawer_id",
                table: "channel_batches");

            migrationBuilder.DropIndex(
                name: "IX_channel_batches_selected_workflow_version_id",
                table: "channel_batches");

            migrationBuilder.DropIndex(
                name: "IX_channel_batches_workflow_selected_by_user_id",
                table: "channel_batches");

            migrationBuilder.DropCheckConstraint(
                name: "ck_channel_batches_workflow_selection_status",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "completed_at_utc",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "experiment_type",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "selected_workflow_version_id",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "started_at_utc",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "workflow_locked_at_utc",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "workflow_selected_at_utc",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "workflow_selected_by_user_id",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "workflow_selection_status",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "workflow_snapshot_json",
                table: "channel_batches");

            migrationBuilder.AlterColumn<string>(
                name: "machine_run_id",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_slide_tasks_physical_slot_id",
                table: "slide_tasks",
                column: "physical_slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_batches_drawer_id",
                table: "channel_batches",
                column: "drawer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_channel_batches_machine_runs_machine_run_id",
                table: "channel_batches",
                column: "machine_run_id",
                principalTable: "machine_runs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
