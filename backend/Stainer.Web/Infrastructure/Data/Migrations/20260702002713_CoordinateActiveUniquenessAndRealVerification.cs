using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoordinateActiveUniquenessAndRealVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions");

            migrationBuilder.AddColumn<string>(
                name: "usage_scope",
                table: "coordinate_profile_versions",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "MockOnly");

            migrationBuilder.AddColumn<string>(
                name: "verification_status",
                table: "coordinate_profile_versions",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unverified");

            migrationBuilder.Sql("""
                WITH ranked_active AS (
                    SELECT
                        v.id,
                        ROW_NUMBER() OVER (
                            PARTITION BY v.coordinate_profile_id
                            ORDER BY
                                CASE WHEN p.active_version_id = v.id THEN 0 ELSE 1 END,
                                v.activated_at_utc DESC,
                                v.created_at_utc DESC,
                                v.id
                        ) AS active_rank
                    FROM coordinate_profile_versions v
                    INNER JOIN coordinate_profiles p ON p.id = v.coordinate_profile_id
                    WHERE v.is_active = 1
                )
                UPDATE coordinate_profile_versions
                SET
                    is_active = 0,
                    status = 'NeedsManualResolution',
                    retired_at_utc = COALESCE(retired_at_utc, CURRENT_TIMESTAMP)
                WHERE id IN (
                    SELECT id FROM ranked_active WHERE active_rank > 1
                );
                """);

            migrationBuilder.Sql("""
                UPDATE coordinate_profiles
                SET active_version_id = (
                    SELECT v.id
                    FROM coordinate_profile_versions v
                    WHERE v.coordinate_profile_id = coordinate_profiles.id
                      AND v.is_active = 1
                    ORDER BY v.activated_at_utc DESC, v.created_at_utc DESC, v.id
                    LIMIT 1
                )
                WHERE EXISTS (
                    SELECT 1
                    FROM coordinate_profile_versions v
                    WHERE v.coordinate_profile_id = coordinate_profiles.id
                      AND v.is_active = 1
                );
                """);

            migrationBuilder.CreateIndex(
                name: "UX_coordinate_profile_versions_profile_active",
                table: "coordinate_profile_versions",
                column: "coordinate_profile_id",
                unique: true,
                filter: "is_active = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_coordinate_profile_versions_profile_active",
                table: "coordinate_profile_versions");

            migrationBuilder.DropColumn(
                name: "usage_scope",
                table: "coordinate_profile_versions");

            migrationBuilder.DropColumn(
                name: "verification_status",
                table: "coordinate_profile_versions");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profile_versions_is_active",
                table: "coordinate_profile_versions",
                column: "is_active");
        }
    }
}
