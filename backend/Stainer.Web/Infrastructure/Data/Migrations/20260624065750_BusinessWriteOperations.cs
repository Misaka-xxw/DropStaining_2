using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class BusinessWriteOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "command_receipts",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    operation = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    request_hash = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    response_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    actor_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    entity_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    entity_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    error_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    completed_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_receipts", x => x.id);
                    table.ForeignKey(
                        name: "FK_command_receipts_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "hospital_barcode_mappings",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    hospital_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    primary_antibody_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hospital_barcode_mappings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "staining_tasks",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    task_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    task_type = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    physical_slot_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    workflow_definition_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    workflow_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    workflow_snapshot_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    input_mode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    raw_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    normalized_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    primary_antibody_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    candidate_results_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    created_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staining_tasks", x => x.id);
                    table.CheckConstraint("ck_staining_tasks_status", "status in ('Confirmed', 'Cancelled', 'Completed')");
                    table.CheckConstraint("ck_staining_tasks_task_type", "task_type in ('HE', 'IHC')");
                    table.ForeignKey(
                        name: "FK_staining_tasks_physical_slots_physical_slot_id",
                        column: x => x.physical_slot_id,
                        principalTable: "physical_slots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staining_tasks_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_staining_tasks_workflow_definitions_workflow_definition_id",
                        column: x => x.workflow_definition_id,
                        principalTable: "workflow_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staining_tasks_workflow_versions_workflow_version_id",
                        column: x => x.workflow_version_id,
                        principalTable: "workflow_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_command_receipts_actor_user_id",
                table: "command_receipts",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_command_receipts_command_id",
                table: "command_receipts",
                column: "command_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_command_receipts_operation_created_at_utc",
                table: "command_receipts",
                columns: new[] { "operation", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_hospital_barcode_mappings_hospital_code_primary_antibody_code",
                table: "hospital_barcode_mappings",
                columns: new[] { "hospital_code", "primary_antibody_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hospital_barcode_mappings_primary_antibody_code",
                table: "hospital_barcode_mappings",
                column: "primary_antibody_code");

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_created_by_user_id",
                table: "staining_tasks",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_physical_slot_id_status",
                table: "staining_tasks",
                columns: new[] { "physical_slot_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_primary_antibody_code",
                table: "staining_tasks",
                column: "primary_antibody_code");

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_task_code",
                table: "staining_tasks",
                column: "task_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_workflow_definition_id",
                table: "staining_tasks",
                column: "workflow_definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_workflow_version_id",
                table: "staining_tasks",
                column: "workflow_version_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "command_receipts");

            migrationBuilder.DropTable(
                name: "hospital_barcode_mappings");

            migrationBuilder.DropTable(
                name: "staining_tasks");
        }
    }
}
