using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SampleRegionAndReagentAnchorConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "region_no",
                table: "scanner_regions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "scan_order",
                table: "scanner_regions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "reagent_coordinate_anchors",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    coordinate_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    coordinate_profile_version_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    column_no = table.Column<int>(type: "INTEGER", nullable: false),
                    column_code = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    slot_count = table.Column<int>(type: "INTEGER", nullable: false),
                    start_x_um = table.Column<double>(type: "REAL", nullable: true),
                    start_y_um = table.Column<double>(type: "REAL", nullable: true),
                    start_z_um = table.Column<double>(type: "REAL", nullable: true),
                    end_x_um = table.Column<double>(type: "REAL", nullable: true),
                    end_y_um = table.Column<double>(type: "REAL", nullable: true),
                    end_z_um = table.Column<double>(type: "REAL", nullable: true),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reagent_coordinate_anchors", x => x.id);
                    table.ForeignKey(
                        name: "FK_reagent_coordinate_anchors_coordinate_profile_versions_coordinate_profile_version_id",
                        column: x => x.coordinate_profile_version_id,
                        principalTable: "coordinate_profile_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_reagent_coordinate_anchors_coordinate_profiles_coordinate_profile_id",
                        column: x => x.coordinate_profile_id,
                        principalTable: "coordinate_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_scanner_profile_id_region_no",
                table: "scanner_regions",
                columns: new[] { "scanner_profile_id", "region_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scanner_regions_scanner_profile_id_scan_order",
                table: "scanner_regions",
                columns: new[] { "scanner_profile_id", "scan_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reagent_coordinate_anchors_coordinate_profile_id",
                table: "reagent_coordinate_anchors",
                column: "coordinate_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_reagent_coordinate_anchors_coordinate_profile_id_column_no",
                table: "reagent_coordinate_anchors",
                columns: new[] { "coordinate_profile_id", "column_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reagent_coordinate_anchors_coordinate_profile_version_id",
                table: "reagent_coordinate_anchors",
                column: "coordinate_profile_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_reagent_coordinate_anchors_coordinate_profile_version_id_column_no",
                table: "reagent_coordinate_anchors",
                columns: new[] { "coordinate_profile_version_id", "column_no" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reagent_coordinate_anchors");

            migrationBuilder.DropIndex(
                name: "IX_scanner_regions_scanner_profile_id_region_no",
                table: "scanner_regions");

            migrationBuilder.DropIndex(
                name: "IX_scanner_regions_scanner_profile_id_scan_order",
                table: "scanner_regions");

            migrationBuilder.DropColumn(
                name: "region_no",
                table: "scanner_regions");

            migrationBuilder.DropColumn(
                name: "scan_order",
                table: "scanner_regions");
        }
    }
}
