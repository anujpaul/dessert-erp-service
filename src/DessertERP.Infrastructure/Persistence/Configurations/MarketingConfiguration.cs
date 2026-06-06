using DessertERP.Domain.Modules.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> b)
    {
        b.ToTable("Campaigns");
        b.HasKey(e => e.Id);

        b.Property(e => e.Name).IsRequired().HasMaxLength(200);
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.Type).HasConversion<string>().HasMaxLength(50);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
        b.Property(e => e.TargetAudience).HasConversion<string>().HasMaxLength(50);
        b.Property(e => e.Budget).HasColumnType("numeric(18,4)");
        b.Property(e => e.ActualSpend).HasColumnType("numeric(18,4)");
        b.Property(e => e.Tags).HasMaxLength(500);

        b.HasIndex(e => e.OrganizationId);
        b.HasIndex(e => e.Status);
    }
}

public class LoyaltyProgramConfiguration : IEntityTypeConfiguration<LoyaltyProgram>
{
    public void Configure(EntityTypeBuilder<LoyaltyProgram> b)
    {
        b.ToTable("LoyaltyPrograms");
        b.HasKey(e => e.Id);

        b.Property(e => e.Name).IsRequired().HasMaxLength(200);
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.PointsPerDollar).HasColumnType("numeric(18,4)");
        b.Property(e => e.DollarPerPoint).HasColumnType("numeric(18,4)");

        b.HasIndex(e => e.OrganizationId);
    }
}

public class CustomerLoyaltyAccountConfiguration : IEntityTypeConfiguration<CustomerLoyaltyAccount>
{
    public void Configure(EntityTypeBuilder<CustomerLoyaltyAccount> b)
    {
        b.ToTable("CustomerLoyaltyAccounts");
        b.HasKey(e => e.Id);

        b.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
        b.Property(e => e.CustomerEmail).HasMaxLength(200);
        b.Property(e => e.Tier).HasConversion<string>().HasMaxLength(20);

        // Ignore computed property
        b.Ignore(e => e.AvailablePoints);

        b.HasOne(e => e.Program)
         .WithMany()
         .HasForeignKey(e => e.LoyaltyProgramId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(e => new { e.OrganizationId, e.CustomerId });
        b.HasIndex(e => e.LoyaltyProgramId);
    }
}
