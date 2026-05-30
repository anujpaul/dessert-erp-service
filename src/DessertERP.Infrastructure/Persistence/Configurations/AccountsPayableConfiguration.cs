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
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.TaxId).HasMaxLength(50);
        b.Property(e => e.BankAccountName).HasMaxLength(200);
        b.Property(e => e.BankAccountNumber).HasMaxLength(50);
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
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.Vendor).WithMany().HasForeignKey(e => e.VendorId);
        b.HasMany(e => e.Lines).WithOne(l => l.PurchaseOrder)
            .HasForeignKey(l => l.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.PONumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
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
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Ignore(e => e.OutstandingAmount);
        b.Ignore(e => e.DaysOutstanding);
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
