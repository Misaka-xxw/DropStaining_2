using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class StainingTaskIhcCompatibilityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "compatibility_validation_message",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "compatibility_validation_status",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "confirmed_primary_antibody_code",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lis_candidate_primary_antibody_codes_json",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 40000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lis_query_log_id",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "normalized_sample_code",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "raw_sample_code",
                table: "staining_tasks",
                type: "TEXT",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_compatibility_validation_status",
                table: "staining_tasks",
                column: "compatibility_validation_status");

            migrationBuilder.CreateIndex(
                name: "IX_staining_tasks_confirmed_primary_antibody_code",
                table: "staining_tasks",
                column: "confirmed_primary_antibody_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_staining_tasks_compatibility_validation_status",
                table: "staining_tasks");

            migrationBuilder.DropIndex(
                name: "IX_staining_tasks_confirmed_primary_antibody_code",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "compatibility_validation_message",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "compatibility_validation_status",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "confirmed_primary_antibody_code",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "lis_candidate_primary_antibody_codes_json",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "lis_query_log_id",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "normalized_sample_code",
                table: "staining_tasks");

            migrationBuilder.DropColumn(
                name: "raw_sample_code",
                table: "staining_tasks");
        }
    }
}
