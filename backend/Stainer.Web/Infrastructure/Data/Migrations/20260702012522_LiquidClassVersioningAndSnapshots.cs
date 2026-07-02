using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class LiquidClassVersioningAndSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "liquid_class_selection_status",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unselected");

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_snapshot_json",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 80000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "enabled_version_id",
                table: "liquid_class_profiles",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_parameters_json",
                table: "device_command_executions",
                type: "TEXT",
                maxLength: 16000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_selection_status",
                table: "device_command_executions",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "NotApplicable");

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_version_id",
                table: "device_command_executions",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "liquid_class_version_no",
                table: "device_command_executions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_selection_status",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unselected");

            migrationBuilder.AddColumn<string>(
                name: "liquid_class_snapshot_json",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 80000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.Sql(
                """
                UPDATE channel_batches
                SET liquid_class_selection_status = 'NeedsManualResolution',
                    needs_manual_resolution = 1,
                    manual_resolution_reason = CASE
                        WHEN trim(manual_resolution_reason) = '' THEN 'Legacy channel batch has no frozen Liquid Class version.'
                        ELSE manual_resolution_reason || ' Legacy channel batch has no frozen Liquid Class version.'
                    END;

                UPDATE machine_runs
                SET liquid_class_selection_status = 'NeedsManualResolution';

                UPDATE device_command_executions
                SET liquid_class_selection_status = CASE
                    WHEN lower(command_type) LIKE '%aspirat%'
                      OR lower(command_type) LIKE '%dispens%'
                      OR lower(command_type) LIKE '%pipett%'
                      OR lower(command_type) LIKE '%liquid%detect%'
                      OR lower(command_type) LIKE '%blowout%'
                      OR lower(command_type) = 'dab'
                    THEN 'NeedsManualResolution'
                    ELSE 'NotApplicable'
                END;
                """);

            migrationBuilder.CreateTable(
                name: "liquid_class_versions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    liquid_class_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    version_no = table.Column<int>(type: "INTEGER", nullable: false),
                    version_label = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    source_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    change_reason = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    change_summary_json = table.Column<string>(type: "TEXT", maxLength: 16000, nullable: false),
                    liquid_detection_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    liquid_detection_sensitivity_percent = table.Column<int>(type: "INTEGER", nullable: false),
                    liquid_detection_speed_um_per_second = table.Column<int>(type: "INTEGER", nullable: false),
                    aspirate_speed_ul_per_second = table.Column<int>(type: "INTEGER", nullable: false),
                    aspirate_delay_ms = table.Column<int>(type: "INTEGER", nullable: false),
                    dispense_speed_ul_per_second = table.Column<int>(type: "INTEGER", nullable: false),
                    dispense_delay_ms = table.Column<int>(type: "INTEGER", nullable: false),
                    leading_air_gap_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    trailing_air_gap_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    blowout_volume_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    blowout_delay_ms = table.Column<int>(type: "INTEGER", nullable: false),
                    volume_adjustment_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    pre_wet_cycles = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_cycles = table.Column<int>(type: "INTEGER", nullable: false),
                    created_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    published_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    enabled_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    published_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    enabled_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liquid_class_versions", x => x.id);
                    table.CheckConstraint("ck_liquid_class_versions_status", "status in ('Draft', 'Published', 'Enabled')");
                    table.ForeignKey(
                        name: "FK_liquid_class_versions_liquid_class_profiles_liquid_class_profile_id",
                        column: x => x.liquid_class_profile_id,
                        principalTable: "liquid_class_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_liquid_class_versions_liquid_class_versions_source_version_id",
                        column: x => x.source_version_id,
                        principalTable: "liquid_class_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_liquid_class_versions_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_liquid_class_versions_users_enabled_by_user_id",
                        column: x => x.enabled_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_liquid_class_versions_users_published_by_user_id",
                        column: x => x.published_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "liquid_class_validation_records",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    liquid_class_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    stage = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    is_valid = table.Column<bool>(type: "INTEGER", nullable: false),
                    result_json = table.Column<string>(type: "TEXT", maxLength: 16000, nullable: false),
                    validated_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liquid_class_validation_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_liquid_class_validation_records_liquid_class_versions_liquid_class_version_id",
                        column: x => x.liquid_class_version_id,
                        principalTable: "liquid_class_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_liquid_class_validation_records_users_validated_by_user_id",
                        column: x => x.validated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "liquid_class_version_differences",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    liquid_class_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    parameter_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    previous_value = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    new_value = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    unit = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liquid_class_version_differences", x => x.id);
                    table.ForeignKey(
                        name: "FK_liquid_class_version_differences_liquid_class_versions_liquid_class_version_id",
                        column: x => x.liquid_class_version_id,
                        principalTable: "liquid_class_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_profiles_enabled_version_id",
                table: "liquid_class_profiles",
                column: "enabled_version_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_command_executions_liquid_class_version_id",
                table: "device_command_executions",
                column: "liquid_class_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_validation_records_liquid_class_version_id_created_at_utc",
                table: "liquid_class_validation_records",
                columns: new[] { "liquid_class_version_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_validation_records_validated_by_user_id",
                table: "liquid_class_validation_records",
                column: "validated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_version_differences_liquid_class_version_id_parameter_name",
                table: "liquid_class_version_differences",
                columns: new[] { "liquid_class_version_id", "parameter_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_created_by_user_id",
                table: "liquid_class_versions",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_enabled_by_user_id",
                table: "liquid_class_versions",
                column: "enabled_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_liquid_class_profile_id",
                table: "liquid_class_versions",
                column: "liquid_class_profile_id",
                unique: true,
                filter: "status = 'Enabled'");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_liquid_class_profile_id_version_no",
                table: "liquid_class_versions",
                columns: new[] { "liquid_class_profile_id", "version_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_published_by_user_id",
                table: "liquid_class_versions",
                column: "published_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_liquid_class_versions_source_version_id",
                table: "liquid_class_versions",
                column: "source_version_id");

            migrationBuilder.AddForeignKey(
                name: "FK_device_command_executions_liquid_class_versions_liquid_class_version_id",
                table: "device_command_executions",
                column: "liquid_class_version_id",
                principalTable: "liquid_class_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_liquid_class_profiles_liquid_class_versions_enabled_version_id",
                table: "liquid_class_profiles",
                column: "enabled_version_id",
                principalTable: "liquid_class_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_device_command_executions_liquid_class_versions_liquid_class_version_id",
                table: "device_command_executions");

            migrationBuilder.DropForeignKey(
                name: "FK_liquid_class_profiles_liquid_class_versions_enabled_version_id",
                table: "liquid_class_profiles");

            migrationBuilder.DropTable(
                name: "liquid_class_validation_records");

            migrationBuilder.DropTable(
                name: "liquid_class_version_differences");

            migrationBuilder.DropTable(
                name: "liquid_class_versions");

            migrationBuilder.DropIndex(
                name: "IX_liquid_class_profiles_enabled_version_id",
                table: "liquid_class_profiles");

            migrationBuilder.DropIndex(
                name: "IX_device_command_executions_liquid_class_version_id",
                table: "device_command_executions");

            migrationBuilder.DropColumn(
                name: "liquid_class_selection_status",
                table: "machine_runs");

            migrationBuilder.DropColumn(
                name: "liquid_class_snapshot_json",
                table: "machine_runs");

            migrationBuilder.DropColumn(
                name: "enabled_version_id",
                table: "liquid_class_profiles");

            migrationBuilder.DropColumn(
                name: "liquid_class_parameters_json",
                table: "device_command_executions");

            migrationBuilder.DropColumn(
                name: "liquid_class_selection_status",
                table: "device_command_executions");

            migrationBuilder.DropColumn(
                name: "liquid_class_version_id",
                table: "device_command_executions");

            migrationBuilder.DropColumn(
                name: "liquid_class_version_no",
                table: "device_command_executions");

            migrationBuilder.DropColumn(
                name: "liquid_class_selection_status",
                table: "channel_batches");

            migrationBuilder.DropColumn(
                name: "liquid_class_snapshot_json",
                table: "channel_batches");
        }
    }
}
