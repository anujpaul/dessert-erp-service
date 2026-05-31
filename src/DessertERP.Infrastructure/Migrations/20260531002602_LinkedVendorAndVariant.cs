using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkedVendorAndVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "purchase_order_lines",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariantId",
                table: "purchase_order_lines",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PreferredVendorId",
                table: "catalog_products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_lines_ProductVariantId",
                table: "purchase_order_lines",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_products_PreferredVendorId",
                table: "catalog_products",
                column: "PreferredVendorId");

            // Wipe all AP test data in FK-safe order so the new FK constraints can be applied cleanly
            migrationBuilder.Sql("DELETE FROM ap_payments;");
            migrationBuilder.Sql("DELETE FROM ap_invoices;");
            migrationBuilder.Sql("DELETE FROM purchase_order_lines;");
            migrationBuilder.Sql("DELETE FROM purchase_orders;");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_products_vendors_PreferredVendorId",
                table: "catalog_products",
                column: "PreferredVendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_lines_product_variants_ProductVariantId",
                table: "purchase_order_lines",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_products_vendors_PreferredVendorId",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_lines_product_variants_ProductVariantId",
                table: "purchase_order_lines");

            migrationBuilder.DropIndex(
                name: "IX_purchase_order_lines_ProductVariantId",
                table: "purchase_order_lines");

            migrationBuilder.DropIndex(
                name: "IX_catalog_products_PreferredVendorId",
                table: "catalog_products");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "purchase_order_lines");

            migrationBuilder.DropColumn(
                name: "PreferredVendorId",
                table: "catalog_products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "purchase_order_lines",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);
        }
    }
}
