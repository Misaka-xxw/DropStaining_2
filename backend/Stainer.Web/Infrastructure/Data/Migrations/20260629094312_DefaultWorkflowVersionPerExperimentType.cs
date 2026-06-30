using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DefaultWorkflowVersionPerExperimentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "default_experiment_type",
                table: "workflow_versions",
                type: "TEXT",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_workflow_versions_default_experiment_type",
                table: "workflow_versions",
                column: "default_experiment_type",
                unique: true,
                filter: "default_experiment_type IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "ck_workflow_versions_default_experiment_type",
                table: "workflow_versions",
                sql: "default_experiment_type is null or (default_experiment_type in ('HE', 'IHC') and status = 'Published')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_workflow_versions_default_experiment_type",
                table: "workflow_versions");

            migrationBuilder.DropCheckConstraint(
                name: "ck_workflow_versions_default_experiment_type",
                table: "workflow_versions");

            migrationBuilder.DropColumn(
                name: "default_experiment_type",
                table: "workflow_versions");
        }
    }
}
