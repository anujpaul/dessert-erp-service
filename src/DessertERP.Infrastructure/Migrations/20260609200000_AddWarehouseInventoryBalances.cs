using DessertERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260609200000_AddWarehouseInventoryBalances")]
public partial class AddWarehouseInventoryBalances : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "WarehouseInventoryBalances",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                QuantityOnHand = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                QuantityReserved = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WarehouseInventoryBalances", x => x.Id);
                table.ForeignKey(
                    name: "FK_WarehouseInventoryBalances_WarehouseLocations_WarehouseLocationId",
                    column: x => x.WarehouseLocationId,
                    principalTable: "WarehouseLocations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_WarehouseInventoryBalances_Warehouses_WarehouseId",
                    column: x => x.WarehouseId,
                    principalTable: "Warehouses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_WarehouseInventoryBalances_product_variants_ProductVariantId",
                    column: x => x.ProductVariantId,
                    principalTable: "product_variants",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_WarehouseInventoryBalances_ProductVariantId",
            table: "WarehouseInventoryBalances",
            column: "ProductVariantId");

        migrationBuilder.CreateIndex(
            name: "IX_WarehouseInventoryBalances_WarehouseId",
            table: "WarehouseInventoryBalances",
            column: "WarehouseId");

        migrationBuilder.CreateIndex(
            name: "IX_WarehouseInventoryBalances_WarehouseLocationId",
            table: "WarehouseInventoryBalances",
            column: "WarehouseLocationId");

        migrationBuilder.CreateIndex(
            name: "IX_WarehouseInventoryBalances_OrganizationId_ProductVariantId_WarehouseId_WarehouseLocationId",
            table: "WarehouseInventoryBalances",
            columns: new[] { "OrganizationId", "ProductVariantId", "WarehouseId", "WarehouseLocationId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "WarehouseInventoryBalances");
    }
}
