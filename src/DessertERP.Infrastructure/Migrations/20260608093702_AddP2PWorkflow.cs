using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddP2PWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "purchase_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowInstanceId",
                table: "purchase_orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowInstanceId",
                table: "ap_invoices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "payment_proposals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposalNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ProposalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BankAccount = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProcessedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_proposals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "purchase_requisitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisitionNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DepartmentCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CostCenterCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NeededByDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WorkflowInstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConvertedToPOId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConvertedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_requisitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vendor_credit_notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditNoteNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    APInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreditDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    VendorCNRef = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor_credit_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vendor_credit_notes_ap_invoices_APInvoiceId",
                        column: x => x.APInvoiceId,
                        principalTable: "ap_invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_vendor_credit_notes_vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payment_proposal_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposalId = table.Column<Guid>(type: "uuid", nullable: false),
                    APInvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProposedAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    InvoiceDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    APPaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_proposal_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payment_proposal_lines_ap_invoices_APInvoiceId",
                        column: x => x.APInvoiceId,
                        principalTable: "ap_invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_proposal_lines_ap_payments_APPaymentId",
                        column: x => x.APPaymentId,
                        principalTable: "ap_payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payment_proposal_lines_payment_proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "payment_proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchase_requisition_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstimatedUnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    SuggestedVendorId = table.Column<Guid>(type: "uuid", nullable: true),
                    GlAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_requisition_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_requisition_lines_purchase_requisitions_Requisitio~",
                        column: x => x.RequisitionId,
                        principalTable: "purchase_requisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payment_proposal_lines_APInvoiceId",
                table: "payment_proposal_lines",
                column: "APInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_proposal_lines_APPaymentId",
                table: "payment_proposal_lines",
                column: "APPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_proposal_lines_ProposalId",
                table: "payment_proposal_lines",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_proposals_OrganizationId_ProposalNumber",
                table: "payment_proposals",
                columns: new[] { "OrganizationId", "ProposalNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requisition_lines_RequisitionId",
                table: "purchase_requisition_lines",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requisitions_OrganizationId_RequisitionNumber",
                table: "purchase_requisitions",
                columns: new[] { "OrganizationId", "RequisitionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vendor_credit_notes_APInvoiceId",
                table: "vendor_credit_notes",
                column: "APInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_credit_notes_OrganizationId_CreditNoteNumber",
                table: "vendor_credit_notes",
                columns: new[] { "OrganizationId", "CreditNoteNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vendor_credit_notes_VendorId",
                table: "vendor_credit_notes",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_proposal_lines");

            migrationBuilder.DropTable(
                name: "purchase_requisition_lines");

            migrationBuilder.DropTable(
                name: "vendor_credit_notes");

            migrationBuilder.DropTable(
                name: "payment_proposals");

            migrationBuilder.DropTable(
                name: "purchase_requisitions");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "purchase_orders");

            migrationBuilder.DropColumn(
                name: "WorkflowInstanceId",
                table: "purchase_orders");

            migrationBuilder.DropColumn(
                name: "WorkflowInstanceId",
                table: "ap_invoices");
        }
    }
}
