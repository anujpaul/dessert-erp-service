using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.ToTable("categories");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Code).HasMaxLength(30).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.TaxRate).HasColumnType("numeric(8,4)").HasDefaultValue(0m);
        b.Property(e => e.TaxCode).HasMaxLength(30);   // e.g. CLOTHING, FOOTWEAR, FOOD_EXEMPT
        b.HasIndex(e => new { e.OrganizationId, e.Code }).IsUnique();
        b.HasOne(e => e.ParentCategory).WithMany()
            .HasForeignKey(e => e.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> b)
    {
        b.ToTable("brands");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Code).HasMaxLength(30).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.Country).HasMaxLength(100);
        b.Property(e => e.Website).HasMaxLength(300);
        b.Property(e => e.LogoUrl).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.Code }).IsUnique();
    }
}

public class CatalogProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("catalog_products");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Sku).HasMaxLength(50).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.LongDescription).HasMaxLength(4000);
        b.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        b.Property(e => e.BasePrice).HasColumnType("numeric(18,4)");
        b.Property(e => e.BaseCost).HasColumnType("numeric(18,4)");
        // Nullable override — null means "inherit from category"
        b.Property(e => e.TaxRateOverride).HasColumnType("numeric(8,4)").IsRequired(false);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.Tags).HasMaxLength(500);
        b.Property(e => e.ImageUrl).HasMaxLength(500);
        b.Property(e => e.ProductType).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.GenderTarget).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.PreferredVendorId); // nullable FK to vendors
        b.HasIndex(e => new { e.OrganizationId, e.Sku }).IsUnique();
        b.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        b.HasOne(e => e.Brand).WithMany().HasForeignKey(e => e.BrandId);
        // FK to vendors (cross-module) — restrict to avoid cascade issues
        b.HasOne<DessertERP.Domain.Modules.AccountsPayable.Vendor>()
            .WithMany()
            .HasForeignKey(e => e.PreferredVendorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        b.HasMany(e => e.Variants).WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> b)
    {
        b.ToTable("product_variants");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Sku).HasMaxLength(60).IsRequired();
        b.Property(e => e.Barcode).HasMaxLength(50);
        b.Property(e => e.Size).HasMaxLength(30).IsRequired();
        b.Property(e => e.Color).HasMaxLength(50);
        b.Property(e => e.Material).HasMaxLength(100);
        b.Property(e => e.AdditionalAttributes).HasMaxLength(2000);
        b.Property(e => e.PriceOverride).HasColumnType("numeric(18,4)");
        b.Property(e => e.CostOverride).HasColumnType("numeric(18,4)");
        b.Property(e => e.Weight).HasColumnType("numeric(10,4)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.Sku }).IsUnique();
        b.HasIndex(e => e.Barcode).HasFilter("barcode IS NOT NULL");
        b.HasOne(e => e.Inventory).WithOne(i => i.ProductVariant)
            .HasForeignKey<InventoryRecord>(i => i.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
        // Computed helpers - not mapped
    }
}

public class InventoryRecordConfiguration : IEntityTypeConfiguration<InventoryRecord>
{
    public void Configure(EntityTypeBuilder<InventoryRecord> b)
    {
        b.ToTable("inventory_records");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.QuantityOnHand).HasColumnType("numeric(18,4)");
        b.Property(e => e.QuantityReserved).HasColumnType("numeric(18,4)");
        b.Property(e => e.QuantityOnOrder).HasColumnType("numeric(18,4)");
        b.Property(e => e.ReorderPoint).HasColumnType("numeric(18,4)");
        b.Property(e => e.MinimumStock).HasColumnType("numeric(18,4)");
        b.Property(e => e.MaximumStock).HasColumnType("numeric(18,4)");
        b.Property(e => e.AverageCost).HasColumnType("numeric(18,4)");
        b.Property(e => e.Location).HasMaxLength(50);
        // Computed — not persisted
        b.Ignore(e => e.QuantityAvailable);
        b.Ignore(e => e.QuantityProjected);
        b.Ignore(e => e.NeedsReorder);
        b.Ignore(e => e.IsOutOfStock);
        b.Ignore(e => e.StockValue);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> b)
    {
        b.ToTable("inventory_transactions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.ProductVariantId).IsRequired();
        b.Property(e => e.TransactionType).HasConversion<string>().HasMaxLength(40);
        b.Property(e => e.Quantity).HasColumnType("numeric(18,4)");
        b.Property(e => e.UnitCost).HasColumnType("numeric(18,4)");
        b.Property(e => e.BalanceAfter).HasColumnType("numeric(18,4)");
        b.Property(e => e.ReferenceNumber).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.CreatedBy).HasMaxLength(200);
        b.HasOne(e => e.ProductVariant).WithMany()
            .HasForeignKey(e => e.ProductVariantId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(e => new { e.OrganizationId, e.ProductVariantId, e.TransactionDate });
        // Transactions are permanent — no soft-delete filter needed
    }
}
