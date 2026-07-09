using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScannerConfigurationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scanner_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    scanner_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    port = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    baud_rate = table.Column<int>(type: "INTEGER", nullable: true),
                    timeout_milliseconds = table.Column<int>(type: "INTEGER", nullable: true),
                    trigger_mode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    roi_x = table.Column<int>(type: "INTEGER", nullable: true),
                    roi_y = table.Column<int>(type: "INTEGER", nullable: true),
                    roi_width = table.Column<int>(type: "INTEGER", nullable: true),
                    roi_height = table.Column<int>(type: "INTEGER", nullable: true),
                    check_light_enabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    special_parameters_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scanner_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scanner_regions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    region_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    scanner_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    scan_path_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    coordinate_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    coordinate_profile_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    coordinate_point_codes_json = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scanner_regions", x => x.id);
                    table.ForeignKey(
                        name: "FK_scanner_regions_coordinate_profile_versions_coordinate_profile_version_id",
                        column: x => x.coordinate_profile_version_id,
                        principalTable: "coordinate_profile_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_scanner_regions_coordinate_profiles_coordinate_profile_id",
                        column: x => x.coordinate_profile_id,
                        principalTable: "coordinate_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_scanner_regions_scanner_profiles_scanner_profile_id",
                        column: x => x.scanner_profile_id,
                        principalTable: "scanner_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_scanner_profiles_scanner_type_enabled",
                table: "scanner_profiles",
                columns: new[] { "scanner_type", "enabled" });

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_coordinate_profile_id",
                table: "scanner_regions",
                column: "coordinate_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_coordinate_profile_version_id",
                table: "scanner_regions",
                column: "coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_region_type",
                table: "scanner_regions",
                column: "region_type");

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_scanner_profile_id",
                table: "scanner_regions",
                column: "scanner_profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scanner_regions");

            migrationBuilder.DropTable(
                name: "scanner_profiles");
        }
    }
}
