using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DabExpiryRepreparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dab_repreparation_plans",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    expired_dab_batch_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    replacement_dab_batch_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    machine_run_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    reason = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dab_repreparation_plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_dab_repreparation_plans_dab_batches_expired_dab_batch_id",
                        column: x => x.expired_dab_batch_id,
                        principalTable: "dab_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dab_repreparation_plans_dab_batches_replacement_dab_batch_id",
                        column: x => x.replacement_dab_batch_id,
                        principalTable: "dab_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dab_repreparation_plans_machine_runs_machine_run_id",
                        column: x => x.machine_run_id,
                        principalTable: "machine_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dab_repreparation_plans_expired_dab_batch_id_machine_run_id",
                table: "dab_repreparation_plans",
                columns: new[] { "expired_dab_batch_id", "machine_run_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dab_repreparation_plans_machine_run_id",
                table: "dab_repreparation_plans",
                column: "machine_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_dab_repreparation_plans_replacement_dab_batch_id",
                table: "dab_repreparation_plans",
                column: "replacement_dab_batch_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dab_repreparation_plans");
        }
    }
}
