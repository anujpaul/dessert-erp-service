using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRetailStatementPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RetailStatementId",
                table: "pos_transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "retail_statements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatementNumber = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransactionCount = table.Column<int>(type: "integer", nullable: false),
                    NetSales = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    DiscountTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TaxTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CostTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ARInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ARCreditNoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    PostedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PostingError = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retail_statements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "retail_tender_settlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RetailStatementId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProcessorReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BankTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    SettledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retail_tender_settlements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pos_transactions_OrganizationId_ExternalRef",
                table: "pos_transactions",
                columns: new[] { "OrganizationId", "ExternalRef" },
                unique: true,
                filter: "\"ExternalRef\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_pos_transactions_RetailStatementId",
                table: "pos_transactions",
                column: "RetailStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_retail_statements_OrganizationId_StatementNumber",
                table: "retail_statements",
                columns: new[] { "OrganizationId", "StatementNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_retail_statements_OrganizationId_StoreId_BusinessDate_Curre~",
                table: "retail_statements",
                columns: new[] { "OrganizationId", "StoreId", "BusinessDate", "Currency", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_retail_tender_settlements_OrganizationId_Status",
                table: "retail_tender_settlements",
                columns: new[] { "OrganizationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_retail_tender_settlements_RetailStatementId",
                table: "retail_tender_settlements",
                column: "RetailStatementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "retail_statements");

            migrationBuilder.DropTable(
                name: "retail_tender_settlements");

            migrationBuilder.DropIndex(
                name: "IX_pos_transactions_OrganizationId_ExternalRef",
                table: "pos_transactions");

            migrationBuilder.DropIndex(
                name: "IX_pos_transactions_RetailStatementId",
                table: "pos_transactions");

            migrationBuilder.DropColumn(
                name: "RetailStatementId",
                table: "pos_transactions");
        }
    }
}
