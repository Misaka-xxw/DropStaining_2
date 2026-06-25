using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChannelBatchManualResolutionFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manual_resolution_reason",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "needs_manual_resolution",
                table: "channel_batches",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(
                """
                WITH analysis AS (
                    SELECT
                        cb.id,
                        COUNT(sl.id) AS slide_count,
                        SUM(CASE WHEN sl.id IS NOT NULL AND st.id IS NULL THEN 1 ELSE 0 END) AS missing_task_count,
                        COUNT(DISTINCT NULLIF(trim(st.task_type), '')) AS type_count,
                        MIN(NULLIF(trim(st.task_type), '')) AS experiment_type,
                        SUM(CASE WHEN st.id IS NOT NULL AND (st.workflow_version_id IS NULL OR trim(st.workflow_version_id) = '') THEN 1 ELSE 0 END) AS missing_workflow_count,
                        COUNT(DISTINCT NULLIF(trim(st.workflow_version_id), '')) AS workflow_count,
                        MIN(NULLIF(trim(st.workflow_version_id), '')) AS workflow_version_id,
                        COUNT(DISTINCT CASE
                            WHEN st.workflow_snapshot_json IS NOT NULL
                                AND trim(st.workflow_snapshot_json) <> ''
                                AND trim(st.workflow_snapshot_json) <> '{}'
                            THEN trim(st.workflow_snapshot_json)
                        END) AS snapshot_count,
                        MIN(CASE
                            WHEN st.workflow_snapshot_json IS NOT NULL
                                AND trim(st.workflow_snapshot_json) <> ''
                                AND trim(st.workflow_snapshot_json) <> '{}'
                            THEN trim(st.workflow_snapshot_json)
                        END) AS snapshot_json
                    FROM channel_batches cb
                    LEFT JOIN slide_tasks sl ON sl.channel_batch_id = cb.id
                    LEFT JOIN staining_tasks st ON st.id = sl.staining_task_id
                    GROUP BY cb.id
                ),
                safe_batches AS (
                    SELECT *
                    FROM analysis
                    WHERE slide_count > 0
                        AND missing_task_count = 0
                        AND type_count = 1
                        AND missing_workflow_count = 0
                        AND workflow_count = 1
                        AND snapshot_count = 1
                )
                UPDATE channel_batches
                SET
                    experiment_type = (SELECT experiment_type FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    selected_workflow_version_id = (SELECT workflow_version_id FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    workflow_snapshot_json = (SELECT snapshot_json FROM safe_batches WHERE safe_batches.id = channel_batches.id),
                    workflow_selection_status = CASE WHEN machine_run_id IS NULL THEN 'Selected' ELSE 'Locked' END,
                    needs_manual_resolution = 0,
                    manual_resolution_reason = ''
                WHERE id IN (SELECT id FROM safe_batches);
                """);

            migrationBuilder.Sql(
                """
                WITH analysis AS (
                    SELECT
                        cb.id,
                        COUNT(sl.id) AS slide_count,
                        SUM(CASE WHEN sl.id IS NOT NULL AND st.id IS NULL THEN 1 ELSE 0 END) AS missing_task_count,
                        COUNT(DISTINCT NULLIF(trim(st.task_type), '')) AS type_count,
                        SUM(CASE WHEN st.id IS NOT NULL AND (st.workflow_version_id IS NULL OR trim(st.workflow_version_id) = '') THEN 1 ELSE 0 END) AS missing_workflow_count,
                        COUNT(DISTINCT NULLIF(trim(st.workflow_version_id), '')) AS workflow_count,
                        COUNT(DISTINCT CASE
                            WHEN st.workflow_snapshot_json IS NOT NULL
                                AND trim(st.workflow_snapshot_json) <> ''
                                AND trim(st.workflow_snapshot_json) <> '{}'
                            THEN trim(st.workflow_snapshot_json)
                        END) AS snapshot_count
                    FROM channel_batches cb
                    LEFT JOIN slide_tasks sl ON sl.channel_batch_id = cb.id
                    LEFT JOIN staining_tasks st ON st.id = sl.staining_task_id
                    GROUP BY cb.id
                ),
                manual_batches AS (
                    SELECT
                        id,
                        trim(
                            CASE WHEN slide_count = 0 THEN 'NoSlideTasks; ' ELSE '' END ||
                            CASE WHEN missing_task_count > 0 THEN 'MissingStainingTask; ' ELSE '' END ||
                            CASE WHEN type_count = 0 THEN 'CannotDetermineExperimentType; ' ELSE '' END ||
                            CASE WHEN type_count > 1 THEN 'MixedExperimentType; ' ELSE '' END ||
                            CASE WHEN missing_workflow_count > 0 THEN 'MissingWorkflowVersion; ' ELSE '' END ||
                            CASE WHEN workflow_count > 1 THEN 'MultipleWorkflowVersions; ' ELSE '' END ||
                            CASE WHEN snapshot_count = 0 THEN 'MissingWorkflowSnapshot; ' ELSE '' END ||
                            CASE WHEN snapshot_count > 1 THEN 'WorkflowSnapshotConflict; ' ELSE '' END
                        ) AS reason
                    FROM analysis
                    WHERE NOT (
                        slide_count > 0
                        AND missing_task_count = 0
                        AND type_count = 1
                        AND missing_workflow_count = 0
                        AND workflow_count = 1
                        AND snapshot_count = 1
                    )
                )
                UPDATE channel_batches
                SET
                    needs_manual_resolution = 1,
                    manual_resolution_reason = (SELECT reason FROM manual_batches WHERE manual_batches.id = channel_batches.id),
                    workflow_selection_status = 'NeedsManualResolution'
                WHERE id IN (SELECT id FROM manual_batches);
                """);

            migrationBuilder.Sql(
                """
                WITH analysis AS (
                    SELECT
                        cb.id,
                        COUNT(sl.id) AS slide_count,
                        SUM(CASE WHEN sl.id IS NOT NULL AND st.id IS NULL THEN 1 ELSE 0 END) AS missing_task_count,
                        COUNT(DISTINCT NULLIF(trim(st.task_type), '')) AS type_count,
                        SUM(CASE WHEN st.id IS NOT NULL AND (st.workflow_version_id IS NULL OR trim(st.workflow_version_id) = '') THEN 1 ELSE 0 END) AS missing_workflow_count,
                        COUNT(DISTINCT NULLIF(trim(st.workflow_version_id), '')) AS workflow_count,
                        COUNT(DISTINCT CASE
                            WHEN st.workflow_snapshot_json IS NOT NULL
                                AND trim(st.workflow_snapshot_json) <> ''
                                AND trim(st.workflow_snapshot_json) <> '{}'
                            THEN trim(st.workflow_snapshot_json)
                        END) AS snapshot_count
                    FROM channel_batches cb
                    LEFT JOIN slide_tasks sl ON sl.channel_batch_id = cb.id
                    LEFT JOIN staining_tasks st ON st.id = sl.staining_task_id
                    GROUP BY cb.id
                )
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
                    'migration.channel_batch_manual_resolution_backfill',
                    'Migration',
                    '20260625085434_ChannelBatchManualResolutionFlags',
                    '{"scannedChannelBatchCount":' || (SELECT COUNT(*) FROM analysis) ||
                        ',"backfilledCount":' || (SELECT COUNT(*) FROM analysis WHERE slide_count > 0 AND missing_task_count = 0 AND type_count = 1 AND missing_workflow_count = 0 AND workflow_count = 1 AND snapshot_count = 1) ||
                        ',"needsManualResolutionCount":' || (SELECT COUNT(*) FROM analysis WHERE NOT (slide_count > 0 AND missing_task_count = 0 AND type_count = 1 AND missing_workflow_count = 0 AND workflow_count = 1 AND snapshot_count = 1)) ||
                        ',"reasonCounts":{"NoSlideTasks":' || (SELECT COUNT(*) FROM analysis WHERE slide_count = 0) ||
                        ',"MissingStainingTask":' || (SELECT COUNT(*) FROM analysis WHERE missing_task_count > 0) ||
                        ',"CannotDetermineExperimentType":' || (SELECT COUNT(*) FROM analysis WHERE type_count = 0) ||
                        ',"MixedExperimentType":' || (SELECT COUNT(*) FROM analysis WHERE type_count > 1) ||
                        ',"MissingWorkflowVersion":' || (SELECT COUNT(*) FROM analysis WHERE missing_workflow_count > 0) ||
                        ',"MultipleWorkflowVersions":' || (SELECT COUNT(*) FROM analysis WHERE workflow_count > 1) ||
                        ',"MissingWorkflowSnapshot":' || (SELECT COUNT(*) FROM analysis WHERE snapshot_count = 0) ||
                        ',"WorkflowSnapshotConflict":' || (SELECT COUNT(*) FROM analysis WHERE snapshot_count > 1) || '}}',
                    strftime('%Y-%m-%dT%H:%M:%f+00:00', 'now');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "manual_resolution_reason",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "needs_manual_resolution",
                table: "channel_batches");
        }
    }
}
