using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReagentPositionConfigPipette : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pipette_liquid_class_code",
                table: "reagent_position_configs",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pipette_needle_code",
                table: "reagent_position_configs",
                type: "TEXT",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pipette_volume_ul",
                table: "reagent_position_configs",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pipette_liquid_class_code",
                table: "reagent_position_configs");

            migrationBuilder.DropColumn(
                name: "pipette_needle_code",
                table: "reagent_position_configs");

            migrationBuilder.DropColumn(
                name: "pipette_volume_ul",
                table: "reagent_position_configs");
        }
    }
}
