using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleFiscalCalendars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_fiscal_years_organization_id_name",
                table: "fiscal_years");

            migrationBuilder.AddColumn<Guid>(
                name: "fiscal_calendar_id",
                table: "fiscal_years",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "fiscal_calendars",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    calendar_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fiscal_calendars", x => x.id);
                });

            migrationBuilder.Sql(
                """
                INSERT INTO fiscal_calendars
                    (id, organization_id, name, description, calendar_type, is_default,
                     created_at, updated_at, is_deleted)
                SELECT DISTINCT ON (organization_id)
                    id,
                    organization_id,
                    'Corporate Calendar',
                    'Default calendar created for existing fiscal years',
                    COALESCE(NULLIF(calendar_type, ''), 'Monthly'),
                    TRUE,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP,
                    FALSE
                FROM fiscal_years
                ORDER BY organization_id, start_date, id;

                UPDATE fiscal_years AS fy
                SET fiscal_calendar_id = fc.id
                FROM fiscal_calendars AS fc
                WHERE fc.organization_id = fy.organization_id
                  AND fc.is_default = TRUE
                  AND fc.is_deleted = FALSE;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "fiscal_calendar_id",
                table: "fiscal_years",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_fiscal_years_fiscal_calendar_id_name",
                table: "fiscal_years",
                columns: new[] { "fiscal_calendar_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_fiscal_calendars_organization_id",
                table: "fiscal_calendars",
                column: "organization_id",
                unique: true,
                filter: "is_default = TRUE AND is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "ix_fiscal_calendars_organization_id_name",
                table: "fiscal_calendars",
                columns: new[] { "organization_id", "name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_fiscal_years_fiscal_calendars_fiscal_calendar_id",
                table: "fiscal_years",
                column: "fiscal_calendar_id",
                principalTable: "fiscal_calendars",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_fiscal_years_fiscal_calendars_fiscal_calendar_id",
                table: "fiscal_years");

            migrationBuilder.DropTable(
                name: "fiscal_calendars");

            migrationBuilder.DropIndex(
                name: "ix_fiscal_years_fiscal_calendar_id_name",
                table: "fiscal_years");

            migrationBuilder.DropColumn(
                name: "fiscal_calendar_id",
                table: "fiscal_years");

            migrationBuilder.CreateIndex(
                name: "ix_fiscal_years_organization_id_name",
                table: "fiscal_years",
                columns: new[] { "organization_id", "name" },
                unique: true);
        }
    }
}
