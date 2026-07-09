using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Stainer.Web.Infrastructure.Data;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(StainerDbContext))]
    [Migration("20260709110000_EngineeringPipettingOperationTracking")]
    public partial class EngineeringPipettingOperationTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "actor_user_id",
                table: "pipetting_operations",
                type: "TEXT",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "channel_code",
                table: "pipetting_operations",
                type: "TEXT",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "parameters_json",
                table: "pipetting_operations",
                type: "TEXT",
                maxLength: 16000,
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.CreateIndex(
                name: "IX_pipetting_operations_actor_user_id",
                table: "pipetting_operations",
                column: "actor_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pipetting_operations_actor_user_id",
                table: "pipetting_operations");

            migrationBuilder.DropColumn(
                name: "actor_user_id",
                table: "pipetting_operations");

            migrationBuilder.DropColumn(
                name: "channel_code",
                table: "pipetting_operations");

            migrationBuilder.DropColumn(
                name: "parameters_json",
                table: "pipetting_operations");
        }
    }
}
