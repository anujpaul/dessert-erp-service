using DessertERP.Domain.Modules.Retail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class RetailStoreConfiguration : IEntityTypeConfiguration<RetailStore>
{
    public void Configure(EntityTypeBuilder<RetailStore> b)
    {
        b.ToTable("retail_stores");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.StoreCode).HasMaxLength(20).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Address).HasMaxLength(500);
        b.Property(e => e.Phone).HasMaxLength(50);
        b.Property(e => e.ManagerName).HasMaxLength(200);
        b.HasIndex(e => new { e.OrganizationId, e.StoreCode }).IsUnique();
    }
}

public class POSTransactionConfiguration : IEntityTypeConfiguration<POSTransaction>
{
    public void Configure(EntityTypeBuilder<POSTransaction> b)
    {
        b.ToTable("pos_transactions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.StoreId).IsRequired();
        b.Property(e => e.TransactionNumber).HasMaxLength(50).IsRequired();
        b.Property(e => e.ExternalRef).HasMaxLength(100);
        b.Property(e => e.CashierId).HasMaxLength(100);
        b.Property(e => e.CashierName).HasMaxLength(200);
        b.Property(e => e.TransactionType).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Channel).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.FulfillmentStatus).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.CustomerName).HasMaxLength(200);
        b.Property(e => e.CustomerEmail).HasMaxLength(200);
        b.Property(e => e.CustomerPhone).HasMaxLength(50);
        b.Property(e => e.DeliveryAddress).HasMaxLength(500);
        b.Property(e => e.ExternalOrderRef).HasMaxLength(100);
        b.Property(e => e.ChannelNotes).HasMaxLength(1000);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TenderedAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.ChangeAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.CouponCode).HasMaxLength(50);
        b.Property(e => e.CouponDiscount).HasColumnType("numeric(18,4)");
        b.Property(e => e.ProcessingError).HasMaxLength(1000);
        b.Property(e => e.SourceFile).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.TransactionNumber }).IsUnique();
        b.HasIndex(e => new { e.OrganizationId, e.ExternalRef })
            .IsUnique()
            .HasFilter("\"ExternalRef\" IS NOT NULL");
        b.HasIndex(e => e.RetailStatementId);
        b.HasIndex(e => e.Status);

        b.HasMany(e => e.Lines)
            .WithOne()
            .HasForeignKey(l => l.POSTransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(e => e.Payments)
            .WithOne()
            .HasForeignKey(p => p.POSTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RetailStatementConfiguration : IEntityTypeConfiguration<RetailStatement>
{
    public void Configure(EntityTypeBuilder<RetailStatement> b)
    {
        b.ToTable("retail_statements");
        b.HasKey(e => e.Id);
        b.Property(e => e.StatementNumber).HasMaxLength(60).IsRequired();
        b.Property(e => e.Currency).HasMaxLength(3).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.NetSales).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.CostTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.PostingError).HasMaxLength(2000);
        b.HasIndex(e => new { e.OrganizationId, e.StatementNumber }).IsUnique();
        b.HasIndex(e => new { e.OrganizationId, e.StoreId, e.BusinessDate, e.Currency, e.Status });
    }
}

public class RetailTenderSettlementConfiguration : IEntityTypeConfiguration<RetailTenderSettlement>
{
    public void Configure(EntityTypeBuilder<RetailTenderSettlement> b)
    {
        b.ToTable("retail_tender_settlements");
        b.HasKey(e => e.Id);
        b.Property(e => e.PaymentMethod).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)");
        b.Property(e => e.Currency).HasMaxLength(3).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.ProcessorReference).HasMaxLength(200);
        b.HasIndex(e => e.RetailStatementId);
        b.HasIndex(e => new { e.OrganizationId, e.Status });
    }
}

public class RetailTransactionStagingConfiguration : IEntityTypeConfiguration<RetailTransactionStaging>
{
    public void Configure(EntityTypeBuilder<RetailTransactionStaging> b)
    {
        b.ToTable("retail_transaction_staging");
        b.HasKey(e => e.Id);
        b.Property(e => e.SourceFile).HasMaxLength(500).IsRequired();
        b.Property(e => e.SourceHash).HasMaxLength(64).IsRequired();
        b.Property(e => e.RawXml).HasColumnType("text").IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.StoreCode).HasMaxLength(20).IsRequired();
        b.Property(e => e.TransactionNumber).HasMaxLength(100).IsRequired();
        b.Property(e => e.OperatorId).HasMaxLength(100);
        b.Property(e => e.Currency).HasMaxLength(3).IsRequired();
        b.Property(e => e.SubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.GrandTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.ValidationMessage).HasMaxLength(4000);
        b.HasIndex(e => new { e.OrganizationId, e.SourceHash }).IsUnique();
        b.HasIndex(e => new { e.OrganizationId, e.TransactionNumber });
        b.HasIndex(e => new { e.OrganizationId, e.Status, e.CreatedAt });

        b.HasMany(e => e.Lines).WithOne()
            .HasForeignKey(e => e.RetailTransactionStagingId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Tenders).WithOne()
            .HasForeignKey(e => e.RetailTransactionStagingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RetailTransactionStagingLineConfiguration : IEntityTypeConfiguration<RetailTransactionStagingLine>
{
    public void Configure(EntityTypeBuilder<RetailTransactionStagingLine> b)
    {
        b.ToTable("retail_transaction_staging_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.Sku).HasMaxLength(100).IsRequired();
        b.Property(e => e.PosItemId).HasMaxLength(100);
        b.Property(e => e.ProductName).HasMaxLength(300).IsRequired();
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.Quantity).HasColumnType("numeric(18,4)");
        b.Property(e => e.UnitPrice).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.LineSubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.LineTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.ValidationMessage).HasMaxLength(1000);
        b.HasIndex(e => new { e.RetailTransactionStagingId, e.LineNumber }).IsUnique();
    }
}

public class RetailTransactionStagingTenderConfiguration : IEntityTypeConfiguration<RetailTransactionStagingTender>
{
    public void Configure(EntityTypeBuilder<RetailTransactionStagingTender> b)
    {
        b.ToTable("retail_transaction_staging_tenders");
        b.HasKey(e => e.Id);
        b.Property(e => e.PaymentMethod).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)");
        b.Property(e => e.Reference).HasMaxLength(200);
        b.HasIndex(e => new { e.RetailTransactionStagingId, e.Sequence }).IsUnique();
    }
}

public class POSTransactionLineConfiguration : IEntityTypeConfiguration<POSTransactionLine>
{
    public void Configure(EntityTypeBuilder<POSTransactionLine> b)
    {
        b.ToTable("pos_transaction_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.POSTransactionId).IsRequired();
        b.Property(e => e.Sku).HasMaxLength(100).IsRequired();
        b.Property(e => e.ProductName).HasMaxLength(300).IsRequired();
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.Quantity).HasColumnType("numeric(18,4)");
        b.Property(e => e.UnitPrice).HasColumnType("numeric(18,4)");
        b.Property(e => e.DiscountPct).HasColumnType("numeric(10,4)");
        b.Property(e => e.DiscountAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxRate).HasColumnType("numeric(10,4)");
        b.Property(e => e.LineSubTotal).HasColumnType("numeric(18,4)");
        b.Property(e => e.TaxAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.LineTotal).HasColumnType("numeric(18,4)");
    }
}

public class POSPaymentConfiguration : IEntityTypeConfiguration<POSPayment>
{
    public void Configure(EntityTypeBuilder<POSPayment> b)
    {
        b.ToTable("pos_payments");
        b.HasKey(e => e.Id);
        b.Property(e => e.POSTransactionId).IsRequired();
        b.Property(e => e.PaymentMethod).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)");
        b.Property(e => e.Reference).HasMaxLength(200);
    }
}

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> b)
    {
        b.ToTable("promotions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.DiscountType).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.DiscountValue).HasColumnType("numeric(18,4)");
        b.Property(e => e.MinimumOrderAmount).HasColumnType("numeric(18,4)");
        b.Property(e => e.ApplicableSkus).HasMaxLength(2000);
        b.HasIndex(e => e.OrganizationId);

        b.HasMany<Coupon>()
            .WithOne(c => c.Promotion)
            .HasForeignKey(c => c.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> b)
    {
        b.ToTable("coupons");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.PromotionId).IsRequired();
        b.Property(e => e.Code).HasMaxLength(100).IsRequired();
        b.HasIndex(e => new { e.OrganizationId, e.Code }).IsUnique();
        b.HasIndex(e => e.PromotionId);
    }
}

public class CouponRedemptionConfiguration : IEntityTypeConfiguration<CouponRedemption>
{
    public void Configure(EntityTypeBuilder<CouponRedemption> b)
    {
        b.ToTable("coupon_redemptions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.CouponId).IsRequired();
        b.Property(e => e.POSTransactionId).IsRequired();
        b.Property(e => e.DiscountApplied).HasColumnType("numeric(18,4)");
        b.HasIndex(e => e.CouponId);
        b.HasIndex(e => e.POSTransactionId);
    }
}
