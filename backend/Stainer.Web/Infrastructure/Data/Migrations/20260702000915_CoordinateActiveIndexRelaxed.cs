using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoordinateActiveIndexRelaxed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions",
                column: "is_active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions",
                column: "is_active",
                unique: true,
                filter: "is_active = 1");
        }
    }
}
