using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class PriceAgreementConfiguration : IEntityTypeConfiguration<PriceAgreement>
{
    public void Configure(EntityTypeBuilder<PriceAgreement> b)
    {
        b.ToTable("PriceAgreements");
        b.HasKey(e => e.Id);

        b.Property(e => e.Name).IsRequired().HasMaxLength(200);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Currency).HasMaxLength(3).IsRequired();
        b.Property(e => e.Value).HasColumnType("numeric(18,4)");

        // Stored as string so enum renames don't break existing rows
        b.Property(e => e.Level).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.PriceType).HasConversion<string>().HasMaxLength(20);

        // Product-level FK (Item)
        b.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Variant-level FK (SKU)
        b.HasOne(e => e.Variant)
            .WithMany()
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        b.HasIndex(e => new { e.OrganizationId, e.IsActive, e.StartDate, e.EndDate });
        b.HasIndex(e => new { e.ProductId, e.PriceType, e.IsActive });
        b.HasIndex(e => new { e.VariantId, e.PriceType, e.IsActive });
    }
}
