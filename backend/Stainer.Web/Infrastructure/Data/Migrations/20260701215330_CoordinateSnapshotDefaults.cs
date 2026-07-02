using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoordinateSnapshotDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "coordinate_snapshot_json",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40000);

            migrationBuilder.AlterColumn<string>(
                name: "validation_status",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "Unverified",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "validation_message",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "coordinate_snapshot_json",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40000);

            migrationBuilder.AlterColumn<string>(
                name: "coordinate_selection_status",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                defaultValue: "Unselected",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 32);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "coordinate_snapshot_json",
                table: "machine_runs",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40000,
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "validation_status",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64,
                oldDefaultValue: "Unverified");

            migrationBuilder.AlterColumn<string>(
                name: "validation_message",
                table: "coordinate_points",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2000,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "coordinate_snapshot_json",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 40000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40000,
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "coordinate_selection_status",
                table: "channel_batches",
                type: "TEXT",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 32,
                oldDefaultValue: "Unselected");
        }
    }
}
