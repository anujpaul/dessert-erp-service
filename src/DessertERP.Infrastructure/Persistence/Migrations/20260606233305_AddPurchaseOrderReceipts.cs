using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderReceipts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APInvoiceId",
                table: "purchase_orders");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceStatus",
                table: "purchase_orders",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InvoicedAmount",
                table: "purchase_orders",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "purchase_order_receipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_order_receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_order_receipts_purchase_orders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "purchase_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchase_order_receipt_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_order_receipt_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_order_receipt_lines_purchase_order_lines_PurchaseO~",
                        column: x => x.PurchaseOrderLineId,
                        principalTable: "purchase_order_lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_order_receipt_lines_purchase_order_receipts_Receip~",
                        column: x => x.ReceiptId,
                        principalTable: "purchase_order_receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_receipt_lines_PurchaseOrderLineId",
                table: "purchase_order_receipt_lines",
                column: "PurchaseOrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_receipt_lines_ReceiptId",
                table: "purchase_order_receipt_lines",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_receipts_PurchaseOrderId",
                table: "purchase_order_receipts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_receipts_ReceiptNumber",
                table: "purchase_order_receipts",
                column: "ReceiptNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_order_receipt_lines");

            migrationBuilder.DropTable(
                name: "purchase_order_receipts");

            migrationBuilder.DropColumn(
                name: "InvoiceStatus",
                table: "purchase_orders");

            migrationBuilder.DropColumn(
                name: "InvoicedAmount",
                table: "purchase_orders");

            migrationBuilder.AddColumn<Guid>(
                name: "APInvoiceId",
                table: "purchase_orders",
                type: "uuid",
                nullable: true);
        }
    }
}
