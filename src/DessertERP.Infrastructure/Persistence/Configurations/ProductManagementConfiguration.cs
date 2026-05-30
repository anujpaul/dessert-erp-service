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
        b.Property(e => e.TaxRate).HasColumnType("numeric(8,4)");
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
        b.Property(e => e.ReorderPoint).HasColumnType("numeric(18,4)");
        b.Property(e => e.MinimumStock).HasColumnType("numeric(18,4)");
        b.Property(e => e.MaximumStock).HasColumnType("numeric(18,4)");
        b.Property(e => e.Location).HasMaxLength(50);
        // Computed
        b.Ignore(e => e.QuantityAvailable);
        b.Ignore(e => e.NeedsReorder);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
