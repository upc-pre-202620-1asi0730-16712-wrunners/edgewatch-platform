using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentProcessMonitoringAndFaultDiagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "controllers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    hvof_system_id = table.Column<int>(type: "int", nullable: false),
                    controller_number = table.Column<int>(type: "int", nullable: false),
                    device_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    serial_number = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    manufacturer = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    model = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    part_number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    ip_address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    port = table.Column<int>(type: "int", nullable: false),
                    rack = table.Column<int>(type: "int", nullable: false),
                    slot = table.Column<int>(type: "int", nullable: false),
                    endpoint_url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_controllers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_controllers_hvof_systems_hvof_system_id",
                        column: x => x.hvof_system_id,
                        principalTable: "hvof_systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fault_cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organization_id = table.Column<int>(type: "int", nullable: false),
                    spray_session_id = table.Column<int>(type: "int", nullable: false),
                    hvof_system_id = table.Column<int>(type: "int", nullable: false),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    band = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    value = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    epoch_millis = table.Column<long>(type: "bigint", nullable: false),
                    probable_cause = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    recommended_action = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    acknowledged_by = table.Column<int>(type: "int", nullable: true),
                    acknowledged_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    resolved_by = table.Column<int>(type: "int", nullable: true),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    resolution_notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_fault_cases", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fault_rules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organization_id = table.Column<int>(type: "int", nullable: false),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    band = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    probable_cause = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    recommended_action = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    cooldown_seconds = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_fault_rules", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hvof_subsystems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    hvof_system_id = table.Column<int>(type: "int", nullable: false),
                    subsystem_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    alias = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_hvof_subsystems", x => x.id);
                    table.ForeignKey(
                        name: "f_k_hvof_subsystems_hvof_systems_hvof_system_id",
                        column: x => x.hvof_system_id,
                        principalTable: "hvof_systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    hvof_system_id = table.Column<int>(type: "int", nullable: false),
                    recipe_number = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    powder_specification = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_recipes", x => x.id);
                    table.ForeignKey(
                        name: "f_k_recipes_hvof_systems_hvof_system_id",
                        column: x => x.hvof_system_id,
                        principalTable: "hvof_systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "spray_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organization_id = table.Column<int>(type: "int", nullable: false),
                    hvof_system_id = table.Column<int>(type: "int", nullable: false),
                    recuperation_id = table.Column<int>(type: "int", nullable: false),
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    recipe_id = table.Column<int>(type: "int", nullable: false),
                    recipe_number = table.Column<int>(type: "int", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    time_zone = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    abort_reason = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_spray_sessions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "controller_supported_protocols",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    protocol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    controller_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_controller_supported_protocols", x => x.id);
                    table.ForeignKey(
                        name: "f_k_controller_supported_protocols_controllers_controller_id",
                        column: x => x.controller_id,
                        principalTable: "controllers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hvof_parts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    hvof_subsystem_id = table.Column<int>(type: "int", nullable: false),
                    part_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    serial_number = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    manufacturer = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_hvof_parts", x => x.id);
                    table.ForeignKey(
                        name: "f_k_hvof_parts__hvof_subsystem_hvof_subsystem_id",
                        column: x => x.hvof_subsystem_id,
                        principalTable: "hvof_subsystems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hvof_subsystem_parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    hvof_subsystem_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_hvof_subsystem_parameters", x => x.id);
                    table.ForeignKey(
                        name: "f_k_hvof_subsystem_parameters_hvof_subsystems_hvof_subsystem_id",
                        column: x => x.hvof_subsystem_id,
                        principalTable: "hvof_subsystems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recipe_applicabilities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    component_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    machine_model = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    position = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    recipe_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_recipe_applicabilities", x => x.id);
                    table.ForeignKey(
                        name: "f_k_recipe_applicabilities_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recipe_parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    recipe_id = table.Column<int>(type: "int", nullable: false),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    setpoint = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    lower_shutdown = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    lower_warning = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    nominal_min = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    nominal_max = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    upper_warning = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    upper_shutdown = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    unit_symbol = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    unit_category = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_recipe_parameters", x => x.id);
                    table.ForeignKey(
                        name: "f_k_recipe_parameters_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "process_readings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    spray_session_id = table.Column<int>(type: "int", nullable: false),
                    epoch_millis = table.Column<long>(type: "bigint", nullable: false),
                    plc_clock_offset_millis = table.Column<long>(type: "bigint", nullable: false),
                    tag_path = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    subsystem_id = table.Column<int>(type: "int", nullable: true),
                    part_id = table.Column<int>(type: "int", nullable: true),
                    value = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    unit_symbol = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    unit_category = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    band = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    derived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    mapping_pending = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_process_readings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_process_readings_spray_sessions_spray_session_id",
                        column: x => x.spray_session_id,
                        principalTable: "spray_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "spray_session_recipe_bands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parameter = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    setpoint = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    lower_shutdown = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    lower_warning = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    nominal_min = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    nominal_max = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    upper_warning = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    upper_shutdown = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                    unit_symbol = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    unit_category = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    spray_session_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_spray_session_recipe_bands", x => x.id);
                    table.ForeignKey(
                        name: "f_k_spray_session_recipe_bands_spray_sessions_spray_session_id",
                        column: x => x.spray_session_id,
                        principalTable: "spray_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_controller_supported_protocols_controller_id",
                table: "controller_supported_protocols",
                column: "controller_id");

            migrationBuilder.CreateIndex(
                name: "i_x_controllers_hvof_system_id_controller_number",
                table: "controllers",
                columns: new[] { "hvof_system_id", "controller_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_fault_cases_organization_id_status",
                table: "fault_cases",
                columns: new[] { "organization_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_fault_cases_spray_session_id_parameter_band_epoch_millis",
                table: "fault_cases",
                columns: new[] { "spray_session_id", "parameter", "band", "epoch_millis" });

            migrationBuilder.CreateIndex(
                name: "i_x_fault_rules_organization_id_parameter_band",
                table: "fault_rules",
                columns: new[] { "organization_id", "parameter", "band" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_hvof_parts_hvof_subsystem_id_serial_number",
                table: "hvof_parts",
                columns: new[] { "hvof_subsystem_id", "serial_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_hvof_subsystem_parameters_hvof_subsystem_id",
                table: "hvof_subsystem_parameters",
                column: "hvof_subsystem_id");

            migrationBuilder.CreateIndex(
                name: "i_x_hvof_subsystems_hvof_system_id_alias",
                table: "hvof_subsystems",
                columns: new[] { "hvof_system_id", "alias" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_process_readings_spray_session_id_band",
                table: "process_readings",
                columns: new[] { "spray_session_id", "band" });

            migrationBuilder.CreateIndex(
                name: "i_x_process_readings_spray_session_id_epoch_millis",
                table: "process_readings",
                columns: new[] { "spray_session_id", "epoch_millis" });

            migrationBuilder.CreateIndex(
                name: "i_x_recipe_applicabilities_recipe_id",
                table: "recipe_applicabilities",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "i_x_recipe_parameters_recipe_id_parameter",
                table: "recipe_parameters",
                columns: new[] { "recipe_id", "parameter" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_recipes_hvof_system_id_recipe_number_status",
                table: "recipes",
                columns: new[] { "hvof_system_id", "recipe_number", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_spray_session_recipe_bands_spray_session_id",
                table: "spray_session_recipe_bands",
                column: "spray_session_id");

            migrationBuilder.CreateIndex(
                name: "i_x_spray_sessions_organization_id_status",
                table: "spray_sessions",
                columns: new[] { "organization_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_spray_sessions_recuperation_id",
                table: "spray_sessions",
                column: "recuperation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "controller_supported_protocols");

            migrationBuilder.DropTable(
                name: "fault_cases");

            migrationBuilder.DropTable(
                name: "fault_rules");

            migrationBuilder.DropTable(
                name: "hvof_parts");

            migrationBuilder.DropTable(
                name: "hvof_subsystem_parameters");

            migrationBuilder.DropTable(
                name: "process_readings");

            migrationBuilder.DropTable(
                name: "recipe_applicabilities");

            migrationBuilder.DropTable(
                name: "recipe_parameters");

            migrationBuilder.DropTable(
                name: "spray_session_recipe_bands");

            migrationBuilder.DropTable(
                name: "controllers");

            migrationBuilder.DropTable(
                name: "hvof_subsystems");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "spray_sessions");
        }
    }
}
