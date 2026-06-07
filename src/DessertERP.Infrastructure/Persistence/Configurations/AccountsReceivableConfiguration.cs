using DessertERP.Domain.Modules.AccountsReceivable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> b)
    {
        b.ToTable("customers");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.CustomerNumber).HasMaxLength(20).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Phone).HasMaxLength(50);
        b.Property(e => e.Address).HasMaxLength(500);
        b.Property(e => e.BillingAddress).HasMaxLength(500);
        b.Property(e => e.ShippingAddress).HasMaxLength(500);
        b.Property(e => e.Website).HasMaxLength(200);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.CreditLimit).HasColumnType("numeric(18,4)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.CustomerNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}


public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> b)
    {
        b.ToTable("sales_orders");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.OrderNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.CustomerRef).HasMaxLength(100);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
        b.HasMany(e => e.Lines).WithOne(l => l.SalesOrder)
            .HasForeignKey(l => l.SalesOrderId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.OrderNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> b)
    {
        b.ToTable("sales_order_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.Sku).HasMaxLength(60);
        b.Property(e => e.ProductName).HasMaxLength(200);
        b.Property(e => e.VariantDescription).HasMaxLength(200);
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.Quantity).HasColumnType("numeric(18,4)");
        b.Property(e => e.UnitPrice).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountPct).HasColumnType("numeric(8,4)");
        b.Property(e => e.TaxRate).HasColumnType("numeric(8,4)");
        b.HasOne(e => e.ProductVariant).WithMany()
            .HasForeignKey(e => e.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Ignore(e => e.LineSubTotal);
        b.Ignore(e => e.DiscountAmount);
        b.Ignore(e => e.TaxableAmount);
        b.Ignore(e => e.TaxAmount);
        b.Ignore(e => e.LineTotal);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ARInvoiceConfiguration : IEntityTypeConfiguration<ARInvoice>
{
    public void Configure(EntityTypeBuilder<ARInvoice> b)
    {
        b.ToTable("ar_invoices");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.InvoiceNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.TotalAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.PaidAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Ignore(e => e.OutstandingAmount);
        b.Ignore(e => e.DaysOutstanding);
        b.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
        b.HasOne(e => e.SalesOrder).WithMany().HasForeignKey(e => e.SalesOrderId);
        b.HasIndex(e => new { e.OrganizationId, e.InvoiceNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class ARPaymentConfiguration : IEntityTypeConfiguration<ARPayment>
{
    public void Configure(EntityTypeBuilder<ARPayment> b)
    {
        b.ToTable("ar_payments");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.PaymentNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)");
        b.Property(e => e.PaymentMethod).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Reference).HasMaxLength(100);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
        b.HasOne(e => e.ARInvoice).WithMany().HasForeignKey(e => e.ARInvoiceId);
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> b)
    {
        b.ToTable("customer_addresses");
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
        b.HasOne(e => e.Customer).WithMany(c => c.Addresses)
            .HasForeignKey(e => e.CustomerId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class CustomerContactConfiguration : IEntityTypeConfiguration<CustomerContact>
{
    public void Configure(EntityTypeBuilder<CustomerContact> b)
    {
        b.ToTable("customer_contacts");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Title).HasMaxLength(150);
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Phone).HasMaxLength(50);
        b.Property(e => e.Mobile).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.HasOne(e => e.Customer).WithMany(c => c.Contacts)
            .HasForeignKey(e => e.CustomerId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
