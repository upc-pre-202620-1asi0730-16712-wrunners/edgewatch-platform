using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report_templates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organization_id = table.Column<int>(type: "int", nullable: false),
                    owner_user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    report_kind = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    primary_color = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    secondary_color = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    header_text = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    footer_text = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    page_size = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    orientation = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    show_signature_block = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    default_for_kind = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    status = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_report_templates", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "generated_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    report_template_id = table.Column<int>(type: "int", nullable: false),
                    organization_id = table.Column<int>(type: "int", nullable: false),
                    generated_by_user_id = table.Column<int>(type: "int", nullable: false),
                    subject_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    format = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    file_name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    period_from = table.Column<DateTime>(type: "date", nullable: true),
                    period_to = table.Column<DateTime>(type: "date", nullable: true),
                    generated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_generated_reports", x => x.id);
                    table.ForeignKey(
                        name: "f_k_generated_reports__report_template_report_template_id",
                        column: x => x.report_template_id,
                        principalTable: "report_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "report_sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    report_template_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    section_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_report_sections", x => x.id);
                    table.ForeignKey(
                        name: "f_k_report_sections_report_templates_report_template_id",
                        column: x => x.report_template_id,
                        principalTable: "report_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "report_widgets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    position = table.Column<int>(type: "int", nullable: false),
                    variable_source = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    variable_code = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    variable_label = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    view_type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    aggregation = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    width = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    report_section_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_report_widgets", x => x.id);
                    table.ForeignKey(
                        name: "f_k_report_widgets_report_sections_report_section_id",
                        column: x => x.report_section_id,
                        principalTable: "report_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_generated_reports_organization_id_subject_type_subject_id",
                table: "generated_reports",
                columns: new[] { "organization_id", "subject_type", "subject_id" });

            migrationBuilder.CreateIndex(
                name: "i_x_generated_reports_report_template_id",
                table: "generated_reports",
                column: "report_template_id");

            migrationBuilder.CreateIndex(
                name: "i_x_report_sections_report_template_id_section_order",
                table: "report_sections",
                columns: new[] { "report_template_id", "section_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_report_templates_organization_id_name",
                table: "report_templates",
                columns: new[] { "organization_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_report_templates_organization_id_report_kind_status",
                table: "report_templates",
                columns: new[] { "organization_id", "report_kind", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_report_widgets_report_section_id",
                table: "report_widgets",
                column: "report_section_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generated_reports");

            migrationBuilder.DropTable(
                name: "report_widgets");

            migrationBuilder.DropTable(
                name: "report_sections");

            migrationBuilder.DropTable(
                name: "report_templates");
        }
    }
}
