using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PersistWorkflowPlanningRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "planning_rules_json",
                table: "workflow_versions",
                type: "TEXT",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "planning_rules_json",
                table: "workflow_versions");
        }
    }
}
