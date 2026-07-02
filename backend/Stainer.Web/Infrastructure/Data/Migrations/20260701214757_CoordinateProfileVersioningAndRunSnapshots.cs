using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoordinateProfileVersioningAndRunSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_coordinate_points_coordinate_profile_id_point_code",
                table: "coordinate_points");

            migrationBuilder.RenameColumn(
                name: "aspirate_z_um",
                table: "coordinate_points",
                newName: "liquid_detect_z_um");

            migrationBuilder.RenameColumn(
                name: "aspirate_z_um",
                table: "coordinate_calibration_history",
                newName: "liquid_detect_z_um");

            migrationBuilder.AddColumn<string>(
                name: "coordinate_profile_version_id",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coordinate_snapshot_json",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "active_version_id",
                table: "coordinate_profiles",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_x_um",
                table: "coordinate_points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_y_um",
                table: "coordinate_points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_z_um",
                table: "coordinate_points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "calibrated_z_um",
                table: "coordinate_points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coordinate_profile_version_id",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "validation_message",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "validation_status",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "Unverified");

            migrationBuilder.AddColumn<long>(
                name: "new_z_um",
                table: "coordinate_calibration_history",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_x_um",
                table: "coordinate_calibration_history",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_y_um",
                table: "coordinate_calibration_history",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "action_offset_z_um",
                table: "coordinate_calibration_history",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "change_summary_json",
                table: "coordinate_calibration_history",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "validation_result_json",
                table: "coordinate_calibration_history",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "coordinate_profile_version_id",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coordinate_selection_status",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unselected");

            migrationBuilder.AddColumn<string>(
                name: "coordinate_snapshot_json",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.CreateTable(
                name: "coordinate_profile_versions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    coordinate_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    version_no = table.Column<int>(type: "INTEGER", nullable: false),
                    version_label = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    source_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    change_reason = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    change_summary_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    validation_result_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    created_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    published_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    activated_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    published_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    activated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    retired_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinate_profile_versions", x => x.id);
                    table.ForeignKey(
                        name: "FK_coordinate_profile_versions_coordinate_profile_versions_source_version_id",
                        column: x => x.source_version_id,
                        principalTable: "coordinate_profile_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_coordinate_profile_versions_coordinate_profiles_coordinate_profile_id",
                        column: x => x.coordinate_profile_id,
                        principalTable: "coordinate_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coordinate_profile_versions_users_activated_by_user_id",
                        column: x => x.activated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_coordinate_profile_versions_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_coordinate_profile_versions_users_published_by_user_id",
                        column: x => x.published_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.Sql("""
                INSERT INTO coordinate_profile_versions (
                    id,
                    coordinate_profile_id,
                    version_no,
                    version_label,
                    status,
                    is_active,
                    source_version_id,
                    change_reason,
                    change_summary_json,
                    validation_result_json,
                    created_by_user_id,
                    published_by_user_id,
                    activated_by_user_id,
                    created_at_utc,
                    published_at_utc,
                    activated_at_utc,
                    retired_at_utc)
                SELECT
                    p.id,
                    p.id,
                    1,
                    '1',
                    CASE WHEN p.is_active = 1 THEN 'Active' ELSE 'NeedsManualResolution' END,
                    p.is_active,
                    NULL,
                    'Migrated from pre-version coordinate profile. Existing point values are preserved; missing source validation remains explicit in target point validation fields.',
                    '{"migration":"CoordinateProfileVersioningAndRunSnapshots","legacyProfileIdPreserved":true}',
                    '{"migration":"CoordinateProfileVersioningAndRunSnapshots","status":"LegacyCoordinateBaseline"}',
                    NULL,
                    NULL,
                    NULL,
                    p.created_at_utc,
                    CASE WHEN p.is_active = 1 THEN p.created_at_utc ELSE NULL END,
                    CASE WHEN p.is_active = 1 THEN p.created_at_utc ELSE NULL END,
                    NULL
                FROM coordinate_profiles p
                WHERE NOT EXISTS (
                    SELECT 1 FROM coordinate_profile_versions v WHERE v.id = p.id
                );
                """);

            migrationBuilder.Sql("""
                UPDATE coordinate_profiles
                SET
                    status = CASE
                        WHEN is_active = 1 THEN 'Enabled'
                        ELSE 'NeedsManualResolution'
                    END,
                    active_version_id = CASE
                        WHEN is_active = 1 THEN id
                        ELSE NULL
                    END;
                """);

            migrationBuilder.Sql("""
                UPDATE coordinate_points
                SET
                    coordinate_profile_version_id = coordinate_profile_id,
                    validation_status = CASE
                        WHEN requires_calibration = 1 THEN 'NeedsCalibration'
                        ELSE 'Validated'
                    END,
                    validation_message = CASE
                        WHEN requires_calibration = 1 THEN 'Migrated legacy point requires calibration.'
                        ELSE 'Migrated legacy point.'
                    END
                WHERE coordinate_profile_version_id = '';
                """);

            migrationBuilder.Sql("""
                UPDATE coordinate_calibration_history
                SET
                    coordinate_profile_version_id = (
                        SELECT p.coordinate_profile_version_id
                        FROM coordinate_points p
                        WHERE p.id = coordinate_calibration_history.coordinate_point_id
                    ),
                    change_summary_json = CASE WHEN change_summary_json = '' THEN '{}' ELSE change_summary_json END,
                    validation_result_json = CASE WHEN validation_result_json = '' THEN '{}' ELSE validation_result_json END;
                """);

            migrationBuilder.Sql("""
                UPDATE channel_batches
                SET
                    coordinate_selection_status = CASE
                        WHEN workflow_selection_status IN ('Selected', 'Locked')
                             OR machine_run_id IS NOT NULL
                             OR started_at_utc IS NOT NULL
                        THEN 'NeedsManualResolution'
                        ELSE 'Unselected'
                    END,
                    coordinate_snapshot_json = '{}',
                    coordinate_profile_version_id = NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE machine_runs
                SET
                    coordinate_snapshot_json = '{}',
                    coordinate_profile_version_id = NULL;
                """);

            migrationBuilder.Sql("""
                INSERT INTO coordinate_points (
                    id,
                    coordinate_profile_id,
                    coordinate_profile_version_id,
                    point_code,
                    point_type,
                    preset_x_um,
                    preset_y_um,
                    calibrated_x_um,
                    calibrated_y_um,
                    calibrated_z_um,
                    safe_z_um,
                    liquid_detect_z_um,
                    dispense_z_um,
                    action_offset_x_um,
                    action_offset_y_um,
                    action_offset_z_um,
                    requires_calibration,
                    validation_status,
                    validation_message,
                    is_enabled,
                    created_at_utc,
                    updated_at_utc)
                SELECT
                    lower(hex(randomblob(16))),
                    v.coordinate_profile_id,
                    v.id,
                    required.point_code,
                    required.point_type,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    1,
                    'NeedsCalibration',
                    'Required logical target added by coordinate version migration; calibration required.',
                    1,
                    v.created_at_utc,
                    NULL
                FROM coordinate_profile_versions v
                CROSS JOIN (
                    SELECT 'SampleScan' AS point_code, 'SampleScanPosition' AS point_type
                    UNION ALL SELECT 'DabA', 'DabSourceBottle'
                    UNION ALL SELECT 'DabB', 'DabSourceBottle'
                ) required
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM coordinate_points p
                    WHERE p.coordinate_profile_version_id = v.id
                      AND p.point_code = required.point_code
                );
                """);

            migrationBuilder.CreateIndex(
                name: "IX_machine_runs_coordinate_profile_version_id",
                table: "machine_runs",
                column: "coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profiles_active_version_id",
                table: "coordinate_profiles",
                column: "active_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_points_coordinate_profile_id_point_code",
                table: "coordinate_points",
                columns: new[] { "coordinate_profile_id", "point_code" });

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_points_coordinate_profile_version_id_point_code",
                table: "coordinate_points",
                columns: new[] { "coordinate_profile_version_id", "point_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_calibration_history_coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                column: "coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_calibration_history_source_coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                column: "source_coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_batches_coordinate_profile_version_id",
                table: "channel_batches",
                column: "coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_activated_by_user_id",
                table: "coordinate_profile_versions",
                column: "activated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_coordinate_profile_id_version_label",
                table: "coordinate_profile_versions",
                columns: new[] { "coordinate_profile_id", "version_label" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_coordinate_profile_id_version_no",
                table: "coordinate_profile_versions",
                columns: new[] { "coordinate_profile_id", "version_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_created_by_user_id",
                table: "coordinate_profile_versions",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_published_by_user_id",
                table: "coordinate_profile_versions",
                column: "published_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_source_version_id",
                table: "coordinate_profile_versions",
                column: "source_version_id");

            migrationBuilder.AddForeignKey(
                name: "FK_channel_batches_coordinate_profile_versions_coordinate_profile_version_id",
                table: "channel_batches",
                column: "coordinate_profile_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_coordinate_calibration_history_coordinate_profile_versions_coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                column: "coordinate_profile_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_coordinate_calibration_history_coordinate_profile_versions_source_coordinate_profile_version_id",
                table: "coordinate_calibration_history",
                column: "source_coordinate_profile_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_coordinate_points_coordinate_profile_versions_coordinate_profile_version_id",
                table: "coordinate_points",
                column: "coordinate_profile_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_coordinate_profiles_coordinate_profile_versions_active_version_id",
                table: "coordinate_profiles",
                column: "active_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_machine_runs_coordinate_profile_versions_coordinate_profile_version_id",
                table: "machine_runs",
                column: "coordinate_profile_version_id",
                principalTable: "coordinate_profile_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channel_batches_coordinate_profile_versions_coordinate_profile_version_id",
                table: "channel_batches");

            migrationBuilder.DropForeignKey(
                name: "FK_coordinate_calibration_history_coordinate_profile_versions_coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropForeignKey(
                name: "FK_coordinate_calibration_history_coordinate_profile_versions_source_coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropForeignKey(
                name: "FK_coordinate_points_coordinate_profile_versions_coordinate_profile_version_id",
                table: "coordinate_points");

            migrationBuilder.DropForeignKey(
                name: "FK_coordinate_profiles_coordinate_profile_versions_active_version_id",
                table: "coordinate_profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_machine_runs_coordinate_profile_versions_coordinate_profile_version_id",
                table: "machine_runs");

            migrationBuilder.DropTable(
                name: "coordinate_profile_versions");

            migrationBuilder.DropIndex(
                name: "IX_machine_runs_coordinate_profile_version_id",
                table: "machine_runs");

            migrationBuilder.DropIndex(
                name: "IX_coordinate_profiles_active_version_id",
                table: "coordinate_profiles");

            migrationBuilder.DropIndex(
                name: "IX_coordinate_points_coordinate_profile_id_point_code",
                table: "coordinate_points");

            migrationBuilder.DropIndex(
                name: "IX_coordinate_points_coordinate_profile_version_id_point_code",
                table: "coordinate_points");

            migrationBuilder.DropIndex(
                name: "IX_coordinate_calibration_history_coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropIndex(
                name: "IX_coordinate_calibration_history_source_coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropIndex(
                name: "IX_channel_batches_coordinate_profile_version_id",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "coordinate_profile_version_id",
                table: "machine_runs");

            migrationBuilder.DropColumn(
                name: "coordinate_snapshot_json",
                table: "machine_runs");

            migrationBuilder.DropColumn(
                name: "active_version_id",
                table: "coordinate_profiles");

            migrationBuilder.DropColumn(
                name: "action_offset_x_um",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "action_offset_y_um",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "action_offset_z_um",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "calibrated_z_um",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "coordinate_profile_version_id",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "validation_message",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "validation_status",
                table: "coordinate_points");

            migrationBuilder.DropColumn(
                name: "action_offset_x_um",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "action_offset_y_um",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "action_offset_z_um",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "change_summary_json",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "new_z_um",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "source_coordinate_profile_version_id",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "validation_result_json",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "coordinate_profile_version_id",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "coordinate_selection_status",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "coordinate_snapshot_json",
                table: "channel_batches");

            migrationBuilder.RenameColumn(
                name: "liquid_detect_z_um",
                table: "coordinate_points",
                newName: "aspirate_z_um");

            migrationBuilder.RenameColumn(
                name: "liquid_detect_z_um",
                table: "coordinate_calibration_history",
                newName: "aspirate_z_um");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_points_coordinate_profile_id_point_code",
                table: "coordinate_points",
                columns: new[] { "coordinate_profile_id", "point_code" },
                unique: true);
        }
    }
}
