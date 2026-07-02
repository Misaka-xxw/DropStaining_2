using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThermalControlAndCoolingMock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "target_temperature_deci_c",
                table: "workflow_step_executions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cooling_unit_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    current_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    target_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    fault_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cooling_unit_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "temperature_telemetry",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source_type = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    source_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    drawer_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    board_no = table.Column<int>(type: "INTEGER", nullable: true),
                    slot_no = table.Column<int>(type: "INTEGER", nullable: true),
                    point_no = table.Column<int>(type: "INTEGER", nullable: true),
                    current_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    target_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    recorded_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temperature_telemetry", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "thermal_point_states",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    drawer_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    board_no = table.Column<int>(type: "INTEGER", nullable: false),
                    slot_no = table.Column<int>(type: "INTEGER", nullable: false),
                    point_no = table.Column<int>(type: "INTEGER", nullable: false),
                    current_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    target_temperature_deci_c = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    fault_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    fault_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thermal_point_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_temperature_telemetry_recorded_at_utc",
                table: "temperature_telemetry",
                column: "recorded_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_temperature_telemetry_source_type_source_id_recorded_at_utc",
                table: "temperature_telemetry",
                columns: new[] { "source_type", "source_id", "recorded_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_thermal_point_states_board_no_point_no",
                table: "thermal_point_states",
                columns: new[] { "board_no", "point_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_thermal_point_states_drawer_code_slot_no",
                table: "thermal_point_states",
                columns: new[] { "drawer_code", "slot_no" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cooling_unit_states");

            migrationBuilder.DropTable(
                name: "temperature_telemetry");

            migrationBuilder.DropTable(
                name: "thermal_point_states");

            migrationBuilder.DropColumn(
                name: "target_temperature_deci_c",
                table: "workflow_step_executions");
        }
    }
}
