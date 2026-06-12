using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRetailTransactionStaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "retail_transaction_staging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceFile = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SourceHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RawXml = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StoreCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransactionNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OperatorId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    IsReturn = table.Column<bool>(type: "boolean", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    DiscountTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TaxTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ValidationMessage = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    PromotedTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    RetailStatementId = table.Column<Guid>(type: "uuid", nullable: true),
                    ValidatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PromotedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retail_transaction_staging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "retail_transaction_staging_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RetailTransactionStagingId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PosItemId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    LineSubTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsReturn = table.Column<bool>(type: "boolean", nullable: false),
                    MatchedProductVariantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ValidationMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retail_transaction_staging_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_retail_transaction_staging_lines_retail_transaction_staging~",
                        column: x => x.RetailTransactionStagingId,
                        principalTable: "retail_transaction_staging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "retail_transaction_staging_tenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RetailTransactionStagingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Reference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retail_transaction_staging_tenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_retail_transaction_staging_tenders_retail_transaction_stagi~",
                        column: x => x.RetailTransactionStagingId,
                        principalTable: "retail_transaction_staging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_retail_transaction_staging_OrganizationId_SourceHash",
                table: "retail_transaction_staging",
                columns: new[] { "OrganizationId", "SourceHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_retail_transaction_staging_OrganizationId_Status_CreatedAt",
                table: "retail_transaction_staging",
                columns: new[] { "OrganizationId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_retail_transaction_staging_OrganizationId_TransactionNumber",
                table: "retail_transaction_staging",
                columns: new[] { "OrganizationId", "TransactionNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_retail_transaction_staging_lines_RetailTransactionStagingId~",
                table: "retail_transaction_staging_lines",
                columns: new[] { "RetailTransactionStagingId", "LineNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_retail_transaction_staging_tenders_RetailTransactionStaging~",
                table: "retail_transaction_staging_tenders",
                columns: new[] { "RetailTransactionStagingId", "Sequence" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "retail_transaction_staging_lines");

            migrationBuilder.DropTable(
                name: "retail_transaction_staging_tenders");

            migrationBuilder.DropTable(
                name: "retail_transaction_staging");
        }
    }
}
