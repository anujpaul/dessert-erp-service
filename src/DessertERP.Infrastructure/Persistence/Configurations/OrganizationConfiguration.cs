using DessertERP.Domain.Modules.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");
        builder.Property(o => o.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
        builder.Property(o => o.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(o => o.BaseCurrency).HasColumnName("base_currency").HasMaxLength(3).IsRequired();
        builder.Property(o => o.FiscalYearStartMonth).HasColumnName("fiscal_year_start_month").IsRequired();
        builder.Property(o => o.Address).HasColumnName("address").HasMaxLength(500);
        builder.Property(o => o.Phone).HasColumnName("phone").HasMaxLength(50);
        builder.Property(o => o.Email).HasColumnName("email").HasMaxLength(200);
        builder.Property(o => o.TaxId).HasColumnName("tax_id").HasMaxLength(50);
        builder.Property(o => o.LogoUrl).HasColumnName("logo_url").HasMaxLength(500);
        builder.Property(o => o.DefaultCurrency).HasColumnName("default_currency").HasMaxLength(3);
        builder.Property(o => o.Timezone).HasColumnName("timezone").HasMaxLength(100);
        builder.Property(o => o.MoneyDecimalPlaces).HasColumnName("money_decimal_places").IsRequired();
        builder.Property(o => o.MoneyRoundingMethod).HasColumnName("money_rounding_method")
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(o => o.MoneyRoundingLevel).HasColumnName("money_rounding_level")
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(o => o.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.CreatedAt).HasColumnName("created_at");
        builder.Property(o => o.UpdatedAt).HasColumnName("updated_at");
        builder.Property(o => o.IsDeleted).HasColumnName("is_deleted");

        builder.HasIndex(o => o.Code).IsUnique();
    }
}
