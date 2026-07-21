using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLiquidClassExtraParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "aspirate_post_delay_ms",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "dispense_liquid_detection_enabled",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "dispense_post_delay_ms",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "dispense_retract_speed_um_per_second",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "system_trailing_air_gap_ul",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "trailing_air_gap_after_each_dispense_enabled",
                table: "liquid_class_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aspirate_post_delay_ms",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "dispense_liquid_detection_enabled",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "dispense_post_delay_ms",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "dispense_retract_speed_um_per_second",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "system_trailing_air_gap_ul",
                table: "liquid_class_versions");

            migrationBuilder.DropColumn(
                name: "trailing_air_gap_after_each_dispense_enabled",
                table: "liquid_class_versions");
        }
    }
}
