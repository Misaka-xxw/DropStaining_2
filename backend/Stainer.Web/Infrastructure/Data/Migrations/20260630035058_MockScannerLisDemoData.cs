using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MockScannerLisDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lis_query_logs",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    source = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    raw_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    normalized_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    candidate_primary_antibody_codes_json = table.Column<string>(type: "TEXT", maxLength: 40000, nullable: false),
                    selected_primary_antibody_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    selected_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    selected_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    error_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    error_message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    exception_json = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    started_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    completed_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lis_query_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_lis_query_logs_users_selected_by_user_id",
                        column: x => x.selected_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mock_demo_data_tags",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    entity_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    entity_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    demo_key = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mock_demo_data_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mock_lis_entries",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    normalized_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    primary_antibody_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    scenario = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    metadata_json = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mock_lis_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sample_scan_sessions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    session_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    started_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    completed_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    created_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sample_scan_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sample_scan_sessions_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "sample_scan_items",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    sample_scan_session_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    slot_code = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    scan_kind = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    scan_status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    raw_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    normalized_code = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    primary_antibody_code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    error_reason = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    device_status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    scanned_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sample_scan_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_sample_scan_items_sample_scan_sessions_sample_scan_session_id",
                        column: x => x.sample_scan_session_id,
                        principalTable: "sample_scan_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lis_query_logs_normalized_code",
                table: "lis_query_logs",
                column: "normalized_code");

            migrationBuilder.CreateIndex(
                name: "IX_lis_query_logs_selected_by_user_id",
                table: "lis_query_logs",
                column: "selected_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_lis_query_logs_started_at_utc",
                table: "lis_query_logs",
                column: "started_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_lis_query_logs_status",
                table: "lis_query_logs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_mock_demo_data_tags_demo_key",
                table: "mock_demo_data_tags",
                column: "demo_key");

            migrationBuilder.CreateIndex(
                name: "IX_mock_demo_data_tags_entity_type_entity_id",
                table: "mock_demo_data_tags",
                columns: new[] { "entity_type", "entity_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mock_lis_entries_normalized_code_primary_antibody_code_scenario",
                table: "mock_lis_entries",
                columns: new[] { "normalized_code", "primary_antibody_code", "scenario" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mock_lis_entries_scenario",
                table: "mock_lis_entries",
                column: "scenario");

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_items_normalized_code",
                table: "sample_scan_items",
                column: "normalized_code");

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_items_sample_scan_session_id",
                table: "sample_scan_items",
                column: "sample_scan_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_items_scan_status",
                table: "sample_scan_items",
                column: "scan_status");

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_sessions_created_by_user_id",
                table: "sample_scan_sessions",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_sessions_session_code",
                table: "sample_scan_sessions",
                column: "session_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sample_scan_sessions_started_at_utc",
                table: "sample_scan_sessions",
                column: "started_at_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lis_query_logs");

            migrationBuilder.DropTable(
                name: "mock_demo_data_tags");

            migrationBuilder.DropTable(
                name: "mock_lis_entries");

            migrationBuilder.DropTable(
                name: "sample_scan_items");

            migrationBuilder.DropTable(
                name: "sample_scan_sessions");
        }
    }
}
