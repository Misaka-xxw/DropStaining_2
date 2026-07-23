using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReagentPositionConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reagent_position_configs",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    rack_code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    calibrated_x_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    calibrated_y_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    safe_z_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    liquid_detect_z_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    aspirate_end_z_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    dispense_z_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reagent_position_configs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reagent_position_configs_rack_code",
                table: "reagent_position_configs",
                column: "rack_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reagent_position_configs");
        }
    }
}
