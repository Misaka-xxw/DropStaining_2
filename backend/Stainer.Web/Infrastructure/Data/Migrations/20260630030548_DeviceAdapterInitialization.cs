using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeviceAdapterInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device_initialization_runs",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    command_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    device_mode = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    adapter_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    attempt_no = table.Column<int>(type: "INTEGER", nullable: false),
                    retry_of_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    requested_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    failure_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    started_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    completed_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_initialization_runs", x => x.id);
                    table.CheckConstraint("ck_device_initialization_runs_status", "status in ('Running', 'Ready', 'Failed')");
                    table.ForeignKey(
                        name: "FK_device_initialization_runs_device_initialization_runs_retry_of_run_id",
                        column: x => x.retry_of_run_id,
                        principalTable: "device_initialization_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_device_initialization_runs_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "device_initialization_checks",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    device_initialization_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    step_no = table.Column<int>(type: "INTEGER", nullable: false),
                    module_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    error_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    result_json = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    started_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    completed_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_initialization_checks", x => x.id);
                    table.CheckConstraint("ck_device_initialization_checks_status", "status in ('Pending', 'Running', 'Succeeded', 'Failed', 'TimedOut', 'Unknown')");
                    table.ForeignKey(
                        name: "FK_device_initialization_checks_device_initialization_runs_device_initialization_run_id",
                        column: x => x.device_initialization_run_id,
                        principalTable: "device_initialization_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_checks_device_initialization_run_id_step_no",
                table: "device_initialization_checks",
                columns: new[] { "device_initialization_run_id", "step_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_checks_module_code_status",
                table: "device_initialization_checks",
                columns: new[] { "module_code", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_runs_command_id",
                table: "device_initialization_runs",
                column: "command_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_runs_requested_by_user_id",
                table: "device_initialization_runs",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_runs_retry_of_run_id",
                table: "device_initialization_runs",
                column: "retry_of_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_initialization_runs_status_created_at_utc",
                table: "device_initialization_runs",
                columns: new[] { "status", "created_at_utc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_initialization_checks");

            migrationBuilder.DropTable(
                name: "device_initialization_runs");
        }
    }
}
