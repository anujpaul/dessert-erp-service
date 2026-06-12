using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseTypesAndSites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "Warehouses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseTypeId",
                table: "Warehouses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OperationalSites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsRetailStore = table.Column<bool>(type: "boolean", nullable: false),
                    IsFulfillmentCenter = table.Column<bool>(type: "boolean", nullable: false),
                    IsReturnCenter = table.Column<bool>(type: "boolean", nullable: false),
                    IsWarehouse = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SiteId",
                table: "Warehouses",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_WarehouseTypeId",
                table: "Warehouses",
                column: "WarehouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalSites_OrganizationId_Code",
                table: "OperationalSites",
                columns: new[] { "OrganizationId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTypes_OrganizationId_Name",
                table: "WarehouseTypes",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_OperationalSites_SiteId",
                table: "Warehouses",
                column: "SiteId",
                principalTable: "OperationalSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_WarehouseTypes_WarehouseTypeId",
                table: "Warehouses",
                column: "WarehouseTypeId",
                principalTable: "WarehouseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_OperationalSites_SiteId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_WarehouseTypes_WarehouseTypeId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "OperationalSites");

            migrationBuilder.DropTable(
                name: "WarehouseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_SiteId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_WarehouseTypeId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseTypeId",
                table: "Warehouses");
        }
    }
}
