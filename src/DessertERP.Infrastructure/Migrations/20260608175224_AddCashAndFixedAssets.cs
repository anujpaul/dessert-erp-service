using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCashAndFixedAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BankName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    BankBranch = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AccountNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IBAN = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: true),
                    SwiftCode = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    GLAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    LastReconciledBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    LastReconciledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fixed_assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AcquisitionCost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PurchaseOrderRef = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Supplier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GLAssetAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    GLAccumulatedDepreciationAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    GLDepreciationExpenseAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepreciationMethod = table.Column<string>(type: "text", nullable: false),
                    UsefulLifeYears = table.Column<int>(type: "integer", nullable: false),
                    SalvageValue = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    LastDepreciationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TotalEstimatedUnits = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    UnitsProducedToDate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bank_reconciliations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReconciliationNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StatementStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StatementEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StatementOpeningBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    StatementClosingBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    SystemOpeningBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ReconciledAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_reconciliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bank_reconciliations_bank_accounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "bank_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bank_transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransactionStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CounterpartyName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ARInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    APInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransferToAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReconciliationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReconciledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PostedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PostedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bank_transactions_bank_accounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "bank_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cash_journals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    JournalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalDebits = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TotalCredits = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PostedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_journals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cash_journals_bank_accounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "bank_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asset_depreciations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    RunningNBV = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PostedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_depreciations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asset_depreciations_fixed_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "fixed_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_disposals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DisposalType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisposalProceeds = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    NetBookValueAtDisposal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    DisposedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BuyerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GLGainLossAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_disposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asset_disposals_fixed_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "fixed_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_maintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MaintenanceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CapitalizeCost = table.Column<bool>(type: "boolean", nullable: false),
                    Vendor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PerformedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NextMaintenanceDue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_maintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asset_maintenances_fixed_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "fixed_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FromLocation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ToLocation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromDepartment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ToDepartment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TransferredBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asset_transfers_fixed_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "fixed_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cash_journal_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalId = table.Column<Guid>(type: "uuid", nullable: false),
                    GLAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Debit = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Credit = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_journal_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cash_journal_lines_cash_journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "cash_journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asset_depreciations_AssetId",
                table: "asset_depreciations",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_asset_disposals_AssetId",
                table: "asset_disposals",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_asset_maintenances_AssetId",
                table: "asset_maintenances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_asset_transfers_AssetId",
                table: "asset_transfers",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_OrganizationId_AccountCode",
                table: "bank_accounts",
                columns: new[] { "OrganizationId", "AccountCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_reconciliations_BankAccountId",
                table: "bank_reconciliations",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_reconciliations_OrganizationId_ReconciliationNumber",
                table: "bank_reconciliations",
                columns: new[] { "OrganizationId", "ReconciliationNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_transactions_BankAccountId_TransactionDate",
                table: "bank_transactions",
                columns: new[] { "BankAccountId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_bank_transactions_OrganizationId_TransactionNumber",
                table: "bank_transactions",
                columns: new[] { "OrganizationId", "TransactionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_transactions_TransactionStatus",
                table: "bank_transactions",
                column: "TransactionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_cash_journal_lines_JournalId",
                table: "cash_journal_lines",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_cash_journals_BankAccountId",
                table: "cash_journals",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_cash_journals_OrganizationId_JournalNumber",
                table: "cash_journals",
                columns: new[] { "OrganizationId", "JournalNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cash_journals_Status",
                table: "cash_journals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_assets_OrganizationId_AssetCode",
                table: "fixed_assets",
                columns: new[] { "OrganizationId", "AssetCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_depreciations");

            migrationBuilder.DropTable(
                name: "asset_disposals");

            migrationBuilder.DropTable(
                name: "asset_maintenances");

            migrationBuilder.DropTable(
                name: "asset_transfers");

            migrationBuilder.DropTable(
                name: "bank_reconciliations");

            migrationBuilder.DropTable(
                name: "bank_transactions");

            migrationBuilder.DropTable(
                name: "cash_journal_lines");

            migrationBuilder.DropTable(
                name: "fixed_assets");

            migrationBuilder.DropTable(
                name: "cash_journals");

            migrationBuilder.DropTable(
                name: "bank_accounts");
        }
    }
}
