using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Stainer.Web.Infrastructure.Data;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(StainerDbContext))]
    [Migration("20260709120000_CoordinateAndLiquidClassParamCompleteness")]
    public partial class CoordinateAndLiquidClassParamCompleteness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Coordinate Z-End (Z semantic completeness): lowest Z reached while tracking the
            // liquid surface during aspiration. Nullable so existing points stay valid.
            migrationBuilder.AddColumn<long>(
                name: "aspirate_end_z_um",
                table: "coordinate_points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "aspirate_end_z_um",
                table: "coordinate_calibration_history",
                type: "INTEGER",
                nullable: true);

            // Liquid Class parameter completeness: aspiration tracking + dispense shaping.
            migrationBuilder.AddColumn<int>(
                name: "liquid_following_depth_um",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "retract_speed_um_per_second",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "conditioning_volume_ul",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "breakoff_speed_ul_per_second",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "post_dispense_air_gap_ul",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "post_dispense_air_gap_ul",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "breakoff_speed_ul_per_second",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "conditioning_volume_ul",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "retract_speed_um_per_second",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "liquid_following_depth_um",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "aspirate_end_z_um",
                table: "coordinate_calibration_history");

            migrationBuilder.DropColumn(
                name: "aspirate_end_z_um",
                table: "coordinate_points");
        }
    }
}
