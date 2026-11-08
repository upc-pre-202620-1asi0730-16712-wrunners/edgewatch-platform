using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddRecuperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "recuperations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    supplier_organization_id = table.Column<int>(type: "int", nullable: false),
                    work_order_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    manufacturing_order_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    component_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    segment = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    operation = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    weight_value = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    weight_unit_symbol = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    hourmeter_at_entry = table.Column<int>(type: "int", nullable: false),
                    powder_supplier = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    powder_lot_number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    powder_chemical_composition = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    received_at = table.Column<DateOnly>(type: "date", nullable: false),
                    completed_at = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_recuperations", x => x.id);
                    table.ForeignKey(
                        name: "f_k_recuperations_components_component_id",
                        column: x => x.component_id,
                        principalTable: "components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_recuperations_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recuperation_linked_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    session_id = table.Column<int>(type: "int", nullable: false),
                    recuperation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_recuperation_linked_sessions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_recuperation_linked_sessions_recuperations_recuperation_id",
                        column: x => x.recuperation_id,
                        principalTable: "recuperations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_recuperation_linked_sessions_recuperation_id_session_id",
                table: "recuperation_linked_sessions",
                columns: new[] { "recuperation_id", "session_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_recuperations_component_id",
                table: "recuperations",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "i_x_recuperations_customer_id",
                table: "recuperations",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "i_x_recuperations_supplier_organization_id_work_order_number",
                table: "recuperations",
                columns: new[] { "supplier_organization_id", "work_order_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recuperation_linked_sessions");

            migrationBuilder.DropTable(
                name: "recuperations");
        }
    }
}
