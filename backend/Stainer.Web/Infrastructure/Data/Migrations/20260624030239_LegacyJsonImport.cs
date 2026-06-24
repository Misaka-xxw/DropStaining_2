using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class LegacyJsonImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "version_label",
                table: "workflow_versions",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "legacy_parameters_json",
                table: "workflow_steps",
                type: "TEXT",
                maxLength: 8000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "step_name",
                table: "workflow_steps",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "workflow_definitions",
                type: "TEXT",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "users",
                type: "TEXT",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password_hash_algorithm",
                table: "users",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_updated_at_utc",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "legacy_metadata_json",
                table: "reagent_definitions",
                type: "TEXT",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "minimum_alarm_volume_ul",
                table: "reagent_definitions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reagent_type",
                table: "reagent_definitions",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "excess_volume_ul",
                table: "liquid_class_profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "leading_air_gap_ul",
                table: "liquid_class_profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "legacy_parameters_json",
                table: "liquid_class_profiles",
                type: "TEXT",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "trailing_air_gap_ul",
                table: "liquid_class_profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "legacy_import_runs",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    imported_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    source_path = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    source_hash_json = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    is_dry_run = table.Column<bool>(type: "INTEGER", nullable: false),
                    result = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    statistics_json = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    report_path = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legacy_import_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "legacy_import_issues",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    legacy_import_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    file_path = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    record_identifier = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    field_name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    issue_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    raw_fragment = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legacy_import_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_legacy_import_issues_legacy_import_runs_legacy_import_run_id",
                        column: x => x.legacy_import_run_id,
                        principalTable: "legacy_import_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "legacy_runtime_snapshots",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    legacy_import_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source_file_path = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    source_file_hash = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    run_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    captured_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    snapshot_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legacy_runtime_snapshots", x => x.id);
                    table.ForeignKey(
                        name: "FK_legacy_runtime_snapshots_legacy_import_runs_legacy_import_run_id",
                        column: x => x.legacy_import_run_id,
                        principalTable: "legacy_import_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_versions_workflow_definition_id_version_label",
                table: "workflow_versions",
                columns: new[] { "workflow_definition_id", "version_label" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_legacy_import_issues_issue_type",
                table: "legacy_import_issues",
                column: "issue_type");

            migrationBuilder.CreateIndex(
                name: "IX_legacy_import_issues_legacy_import_run_id",
                table: "legacy_import_issues",
                column: "legacy_import_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_legacy_import_runs_imported_at_utc",
                table: "legacy_import_runs",
                column: "imported_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_legacy_import_runs_source_path_imported_at_utc",
                table: "legacy_import_runs",
                columns: new[] { "source_path", "imported_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_legacy_runtime_snapshots_legacy_import_run_id",
                table: "legacy_runtime_snapshots",
                column: "legacy_import_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_legacy_runtime_snapshots_source_file_hash",
                table: "legacy_runtime_snapshots",
                column: "source_file_hash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "legacy_import_issues");

            migrationBuilder.DropTable(
                name: "legacy_runtime_snapshots");

            migrationBuilder.DropTable(
                name: "legacy_import_runs");

            migrationBuilder.DropIndex(
                name: "IX_workflow_versions_workflow_definition_id_version_label",
                table: "workflow_versions");

            migrationBuilder.DropColumn(
                name: "version_label",
                table: "workflow_versions");

            migrationBuilder.DropColumn(
                name: "legacy_parameters_json",
                table: "workflow_steps");

            migrationBuilder.DropColumn(
                name: "step_name",
                table: "workflow_steps");

            migrationBuilder.DropColumn(
                name: "description",
                table: "workflow_definitions");

            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_hash_algorithm",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_updated_at_utc",
                table: "users");

            migrationBuilder.DropColumn(
                name: "legacy_metadata_json",
                table: "reagent_definitions");

            migrationBuilder.DropColumn(
                name: "minimum_alarm_volume_ul",
                table: "reagent_definitions");

            migrationBuilder.DropColumn(
                name: "reagent_type",
                table: "reagent_definitions");

            migrationBuilder.DropColumn(
                name: "excess_volume_ul",
                table: "liquid_class_profiles");

            migrationBuilder.DropColumn(
                name: "leading_air_gap_ul",
                table: "liquid_class_profiles");

            migrationBuilder.DropColumn(
                name: "legacy_parameters_json",
                table: "liquid_class_profiles");

            migrationBuilder.DropColumn(
                name: "trailing_air_gap_ul",
                table: "liquid_class_profiles");
        }
    }
}
