using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSettingsProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_settings_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    scope_key = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    data_interface = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    host_address = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    heartbeat_sec = table.Column<int>(type: "INTEGER", nullable: true),
                    reagent_bottle_capacity_ml = table.Column<decimal>(type: "TEXT", nullable: true),
                    reagent_target_temp_c = table.Column<decimal>(type: "TEXT", nullable: true),
                    work_target_temp_c = table.Column<decimal>(type: "TEXT", nullable: true),
                    needle_gap_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_settings_profiles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_settings_profiles_scope_key",
                table: "app_settings_profiles",
                column: "scope_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_settings_profiles");
        }
    }
}
