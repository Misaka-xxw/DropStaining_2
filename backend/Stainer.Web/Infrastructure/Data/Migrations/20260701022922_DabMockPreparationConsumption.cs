using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DabMockPreparationConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dab_batch_id",
                table: "reagent_consumptions",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "device_command_execution_id",
                table: "reagent_consumptions",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_role",
                table: "reagent_consumptions",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "system_liquid_usages",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    machine_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    workflow_step_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    device_command_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    dab_batch_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, defaultValue: "SystemWater"),
                    volume_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    level_snapshot_json = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_liquid_usages", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_liquid_usages_dab_batches_dab_batch_id",
                        column: x => x.dab_batch_id,
                        principalTable: "dab_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_system_liquid_usages_device_command_executions_device_command_execution_id",
                        column: x => x.device_command_execution_id,
                        principalTable: "device_command_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_system_liquid_usages_machine_runs_machine_run_id",
                        column: x => x.machine_run_id,
                        principalTable: "machine_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_system_liquid_usages_workflow_step_executions_workflow_step_execution_id",
                        column: x => x.workflow_step_execution_id,
                        principalTable: "workflow_step_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reagent_consumptions_dab_batch_id_device_command_execution_id",
                table: "reagent_consumptions",
                columns: new[] { "dab_batch_id", "device_command_execution_id" });

            migrationBuilder.CreateIndex(
                name: "IX_reagent_consumptions_device_command_execution_id",
                table: "reagent_consumptions",
                column: "device_command_execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_liquid_usages_dab_batch_id_device_command_execution_id",
                table: "system_liquid_usages",
                columns: new[] { "dab_batch_id", "device_command_execution_id" });

            migrationBuilder.CreateIndex(
                name: "IX_system_liquid_usages_device_command_execution_id",
                table: "system_liquid_usages",
                column: "device_command_execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_liquid_usages_machine_run_id_source_type",
                table: "system_liquid_usages",
                columns: new[] { "machine_run_id", "source_type" });

            migrationBuilder.CreateIndex(
                name: "IX_system_liquid_usages_workflow_step_execution_id",
                table: "system_liquid_usages",
                column: "workflow_step_execution_id");

            migrationBuilder.AddForeignKey(
                name: "FK_reagent_consumptions_dab_batches_dab_batch_id",
                table: "reagent_consumptions",
                column: "dab_batch_id",
                principalTable: "dab_batches",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_reagent_consumptions_device_command_executions_device_command_execution_id",
                table: "reagent_consumptions",
                column: "device_command_execution_id",
                principalTable: "device_command_executions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reagent_consumptions_dab_batches_dab_batch_id",
                table: "reagent_consumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_reagent_consumptions_device_command_executions_device_command_execution_id",
                table: "reagent_consumptions");

            migrationBuilder.DropTable(
                name: "system_liquid_usages");

            migrationBuilder.DropIndex(
                name: "IX_reagent_consumptions_dab_batch_id_device_command_execution_id",
                table: "reagent_consumptions");

            migrationBuilder.DropIndex(
                name: "IX_reagent_consumptions_device_command_execution_id",
                table: "reagent_consumptions");

            migrationBuilder.DropColumn(
                name: "dab_batch_id",
                table: "reagent_consumptions");

            migrationBuilder.DropColumn(
                name: "device_command_execution_id",
                table: "reagent_consumptions");

            migrationBuilder.DropColumn(
                name: "source_role",
                table: "reagent_consumptions");
        }
    }
}
