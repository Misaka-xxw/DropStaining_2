using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReagentPositionConfigRoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "roi_height",
                table: "reagent_position_configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roi_left",
                table: "reagent_position_configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roi_top",
                table: "reagent_position_configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roi_width",
                table: "reagent_position_configs",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roi_height",
                table: "reagent_position_configs");

            migrationBuilder.DropColumn(
                name: "roi_left",
                table: "reagent_position_configs");

            migrationBuilder.DropColumn(
                name: "roi_top",
                table: "reagent_position_configs");

            migrationBuilder.DropColumn(
                name: "roi_width",
                table: "reagent_position_configs");
        }
    }
}
