using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialConnectionProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "serial_connection_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    device_key = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    port_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    baud_rate = table.Column<int>(type: "INTEGER", nullable: true),
                    data_bits = table.Column<int>(type: "INTEGER", nullable: true),
                    parity = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    stop_bits = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    handshake = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    read_timeout_milliseconds = table.Column<int>(type: "INTEGER", nullable: true),
                    write_timeout_milliseconds = table.Column<int>(type: "INTEGER", nullable: true),
                    enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serial_connection_profiles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_serial_connection_profiles_device_key",
                table: "serial_connection_profiles",
                column: "device_key",
                unique: true);

            // seed 默认主控串口配置（115200 / 8 / None / One / None）。PortName 留空 = 未配置，匹配 fail-closed 默认。
            migrationBuilder.InsertData(
                table: "serial_connection_profiles",
                columns: new[] { "id", "device_key", "port_name", "baud_rate", "data_bits", "parity", "stop_bits", "handshake", "read_timeout_milliseconds", "write_timeout_milliseconds", "enabled", "created_at_utc", "updated_at_utc" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "main-controller", null, 115200, 8, "None", "One", "None", 2000, 2000, true, new DateTimeOffset(2026, 7, 17, 0, 0, 0, TimeSpan.Zero), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "serial_connection_profiles");
        }
    }
}
