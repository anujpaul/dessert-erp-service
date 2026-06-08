using DessertERP.Domain.Modules.FixedAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class FixedAssetConfiguration : IEntityTypeConfiguration<FixedAsset>
{
    public void Configure(EntityTypeBuilder<FixedAsset> b)
    {
        b.ToTable("fixed_assets");
        b.HasKey(e => e.Id);

        b.Property(e => e.AssetCode).IsRequired().HasMaxLength(50);
        b.Property(e => e.AssetName).IsRequired().HasMaxLength(200);
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.Supplier).HasMaxLength(200);
        b.Property(e => e.SerialNumber).HasMaxLength(100);
        b.Property(e => e.Location).HasMaxLength(200);
        b.Property(e => e.PurchaseOrderRef).HasMaxLength(100);
        b.Property(e => e.AcquisitionCost).HasPrecision(18, 4);
        b.Property(e => e.SalvageValue).HasPrecision(18, 4);
        b.Property(e => e.AccumulatedDepreciation).HasPrecision(18, 4);
        b.Property(e => e.TotalEstimatedUnits).HasPrecision(18, 4);
        b.Property(e => e.UnitsProducedToDate).HasPrecision(18, 4);

        b.Property(e => e.Category).HasConversion<string>();
        b.Property(e => e.Status).HasConversion<string>();
        b.Property(e => e.DepreciationMethod).HasConversion<string>();

        // Computed properties — not mapped to columns
        b.Ignore(e => e.NetBookValue);
        b.Ignore(e => e.DepreciableBase);
        b.Ignore(e => e.IsFullyDepreciated);

        b.HasIndex(e => new { e.OrganizationId, e.AssetCode }).IsUnique();

        b.HasMany(e => e.DepreciationEntries)
         .WithOne(d => d.Asset)
         .HasForeignKey(d => d.AssetId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(e => e.MaintenanceRecords)
         .WithOne(m => m.Asset)
         .HasForeignKey(m => m.AssetId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AssetDepreciationConfiguration : IEntityTypeConfiguration<AssetDepreciation>
{
    public void Configure(EntityTypeBuilder<AssetDepreciation> b)
    {
        b.ToTable("asset_depreciations");
        b.HasKey(e => e.Id);
        b.Property(e => e.Amount).HasPrecision(18, 4);
        b.Property(e => e.RunningNBV).HasPrecision(18, 4);
        b.Property(e => e.PostedBy).IsRequired().HasMaxLength(200);
        b.Property(e => e.Reference).HasMaxLength(100);
    }
}

public class AssetDisposalConfiguration : IEntityTypeConfiguration<AssetDisposal>
{
    public void Configure(EntityTypeBuilder<AssetDisposal> b)
    {
        b.ToTable("asset_disposals");
        b.HasKey(e => e.Id);

        b.Property(e => e.DisposalType).IsRequired().HasMaxLength(50);
        b.Property(e => e.DisposalProceeds).HasPrecision(18, 4);
        b.Property(e => e.NetBookValueAtDisposal).HasPrecision(18, 4);
        b.Property(e => e.DisposedBy).IsRequired().HasMaxLength(200);
        b.Property(e => e.Reason).HasMaxLength(500);
        b.Property(e => e.BuyerName).HasMaxLength(200);

        // GainLoss is a computed property — not stored
        b.Ignore(e => e.GainLoss);
    }
}

public class AssetTransferConfiguration : IEntityTypeConfiguration<AssetTransfer>
{
    public void Configure(EntityTypeBuilder<AssetTransfer> b)
    {
        b.ToTable("asset_transfers");
        b.HasKey(e => e.Id);

        b.Property(e => e.FromLocation).IsRequired().HasMaxLength(200);
        b.Property(e => e.ToLocation).IsRequired().HasMaxLength(200);
        b.Property(e => e.FromDepartment).HasMaxLength(200);
        b.Property(e => e.ToDepartment).HasMaxLength(200);
        b.Property(e => e.TransferredBy).IsRequired().HasMaxLength(200);
        b.Property(e => e.Notes).HasMaxLength(1000);
    }
}

public class AssetMaintenanceConfiguration : IEntityTypeConfiguration<AssetMaintenance>
{
    public void Configure(EntityTypeBuilder<AssetMaintenance> b)
    {
        b.ToTable("asset_maintenances");
        b.HasKey(e => e.Id);

        b.Property(e => e.MaintenanceType).IsRequired().HasMaxLength(50);
        b.Property(e => e.Cost).HasPrecision(18, 4);
        b.Property(e => e.Vendor).HasMaxLength(200);
        b.Property(e => e.Description).HasMaxLength(1000);
        b.Property(e => e.PerformedBy).IsRequired().HasMaxLength(200);
    }
}
