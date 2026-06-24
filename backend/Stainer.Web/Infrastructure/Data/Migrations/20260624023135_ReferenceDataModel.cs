using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stainer.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "coordinate_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    origin_definition = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinate_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dab_mix_positions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    position_no = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dab_mix_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drawers",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    sort_order = table.Column<int>(type: "INTEGER", nullable: false),
                    heat_board_id = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drawers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reagent_rack_positions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    position_no = table.Column<int>(type: "INTEGER", nullable: false),
                    column_no = table.Column<int>(type: "INTEGER", nullable: false),
                    row_no = table.Column<int>(type: "INTEGER", nullable: false),
                    scanner_channel_no = table.Column<int>(type: "INTEGER", nullable: false),
                    scanner_channel_code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reagent_rack_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    username = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wash_positions",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    wash_type = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wash_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coordinate_points",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    coordinate_profile_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    point_code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    point_type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    preset_x_um = table.Column<long>(type: "INTEGER", nullable: true),
                    preset_y_um = table.Column<long>(type: "INTEGER", nullable: true),
                    calibrated_x_um = table.Column<long>(type: "INTEGER", nullable: true),
                    calibrated_y_um = table.Column<long>(type: "INTEGER", nullable: true),
                    safe_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    aspirate_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    dispense_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    requires_calibration = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinate_points", x => x.id);
                    table.ForeignKey(
                        name: "FK_coordinate_points_coordinate_profiles_coordinate_profile_id",
                        column: x => x.coordinate_profile_id,
                        principalTable: "coordinate_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "physical_slots",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    drawer_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    slot_no = table.Column<int>(type: "INTEGER", nullable: false),
                    vertical_order_from_bottom = table.Column<int>(type: "INTEGER", nullable: false),
                    heat_point_id = table.Column<int>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_physical_slots", x => x.id);
                    table.ForeignKey(
                        name: "FK_physical_slots_drawers_drawer_id",
                        column: x => x.drawer_id,
                        principalTable: "drawers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    actor_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    action = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    entity_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    entity_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    role_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coordinate_calibration_history",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    coordinate_point_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    previous_x_um = table.Column<long>(type: "INTEGER", nullable: true),
                    previous_y_um = table.Column<long>(type: "INTEGER", nullable: true),
                    new_x_um = table.Column<long>(type: "INTEGER", nullable: true),
                    new_y_um = table.Column<long>(type: "INTEGER", nullable: true),
                    safe_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    aspirate_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    dispense_z_um = table.Column<long>(type: "INTEGER", nullable: true),
                    reason = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    calibrated_by_user_id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinate_calibration_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_coordinate_calibration_history_coordinate_points_coordinate_point_id",
                        column: x => x.coordinate_point_id,
                        principalTable: "coordinate_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coordinate_calibration_history_users_calibrated_by_user_id",
                        column: x => x.calibrated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_actor_user_id",
                table: "audit_logs",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_created_at_utc",
                table: "audit_logs",
                column: "created_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_calibration_history_calibrated_by_user_id",
                table: "coordinate_calibration_history",
                column: "calibrated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_calibration_history_coordinate_point_id",
                table: "coordinate_calibration_history",
                column: "coordinate_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_points_coordinate_profile_id_point_code",
                table: "coordinate_points",
                columns: new[] { "coordinate_profile_id", "point_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinate_profiles_code",
                table: "coordinate_profiles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dab_mix_positions_code",
                table: "dab_mix_positions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dab_mix_positions_position_no",
                table: "dab_mix_positions",
                column: "position_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_profiles_code",
                table: "device_profiles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_drawers_code",
                table: "drawers",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_drawers_heat_board_id",
                table: "drawers",
                column: "heat_board_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_physical_slots_code",
                table: "physical_slots",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_physical_slots_drawer_id_heat_point_id",
                table: "physical_slots",
                columns: new[] { "drawer_id", "heat_point_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_physical_slots_drawer_id_slot_no",
                table: "physical_slots",
                columns: new[] { "drawer_id", "slot_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reagent_rack_positions_code",
                table: "reagent_rack_positions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reagent_rack_positions_column_no_row_no",
                table: "reagent_rack_positions",
                columns: new[] { "column_no", "row_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reagent_rack_positions_position_no",
                table: "reagent_rack_positions",
                column: "position_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_code",
                table: "roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id_role_id",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wash_positions_code",
                table: "wash_positions",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "coordinate_calibration_history");

            migrationBuilder.DropTable(
                name: "dab_mix_positions");

            migrationBuilder.DropTable(
                name: "device_profiles");

            migrationBuilder.DropTable(
                name: "physical_slots");

            migrationBuilder.DropTable(
                name: "reagent_rack_positions");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "wash_positions");

            migrationBuilder.DropTable(
                name: "coordinate_points");

            migrationBuilder.DropTable(
                name: "drawers");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "coordinate_profiles");
        }
    }
}
