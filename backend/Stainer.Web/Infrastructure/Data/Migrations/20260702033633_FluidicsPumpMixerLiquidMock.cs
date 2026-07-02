using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FluidicsPumpMixerLiquidMock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fluidics_telemetry",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source_type = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    source_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    event_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    pwm_channel_code = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    drawer_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    liquid_source_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    speed_percent = table.Column<int>(type: "INTEGER", nullable: true),
                    direction = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    current_volume_ul = table.Column<int>(type: "INTEGER", nullable: true),
                    capacity_ul = table.Column<int>(type: "INTEGER", nullable: true),
                    target_point_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    machine_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    workflow_step_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    device_command_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    recorded_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fluidics_telemetry", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "liquid_container_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    is_waste = table.Column<bool>(type: "INTEGER", nullable: false),
                    capacity_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    current_volume_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    low_threshold_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    full_threshold_ul = table.Column<int>(type: "INTEGER", nullable: false),
                    level_status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    fault_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liquid_container_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mixer_channel_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    drawer_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    channel_no = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    current_round_key = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    current_command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    machine_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    workflow_step_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    device_command_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    fault_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mixer_channel_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pump_channel_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    pwm_channel_code = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    pwm_channel_no = table.Column<int>(type: "INTEGER", nullable: false),
                    drawer_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    speed_percent = table.Column<int>(type: "INTEGER", nullable: false),
                    direction = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    target_point_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    duration_ms = table.Column<int>(type: "INTEGER", nullable: true),
                    current_command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    machine_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    workflow_step_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    device_command_execution_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    fault_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pump_channel_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fluidics_telemetry_machine_run_id_workflow_step_execution_id",
                table: "fluidics_telemetry",
                columns: new[] { "machine_run_id", "workflow_step_execution_id" });

            migrationBuilder.CreateIndex(
                name: "IX_fluidics_telemetry_recorded_at_utc",
                table: "fluidics_telemetry",
                column: "recorded_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_fluidics_telemetry_source_type_source_id_recorded_at_utc",
                table: "fluidics_telemetry",
                columns: new[] { "source_type", "source_id", "recorded_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_liquid_container_states_source_type",
                table: "liquid_container_states",
                column: "source_type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mixer_channel_states_channel_no",
                table: "mixer_channel_states",
                column: "channel_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mixer_channel_states_drawer_code",
                table: "mixer_channel_states",
                column: "drawer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mixer_channel_states_machine_run_id_workflow_step_execution_id",
                table: "mixer_channel_states",
                columns: new[] { "machine_run_id", "workflow_step_execution_id" });

            migrationBuilder.CreateIndex(
                name: "IX_pump_channel_states_drawer_code",
                table: "pump_channel_states",
                column: "drawer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pump_channel_states_machine_run_id_workflow_step_execution_id",
                table: "pump_channel_states",
                columns: new[] { "machine_run_id", "workflow_step_execution_id" });

            migrationBuilder.CreateIndex(
                name: "IX_pump_channel_states_pwm_channel_code",
                table: "pump_channel_states",
                column: "pwm_channel_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pump_channel_states_pwm_channel_no",
                table: "pump_channel_states",
                column: "pwm_channel_no",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fluidics_telemetry");

            migrationBuilder.DropTable(
                name: "liquid_container_states");

            migrationBuilder.DropTable(
                name: "mixer_channel_states");

            migrationBuilder.DropTable(
                name: "pump_channel_states");
        }
    }
}
