using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExportTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedAt",
                table: "vendors",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExported",
                table: "vendors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedAt",
                table: "sales_orders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExported",
                table: "sales_orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedAt",
                table: "purchase_orders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExported",
                table: "purchase_orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedAt",
                table: "catalog_products",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExported",
                table: "catalog_products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "export_job_rows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchJobConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityRef = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BlobName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExportedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_export_job_rows", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_export_job_rows_BatchJobConfigId_ExportedAt",
                table: "export_job_rows",
                columns: new[] { "BatchJobConfigId", "ExportedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_export_job_rows_EntityType_EntityId",
                table: "export_job_rows",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_export_job_rows_OrganizationId",
                table: "export_job_rows",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "export_job_rows");

            migrationBuilder.DropColumn(
                name: "ExportedAt",
                table: "vendors");

            migrationBuilder.DropColumn(
                name: "IsExported",
                table: "vendors");

            migrationBuilder.DropColumn(
                name: "ExportedAt",
                table: "sales_orders");

            migrationBuilder.DropColumn(
                name: "IsExported",
                table: "sales_orders");

            migrationBuilder.DropColumn(
                name: "ExportedAt",
                table: "purchase_orders");

            migrationBuilder.DropColumn(
                name: "IsExported",
                table: "purchase_orders");

            migrationBuilder.DropColumn(
                name: "ExportedAt",
                table: "catalog_products");

            migrationBuilder.DropColumn(
                name: "IsExported",
                table: "catalog_products");
        }
    }
}
