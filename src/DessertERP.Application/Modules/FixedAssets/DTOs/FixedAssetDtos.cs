namespace DessertERP.Application.Modules.FixedAssets.DTOs;

// ── Asset summary (list views) ────────────────────────────────────────────────

public record FixedAssetSummaryDto(
    Guid Id,
    string AssetCode,
    string AssetName,
    string Category,
    string Status,
    DateTime AcquisitionDate,
    decimal AcquisitionCost,
    decimal AccumulatedDepreciation,
    decimal NetBookValue,
    string DepreciationMethod,
    int UsefulLifeYears,
    string? Location,
    DateTime? LastDepreciationDate
);

// ── Asset detail ──────────────────────────────────────────────────────────────

public record FixedAssetDetailDto(
    Guid Id,
    string AssetCode,
    string AssetName,
    string? Description,
    string Category,
    string Status,
    DateTime AcquisitionDate,
    decimal AcquisitionCost,
    string? PurchaseOrderRef,
    string? Supplier,
    string? SerialNumber,
    string? Location,
    Guid? GLAssetAccountId,
    Guid? GLAccumulatedDepreciationAccountId,
    Guid? GLDepreciationExpenseAccountId,
    string DepreciationMethod,
    int UsefulLifeYears,
    decimal SalvageValue,
    DateTime? DepreciationStartDate,
    decimal AccumulatedDepreciation,
    decimal NetBookValue,
    decimal DepreciableBase,
    bool IsFullyDepreciated,
    DateTime? LastDepreciationDate,
    decimal? TotalEstimatedUnits,
    decimal? UnitsProducedToDate,
    List<AssetDepreciationDto> DepreciationEntries,
    List<AssetMaintenanceDto> MaintenanceRecords
);

// ── Depreciation entry ────────────────────────────────────────────────────────

public record AssetDepreciationDto(
    Guid Id,
    Guid AssetId,
    DateTime Date,
    decimal Amount,
    decimal RunningNBV,
    string PostedBy,
    string? Reference,
    Guid? JournalEntryId
);

// ── Disposal ──────────────────────────────────────────────────────────────────

public record AssetDisposalDto(
    Guid Id,
    Guid AssetId,
    string AssetCode,
    string AssetName,
    DateTime DisposalDate,
    string DisposalType,
    decimal DisposalProceeds,
    decimal NetBookValueAtDisposal,
    decimal GainLoss,
    string DisposedBy,
    string? Reason,
    string? BuyerName,
    Guid? GLGainLossAccountId,
    Guid? JournalEntryId
);

// ── Transfer ──────────────────────────────────────────────────────────────────

public record AssetTransferDto(
    Guid Id,
    Guid AssetId,
    string AssetCode,
    string AssetName,
    DateTime TransferDate,
    string FromLocation,
    string ToLocation,
    string? FromDepartment,
    string? ToDepartment,
    string TransferredBy,
    string? Notes
);

// ── Maintenance ───────────────────────────────────────────────────────────────

public record AssetMaintenanceDto(
    Guid Id,
    Guid AssetId,
    DateTime MaintenanceDate,
    string MaintenanceType,
    decimal Cost,
    bool CapitalizeCost,
    string? Vendor,
    string? Description,
    string PerformedBy,
    DateTime? NextMaintenanceDue
);

// ── Depreciation schedule (projection) ───────────────────────────────────────

public record DepreciationScheduleRowDto(
    int Period,
    int Year,
    int Month,
    decimal DepreciationAmount,
    decimal AccumulatedDepreciation,
    decimal NetBookValue
);

// ── Asset statistics / dashboard ──────────────────────────────────────────────

public record FixedAssetSummaryStatsDto(
    int TotalAssets,
    int ActiveAssets,
    int FullyDepreciatedAssets,
    int DisposedAssets,
    decimal TotalAcquisitionCost,
    decimal TotalAccumulatedDepreciation,
    decimal TotalNetBookValue,
    Dictionary<string, decimal> NetBookValueByCategory
);

// ══════════════════════════════════════════════════════════════════════════════
// Requests
// ══════════════════════════════════════════════════════════════════════════════

public record CreateFixedAssetRequest(
    string AssetCode,
    string AssetName,
    string? Description,
    string Category,              // AssetCategory enum string
    DateTime AcquisitionDate,
    decimal AcquisitionCost,
    string? PurchaseOrderRef,
    string? Supplier,
    string? SerialNumber,
    string? Location,
    Guid? GLAssetAccountId,
    Guid? GLAccumulatedDepreciationAccountId,
    Guid? GLDepreciationExpenseAccountId,
    string DepreciationMethod,    // DepreciationMethod enum string
    int UsefulLifeYears,
    decimal SalvageValue,
    DateTime? DepreciationStartDate,
    decimal? TotalEstimatedUnits
);

public record UpdateFixedAssetRequest(
    string AssetName,
    string? Description,
    string? Location,
    string? SerialNumber,
    Guid? GLAssetAccountId,
    Guid? GLAccumulatedDepreciationAccountId,
    Guid? GLDepreciationExpenseAccountId,
    int UsefulLifeYears,
    decimal SalvageValue,
    decimal? TotalEstimatedUnits
);

public record RunDepreciationRequest(
    string PostedBy,
    string? Reference,            // e.g. "FY2026-M06"
    int PeriodMonths = 1,
    decimal? UnitsProduced = null // for UnitsOfProduction method
);

public record RunBulkDepreciationRequest(
    string PostedBy,
    string? Reference,
    int PeriodMonths = 1,
    List<Guid>? AssetIds = null  // null = all eligible assets
);

public record BulkDepreciationResultDto(
    int AssetsProcessed,
    int AssetsSkipped,
    decimal TotalDepreciationPosted,
    List<string> Messages
);

public record DisposeAssetRequest(
    DateTime DisposalDate,
    string DisposalType,          // Sale, Scrap, Donation, Write-off
    decimal DisposalProceeds,
    string DisposedBy,
    string? Reason,
    string? BuyerName,
    Guid? GLGainLossAccountId
);

public record CreateTransferRequest(
    Guid AssetId,
    DateTime TransferDate,
    string ToLocation,
    string? ToDepartment,
    string TransferredBy,
    string? Notes
);

public record CreateMaintenanceRequest(
    Guid AssetId,
    DateTime MaintenanceDate,
    string MaintenanceType,
    decimal Cost,
    bool CapitalizeCost,
    string? Vendor,
    string? Description,
    string PerformedBy,
    DateTime? NextMaintenanceDue
);

public record ImpairAssetRequest(
    decimal ImpairmentAmount,
    string PostedBy,
    string? Reason
);
