using DessertERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260609213000_AddPurchaseReceivingLocations")]
public partial class AddPurchaseReceivingLocations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>("WarehouseId", "purchase_orders", "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>("WarehouseId", "purchase_order_receipts", "uuid", nullable: true);
        migrationBuilder.AddColumn<Guid>("WarehouseLocationId", "purchase_order_receipts", "uuid", nullable: true);

        migrationBuilder.CreateIndex("IX_purchase_orders_WarehouseId", "purchase_orders", "WarehouseId");
        migrationBuilder.CreateIndex("IX_purchase_order_receipts_WarehouseId", "purchase_order_receipts", "WarehouseId");
        migrationBuilder.CreateIndex("IX_purchase_order_receipts_WarehouseLocationId", "purchase_order_receipts", "WarehouseLocationId");

        migrationBuilder.AddForeignKey(
            "FK_purchase_orders_Warehouses_WarehouseId", "purchase_orders", "WarehouseId",
            "Warehouses", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
        migrationBuilder.AddForeignKey(
            "FK_purchase_order_receipts_Warehouses_WarehouseId", "purchase_order_receipts", "WarehouseId",
            "Warehouses", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
        migrationBuilder.AddForeignKey(
            "FK_purchase_order_receipts_WarehouseLocations_WarehouseLocationId",
            "purchase_order_receipts", "WarehouseLocationId",
            "WarehouseLocations", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey("FK_purchase_orders_Warehouses_WarehouseId", "purchase_orders");
        migrationBuilder.DropForeignKey("FK_purchase_order_receipts_Warehouses_WarehouseId", "purchase_order_receipts");
        migrationBuilder.DropForeignKey(
            "FK_purchase_order_receipts_WarehouseLocations_WarehouseLocationId", "purchase_order_receipts");

        migrationBuilder.DropIndex("IX_purchase_orders_WarehouseId", "purchase_orders");
        migrationBuilder.DropIndex("IX_purchase_order_receipts_WarehouseId", "purchase_order_receipts");
        migrationBuilder.DropIndex("IX_purchase_order_receipts_WarehouseLocationId", "purchase_order_receipts");

        migrationBuilder.DropColumn("WarehouseId", "purchase_orders");
        migrationBuilder.DropColumn("WarehouseId", "purchase_order_receipts");
        migrationBuilder.DropColumn("WarehouseLocationId", "purchase_order_receipts");
    }
}
