using DessertERP.Domain.Modules.AccountsPayable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> b)
    {
        b.ToTable("vendors");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.VendorNumber).HasMaxLength(20).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Phone).HasMaxLength(50);
        b.Property(e => e.Address).HasMaxLength(500);
        b.Property(e => e.BillingAddress).HasMaxLength(500);
        b.Property(e => e.ShippingAddress).HasMaxLength(500);
        b.Property(e => e.Website).HasMaxLength(200);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.TaxId).HasMaxLength(50);
        b.Property(e => e.BankAccountName).HasMaxLength(200);
        b.Property(e => e.BankAccountNumber).HasMaxLength(50);
        b.Property(e => e.BankRoutingNumber).HasMaxLength(50);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.VendorNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> b)
    {
        b.ToTable("purchase_orders");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.PONumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.InvoiceStatus).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.InvoicedAmount).HasColumnType("numeric(18,4)");
        b.Ignore(e => e.CanReceive);
        b.HasOne(e => e.Vendor).WithMany().HasForeignKey(e => e.VendorId);
        b.HasMany(e => e.Lines).WithOne(l => l.PurchaseOrder)
            .HasForeignKey(l => l.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Receipts).WithOne(r => r.PurchaseOrder)
            .HasForeignKey(r => r.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.PONumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class PurchaseOrderReceiptConfiguration : IEntityTypeConfiguration<PurchaseOrderReceipt>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceipt> b)
    {
        b.ToTable("purchase_order_receipts");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.PurchaseOrderId).IsRequired();
        b.Property(e => e.ReceiptNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Notes).HasMaxLength(500);
        b.HasMany(e => e.Lines).WithOne(l => l.Receipt)
            .HasForeignKey(l => l.ReceiptId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => e.ReceiptNumber).IsUnique();
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class PurchaseOrderReceiptLineConfiguration : IEntityTypeConfiguration<PurchaseOrderReceiptLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceiptLine> b)
    {
        b.ToTable("purchase_order_receipt_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.Qty).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.PurchaseOrderLine).WithMany()
            .HasForeignKey(e => e.PurchaseOrderLineId).OnDelete(DeleteBehavior.Restrict);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> b)
    {
        b.ToTable("purchase_order_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.ProductVariantId).IsRequired();
        b.Property(e => e.ProductCode).HasMaxLength(60);
        b.Property(e => e.Description).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.OrderedQty).HasColumnType("numeric(18,4)");
        b.Property(e => e.ReceivedQty).HasColumnType("numeric(18,4)");
        b.Property(e => e.UnitCost).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxRate).HasColumnType("numeric(8,4)");
        b.Ignore(e => e.LineTotal);
        b.Ignore(e => e.ReceivedTotal);
        b.Ignore(e => e.IsFullyReceived);
        // FK to product_variants — restrict delete (can't delete a variant used on a PO)
        b.HasOne<DessertERP.Domain.Modules.ProductManagement.ProductVariant>()
            .WithMany()
            .HasForeignKey(e => e.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class APInvoiceConfiguration : IEntityTypeConfiguration<APInvoice>
{
    public void Configure(EntityTypeBuilder<APInvoice> b)
    {
        b.ToTable("ap_invoices");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.InvoiceNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.VendorInvoiceRef).HasMaxLength(100);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.TotalAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.PaidAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.PrepaymentApplied).HasColumnType("numeric(18,4)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.InvoiceType).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.MatchStatus).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.MatchNotes).HasMaxLength(2000);
        b.Property(e => e.BypassReason).HasMaxLength(500);
        b.Ignore(e => e.OutstandingAmount);
        b.Ignore(e => e.DaysOutstanding);
        b.Ignore(e => e.IsSubmittedForApproval);
        b.HasOne(e => e.Vendor).WithMany().HasForeignKey(e => e.VendorId);
        b.HasOne(e => e.PurchaseOrder).WithMany().HasForeignKey(e => e.PurchaseOrderId);
        b.HasIndex(e => new { e.OrganizationId, e.InvoiceNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class APPaymentConfiguration : IEntityTypeConfiguration<APPayment>
{
    public void Configure(EntityTypeBuilder<APPayment> b)
    {
        b.ToTable("ap_payments");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.PaymentNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)");
        b.Property(e => e.PaymentMethod).HasMaxLength(30);
        b.Property(e => e.Reference).HasMaxLength(100);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasOne(e => e.Vendor).WithMany().HasForeignKey(e => e.VendorId);
        b.HasOne(e => e.APInvoice).WithMany().HasForeignKey(e => e.APInvoiceId);
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class VendorAddressConfiguration : IEntityTypeConfiguration<VendorAddress>
{
    public void Configure(EntityTypeBuilder<VendorAddress> b)
    {
        b.ToTable("vendor_addresses");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Label).HasMaxLength(100).IsRequired();
        b.Property(e => e.AddressType).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Line1).HasMaxLength(300).IsRequired();
        b.Property(e => e.Line2).HasMaxLength(300);
        b.Property(e => e.City).HasMaxLength(100).IsRequired();
        b.Property(e => e.State).HasMaxLength(100);
        b.Property(e => e.PostalCode).HasMaxLength(20);
        b.Property(e => e.Country).HasMaxLength(100);
        b.Ignore(e => e.SingleLine);
        b.HasOne(e => e.Vendor).WithMany(v => v.Addresses)
            .HasForeignKey(e => e.VendorId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class VendorContactConfiguration : IEntityTypeConfiguration<VendorContact>
{
    public void Configure(EntityTypeBuilder<VendorContact> b)
    {
        b.ToTable("vendor_contacts");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Title).HasMaxLength(150);
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Phone).HasMaxLength(50);
        b.Property(e => e.Mobile).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.HasOne(e => e.Vendor).WithMany(v => v.Contacts)
            .HasForeignKey(e => e.VendorId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

// ── P2P additions ─────────────────────────────────────────────────────────────

public class PurchaseRequisitionConfiguration : IEntityTypeConfiguration<PurchaseRequisition>
{
    public void Configure(EntityTypeBuilder<PurchaseRequisition> b)
    {
        b.ToTable("purchase_requisitions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.RequisitionNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.RequestedBy).HasMaxLength(200).IsRequired();
        b.Property(e => e.DepartmentCode).HasMaxLength(50);
        b.Property(e => e.CostCenterCode).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.RejectionReason).HasMaxLength(500);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Ignore(e => e.TotalEstimatedCost);
        b.HasMany(e => e.Lines).WithOne(l => l.Requisition)
            .HasForeignKey(l => l.RequisitionId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.RequisitionNumber }).IsUnique();
    }
}

public class PurchaseRequisitionLineConfiguration : IEntityTypeConfiguration<PurchaseRequisitionLine>
{
    public void Configure(EntityTypeBuilder<PurchaseRequisitionLine> b)
    {
        b.ToTable("purchase_requisition_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.Description).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.GlAccountCode).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.Quantity).HasColumnType("numeric(18,4)");
        b.Property(e => e.EstimatedUnitCost).HasColumnType("numeric(18,4)");
        b.Ignore(e => e.EstimatedTotalCost);
    }
}

public class VendorCreditNoteConfiguration : IEntityTypeConfiguration<VendorCreditNote>
{
    public void Configure(EntityTypeBuilder<VendorCreditNote> b)
    {
        b.ToTable("vendor_credit_notes");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.CreditNoteNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500).IsRequired();
        b.Property(e => e.VendorCNRef).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.TotalAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.AppliedAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Reason).HasConversion<string>().HasMaxLength(30);
        b.Ignore(e => e.AvailableCredit);
        b.HasOne(e => e.Vendor).WithMany().HasForeignKey(e => e.VendorId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.APInvoice).WithMany().HasForeignKey(e => e.APInvoiceId).OnDelete(DeleteBehavior.SetNull);
        b.HasIndex(e => new { e.OrganizationId, e.CreditNoteNumber }).IsUnique();
    }
}

public class PaymentProposalConfiguration : IEntityTypeConfiguration<PaymentProposal>
{
    public void Configure(EntityTypeBuilder<PaymentProposal> b)
    {
        b.ToTable("payment_proposals");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.ProposalNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.PaymentMethod).HasMaxLength(50);
        b.Property(e => e.BankAccount).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.ProcessedBy).HasMaxLength(200);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Ignore(e => e.TotalAmount);
        b.HasMany(e => e.Lines).WithOne(l => l.Proposal)
            .HasForeignKey(l => l.ProposalId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.ProposalNumber }).IsUnique();
    }
}

public class PaymentProposalLineConfiguration : IEntityTypeConfiguration<PaymentProposalLine>
{
    public void Configure(EntityTypeBuilder<PaymentProposalLine> b)
    {
        b.ToTable("payment_proposal_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.InvoiceNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.VendorName).HasMaxLength(200).IsRequired();
        b.Property(e => e.ProposedAmount).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.APInvoice).WithMany()
            .HasForeignKey(e => e.APInvoiceId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.APPayment).WithMany()
            .HasForeignKey(e => e.APPaymentId).OnDelete(DeleteBehavior.SetNull);
    }
}
