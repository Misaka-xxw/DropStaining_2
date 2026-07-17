using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrecisionCalibrationProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "precision_calibration_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    scope_key = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    move_offset_x_mm = table.Column<double>(type: "REAL", nullable: true),
                    move_offset_y_mm = table.Column<double>(type: "REAL", nullable: true),
                    dispense_target_volume_ul = table.Column<double>(type: "REAL", nullable: true),
                    dispense_measured_volume_ul = table.Column<double>(type: "REAL", nullable: true),
                    dispense_calibration_factor = table.Column<double>(type: "REAL", nullable: true),
                    enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_precision_calibration_profiles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_precision_calibration_profiles_scope_key",
                table: "precision_calibration_profiles",
                column: "scope_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "precision_calibration_profiles");
        }
    }
}
