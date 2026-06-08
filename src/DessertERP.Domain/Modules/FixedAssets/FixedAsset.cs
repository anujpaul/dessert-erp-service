using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.FixedAssets;

public enum AssetStatus
{
    Active,
    UnderMaintenance,
    Disposed,
    FullyDepreciated,
    Impaired,
    Inactive
}

public enum DepreciationMethod
{
    StraightLine,
    DecliningBalance,
    DoubleDecliningBalance,
    UnitsOfProduction,
    SumOfYearsDigits,
    None
}

public enum AssetCategory
{
    Land,
    Buildings,
    Machinery,
    Vehicles,
    Furniture,
    ComputerEquipment,
    IntangibleAssets,
    LeaseholdImprovements,
    OfficeEquipment,
    Other
}

/// <summary>
/// Represents a fixed (long-lived) asset on the balance sheet.
/// </summary>
public class FixedAsset : BaseEntity
{
    public Guid OrganizationId { get; set; }

    // Identity
    public string AssetCode { get; set; } = string.Empty;   // e.g. FA-00042
    public string AssetName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssetCategory Category { get; set; }
    public AssetStatus Status { get; set; } = AssetStatus.Active;

    // Acquisition
    public DateTime AcquisitionDate { get; set; }
    public decimal AcquisitionCost { get; set; }          // original purchase price
    public string? PurchaseOrderRef { get; set; }
    public string? Supplier { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public Guid? GLAssetAccountId { get; set; }           // Balance sheet account
    public Guid? GLAccumulatedDepreciationAccountId { get; set; }
    public Guid? GLDepreciationExpenseAccountId { get; set; }

    // Depreciation
    public DepreciationMethod DepreciationMethod { get; set; } = DepreciationMethod.StraightLine;
    public int UsefulLifeYears { get; set; }               // 0 = non-depreciable (Land)
    public decimal SalvageValue { get; set; }              // residual value at end of life
    public DateTime? DepreciationStartDate { get; set; }
    public decimal AccumulatedDepreciation { get; set; }   // running total
    public DateTime? LastDepreciationDate { get; set; }

    // Units of Production (optional)
    public decimal? TotalEstimatedUnits { get; set; }
    public decimal? UnitsProducedToDate { get; set; }

    // Computed
    public decimal NetBookValue => AcquisitionCost - AccumulatedDepreciation;
    public decimal DepreciableBase => AcquisitionCost - SalvageValue;
    public bool IsFullyDepreciated => NetBookValue <= SalvageValue;

    public List<AssetDepreciation> DepreciationEntries { get; set; } = [];
    public List<AssetMaintenance> MaintenanceRecords { get; set; } = [];

    // ── Business logic ────────────────────────────────────────────────────────

    public decimal CalculatePeriodicDepreciation(int periodMonths = 1)
    {
        if (DepreciationMethod == DepreciationMethod.None || UsefulLifeYears == 0)
            return 0m;

        if (IsFullyDepreciated) return 0m;

        var fraction = (decimal)periodMonths / 12m;

        return DepreciationMethod switch
        {
            DepreciationMethod.StraightLine =>
                Math.Min(DepreciableBase / UsefulLifeYears * fraction,
                         NetBookValue - SalvageValue),

            DepreciationMethod.DecliningBalance =>
                Math.Min((NetBookValue - SalvageValue) / UsefulLifeYears * fraction,
                         NetBookValue - SalvageValue),

            DepreciationMethod.DoubleDecliningBalance =>
                Math.Min(NetBookValue * (2m / UsefulLifeYears) * fraction,
                         NetBookValue - SalvageValue),

            DepreciationMethod.SumOfYearsDigits =>
                CalculateSumOfYearsDepreciation(fraction),

            // UnitsOfProduction is handled via PostDepreciation(units)
            _ => 0m
        };
    }

    private decimal CalculateSumOfYearsDepreciation(decimal fraction)
    {
        var n = UsefulLifeYears;
        var sumOfYears = n * (n + 1) / 2m;
        // remaining life in years from depreciation start
        var elapsed = LastDepreciationDate.HasValue
            ? (decimal)(LastDepreciationDate.Value - (DepreciationStartDate ?? AcquisitionDate)).TotalDays / 365.25m
            : 0m;
        var remaining = Math.Max(0m, UsefulLifeYears - elapsed);
        return Math.Min(DepreciableBase * remaining / sumOfYears * fraction,
                        NetBookValue - SalvageValue);
    }

    public void ApplyDepreciation(decimal amount, string postedBy, string? reference = null)
    {
        if (Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Cannot depreciate a disposed asset.");
        if (amount < 0)
            throw new ArgumentException("Depreciation amount must be non-negative.");

        var entry = new AssetDepreciation
        {
            AssetId = Id,
            Amount = amount,
            Date = DateTime.UtcNow,
            PostedBy = postedBy,
            Reference = reference,
            RunningNBV = NetBookValue - amount
        };

        AccumulatedDepreciation += amount;
        LastDepreciationDate = DateTime.UtcNow;
        DepreciationEntries.Add(entry);

        if (IsFullyDepreciated)
            Status = AssetStatus.FullyDepreciated;
    }

    public void Dispose(decimal disposalProceeds, string disposedBy, string reason)
    {
        if (Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Asset is already disposed.");

        Status = AssetStatus.Disposed;
        // Caller should persist an AssetDisposal record
    }

    public void Impair(decimal impairmentAmount, string postedBy)
    {
        if (Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Cannot impair a disposed asset.");
        if (impairmentAmount <= 0)
            throw new ArgumentException("Impairment amount must be positive.");
        if (impairmentAmount > NetBookValue - SalvageValue)
            throw new InvalidOperationException("Impairment cannot reduce NBV below salvage value.");

        AccumulatedDepreciation += impairmentAmount;
        Status = AssetStatus.Impaired;
    }

    public void UpdateLocation(string newLocation) => Location = newLocation;
    public void SetUnderMaintenance() => Status = AssetStatus.UnderMaintenance;
    public void SetActive() => Status = AssetStatus.Active;
}

/// <summary>
/// Immutable depreciation run record.
/// </summary>
public class AssetDepreciation : BaseEntity
{
    public Guid AssetId { get; set; }
    public FixedAsset Asset { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningNBV { get; set; }    // NBV after this entry
    public string PostedBy { get; set; } = string.Empty;
    public string? Reference { get; set; }     // e.g. "FY2026-Q1"
    public Guid? JournalEntryId { get; set; }  // link to GL journal entry
}

/// <summary>
/// Asset disposal record — created when an asset is sold/scrapped.
/// </summary>
public class AssetDisposal : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid AssetId { get; set; }
    public FixedAsset Asset { get; set; } = null!;
    public DateTime DisposalDate { get; set; }
    public string DisposalType { get; set; } = "Sale";  // Sale, Scrap, Donation, Write-off
    public decimal DisposalProceeds { get; set; }
    public decimal NetBookValueAtDisposal { get; set; }
    public decimal GainLoss => DisposalProceeds - NetBookValueAtDisposal;
    public string DisposedBy { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? BuyerName { get; set; }
    public Guid? GLGainLossAccountId { get; set; }
    public Guid? JournalEntryId { get; set; }
}

/// <summary>
/// Asset transfer — moves asset between locations/departments/cost centres.
/// </summary>
public class AssetTransfer : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid AssetId { get; set; }
    public FixedAsset Asset { get; set; } = null!;
    public DateTime TransferDate { get; set; }
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public string? FromDepartment { get; set; }
    public string? ToDepartment { get; set; }
    public string TransferredBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

/// <summary>
/// Maintenance record — tracks servicing and repair costs.
/// </summary>
public class AssetMaintenance : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid AssetId { get; set; }
    public FixedAsset Asset { get; set; } = null!;
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = "Routine";  // Routine, Repair, Overhaul, Upgrade
    public decimal Cost { get; set; }
    public bool CapitalizeCost { get; set; }   // if true, add to asset AcquisitionCost
    public string? Vendor { get; set; }
    public string? Description { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime? NextMaintenanceDue { get; set; }
}
