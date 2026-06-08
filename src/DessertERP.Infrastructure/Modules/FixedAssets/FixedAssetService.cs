using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.FixedAssets;
using DessertERP.Application.Modules.FixedAssets.DTOs;
using DessertERP.Domain.Modules.FixedAssets;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Infrastructure.Modules.FixedAssets;

public class FixedAssetService : IFixedAssetService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public FixedAssetService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db  = db;
        _org = org;
    }

    // ── Assets ────────────────────────────────────────────────────────────────

    public async Task<List<FixedAssetSummaryDto>> GetAssetsAsync(
        string? category, string? status, CancellationToken ct = default)
    {
        var q = _db.FixedAssets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category) &&
            Enum.TryParse<AssetCategory>(category, true, out var cat))
            q = q.Where(a => a.Category == cat);

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<AssetStatus>(status, true, out var st))
            q = q.Where(a => a.Status == st);

        return await q
            .OrderBy(a => a.AssetCode)
            .Select(a => ToSummary(a))
            .ToListAsync(ct);
    }

    public async Task<FixedAssetDetailDto?> GetAssetAsync(Guid id, CancellationToken ct = default)
    {
        var a = await _db.FixedAssets
            .Include(x => x.DepreciationEntries.OrderByDescending(d => d.Date))
            .Include(x => x.MaintenanceRecords.OrderByDescending(m => m.MaintenanceDate))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return a is null ? null : ToDetail(a);
    }

    public async Task<FixedAssetDetailDto> CreateAssetAsync(
        CreateFixedAssetRequest req, CancellationToken ct = default)
    {
        var orgId = _org.OrganizationId;

        // Unique asset code check
        var exists = await _db.FixedAssets
            .AnyAsync(a => a.AssetCode == req.AssetCode, ct);
        if (exists)
            throw new InvalidOperationException($"Asset code '{req.AssetCode}' already exists.");

        var asset = new FixedAsset
        {
            OrganizationId = orgId,
            AssetCode     = req.AssetCode,
            AssetName     = req.AssetName,
            Description   = req.Description,
            Category      = Enum.Parse<AssetCategory>(req.Category, true),
            AcquisitionDate = req.AcquisitionDate,
            AcquisitionCost = req.AcquisitionCost,
            PurchaseOrderRef = req.PurchaseOrderRef,
            Supplier      = req.Supplier,
            SerialNumber  = req.SerialNumber,
            Location      = req.Location,
            GLAssetAccountId = req.GLAssetAccountId,
            GLAccumulatedDepreciationAccountId = req.GLAccumulatedDepreciationAccountId,
            GLDepreciationExpenseAccountId = req.GLDepreciationExpenseAccountId,
            DepreciationMethod = Enum.Parse<DepreciationMethod>(req.DepreciationMethod, true),
            UsefulLifeYears = req.UsefulLifeYears,
            SalvageValue  = req.SalvageValue,
            DepreciationStartDate = req.DepreciationStartDate,
            TotalEstimatedUnits = req.TotalEstimatedUnits,
            Status        = AssetStatus.Active
        };

        _db.FixedAssets.Add(asset);
        await _db.SaveChangesAsync(ct);
        return ToDetail(asset);
    }

    public async Task<FixedAssetDetailDto> UpdateAssetAsync(
        Guid id, UpdateFixedAssetRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets
            .Include(x => x.DepreciationEntries)
            .Include(x => x.MaintenanceRecords)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new InvalidOperationException("Asset not found.");

        if (asset.Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Cannot update a disposed asset.");

        asset.AssetName   = req.AssetName;
        asset.Description = req.Description;
        asset.Location    = req.Location;
        asset.SerialNumber = req.SerialNumber;
        asset.GLAssetAccountId = req.GLAssetAccountId;
        asset.GLAccumulatedDepreciationAccountId = req.GLAccumulatedDepreciationAccountId;
        asset.GLDepreciationExpenseAccountId = req.GLDepreciationExpenseAccountId;
        asset.UsefulLifeYears = req.UsefulLifeYears;
        asset.SalvageValue    = req.SalvageValue;
        asset.TotalEstimatedUnits = req.TotalEstimatedUnits;

        await _db.SaveChangesAsync(ct);
        return ToDetail(asset);
    }

    public async Task SetAssetStatusAsync(Guid id, string status, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets.FindAsync([id], ct)
            ?? throw new InvalidOperationException("Asset not found.");

        var newStatus = Enum.Parse<AssetStatus>(status, true);
        switch (newStatus)
        {
            case AssetStatus.Active:           asset.SetActive(); break;
            case AssetStatus.UnderMaintenance: asset.SetUnderMaintenance(); break;
            case AssetStatus.Inactive:         asset.Status = AssetStatus.Inactive; break;
            default:
                throw new InvalidOperationException($"Cannot manually set status to '{status}'.");
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task<FixedAssetSummaryStatsDto> GetSummaryStatsAsync(CancellationToken ct = default)
    {
        var assets = await _db.FixedAssets.ToListAsync(ct);

        var byCategory = assets
            .GroupBy(a => a.Category.ToString())
            .ToDictionary(g => g.Key, g => g.Sum(a => a.NetBookValue));

        return new FixedAssetSummaryStatsDto(
            TotalAssets:               assets.Count,
            ActiveAssets:              assets.Count(a => a.Status == AssetStatus.Active),
            FullyDepreciatedAssets:    assets.Count(a => a.Status == AssetStatus.FullyDepreciated),
            DisposedAssets:            assets.Count(a => a.Status == AssetStatus.Disposed),
            TotalAcquisitionCost:      assets.Sum(a => a.AcquisitionCost),
            TotalAccumulatedDepreciation: assets.Sum(a => a.AccumulatedDepreciation),
            TotalNetBookValue:         assets.Sum(a => a.NetBookValue),
            NetBookValueByCategory:    byCategory
        );
    }

    // ── Depreciation ──────────────────────────────────────────────────────────

    public async Task<AssetDepreciationDto> RunDepreciationAsync(
        Guid assetId, RunDepreciationRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets
            .Include(a => a.DepreciationEntries)
            .FirstOrDefaultAsync(a => a.Id == assetId, ct)
            ?? throw new InvalidOperationException("Asset not found.");

        if (asset.Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Asset is disposed — cannot depreciate.");

        decimal amount;
        if (asset.DepreciationMethod == DepreciationMethod.UnitsOfProduction)
        {
            if (!req.UnitsProduced.HasValue)
                throw new InvalidOperationException("Units produced required for UnitsOfProduction method.");
            if (!asset.TotalEstimatedUnits.HasValue || asset.TotalEstimatedUnits == 0)
                throw new InvalidOperationException("TotalEstimatedUnits must be set on asset.");

            amount = asset.DepreciableBase / asset.TotalEstimatedUnits.Value * req.UnitsProduced.Value;
            amount = Math.Min(amount, asset.NetBookValue - asset.SalvageValue);
            asset.UnitsProducedToDate = (asset.UnitsProducedToDate ?? 0) + req.UnitsProduced.Value;
        }
        else
        {
            amount = asset.CalculatePeriodicDepreciation(req.PeriodMonths);
        }

        if (amount <= 0)
            throw new InvalidOperationException("No depreciation to post (asset may be fully depreciated).");

        asset.ApplyDepreciation(amount, req.PostedBy, req.Reference);
        await _db.SaveChangesAsync(ct);

        var entry = asset.DepreciationEntries.Last();
        return ToDepreciationDto(entry);
    }

    public async Task<BulkDepreciationResultDto> RunBulkDepreciationAsync(
        RunBulkDepreciationRequest req, CancellationToken ct = default)
    {
        var q = _db.FixedAssets
            .Include(a => a.DepreciationEntries)
            .Where(a => a.Status == AssetStatus.Active &&
                        a.DepreciationMethod != DepreciationMethod.None &&
                        a.UsefulLifeYears > 0);

        if (req.AssetIds is { Count: > 0 })
            q = q.Where(a => req.AssetIds.Contains(a.Id));

        var assets = await q.ToListAsync(ct);

        int processed = 0, skipped = 0;
        decimal totalPosted = 0;
        var messages = new List<string>();

        foreach (var asset in assets)
        {
            try
            {
                decimal amount;
                if (asset.DepreciationMethod == DepreciationMethod.UnitsOfProduction)
                {
                    skipped++;
                    messages.Add($"{asset.AssetCode}: skipped (UnitsOfProduction requires manual run)");
                    continue;
                }
                amount = asset.CalculatePeriodicDepreciation(req.PeriodMonths);
                if (amount <= 0)
                {
                    skipped++;
                    messages.Add($"{asset.AssetCode}: skipped (fully depreciated or no depreciation)");
                    continue;
                }
                asset.ApplyDepreciation(amount, req.PostedBy, req.Reference);
                totalPosted += amount;
                processed++;
            }
            catch (Exception ex)
            {
                skipped++;
                messages.Add($"{asset.AssetCode}: error — {ex.Message}");
            }
        }

        await _db.SaveChangesAsync(ct);
        messages.Insert(0, $"Processed {processed} assets, skipped {skipped}. Total posted: {totalPosted:N2}");
        return new BulkDepreciationResultDto(processed, skipped, totalPosted, messages);
    }

    public async Task<List<DepreciationScheduleRowDto>> GetDepreciationScheduleAsync(
        Guid assetId, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets.FindAsync([assetId], ct)
            ?? throw new InvalidOperationException("Asset not found.");

        if (asset.DepreciationMethod == DepreciationMethod.None || asset.UsefulLifeYears == 0)
            return [];

        var schedule = new List<DepreciationScheduleRowDto>();
        var startDate = asset.DepreciationStartDate ?? asset.AcquisitionDate;
        var accumulated = asset.AccumulatedDepreciation;
        var nbv = asset.NetBookValue;
        var depreciableBase = asset.DepreciableBase;
        int period = 1;

        // Project forward from today through useful life
        var projStart = asset.LastDepreciationDate.HasValue
            ? asset.LastDepreciationDate.Value.AddMonths(1)
            : startDate;

        var current = projStart;
        var endDate = startDate.AddYears(asset.UsefulLifeYears);

        while (current <= endDate && nbv > asset.SalvageValue)
        {
            decimal monthlyDepr = asset.DepreciationMethod switch
            {
                DepreciationMethod.StraightLine =>
                    depreciableBase / (asset.UsefulLifeYears * 12m),

                DepreciationMethod.DecliningBalance =>
                    (nbv - asset.SalvageValue) / (asset.UsefulLifeYears * 12m),

                DepreciationMethod.DoubleDecliningBalance =>
                    nbv * (2m / asset.UsefulLifeYears) / 12m,

                _ => depreciableBase / (asset.UsefulLifeYears * 12m)
            };

            monthlyDepr = Math.Min(monthlyDepr, nbv - asset.SalvageValue);
            if (monthlyDepr <= 0) break;

            accumulated += monthlyDepr;
            nbv -= monthlyDepr;

            schedule.Add(new DepreciationScheduleRowDto(
                Period: period,
                Year: current.Year,
                Month: current.Month,
                DepreciationAmount: Math.Round(monthlyDepr, 2),
                AccumulatedDepreciation: Math.Round(accumulated, 2),
                NetBookValue: Math.Round(nbv, 2)
            ));

            current = current.AddMonths(1);
            period++;

            if (schedule.Count >= 600) break; // cap at 50 years
        }

        return schedule;
    }

    public async Task<List<AssetDepreciationDto>> GetDepreciationHistoryAsync(
        Guid assetId, CancellationToken ct = default)
        => await _db.AssetDepreciations
            .Where(d => d.AssetId == assetId)
            .OrderByDescending(d => d.Date)
            .Select(d => ToDepreciationDto(d))
            .ToListAsync(ct);

    // ── Disposal ──────────────────────────────────────────────────────────────

    public async Task<AssetDisposalDto> DisposeAssetAsync(
        Guid assetId, DisposeAssetRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets.FindAsync([assetId], ct)
            ?? throw new InvalidOperationException("Asset not found.");

        if (asset.Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Asset is already disposed.");

        var nbvAtDisposal = asset.NetBookValue;
        asset.Dispose(req.DisposalProceeds, req.DisposedBy, req.Reason ?? "");

        var disposal = new AssetDisposal
        {
            OrganizationId        = asset.OrganizationId,
            AssetId               = assetId,
            DisposalDate          = req.DisposalDate,
            DisposalType          = req.DisposalType,
            DisposalProceeds      = req.DisposalProceeds,
            NetBookValueAtDisposal = nbvAtDisposal,
            DisposedBy            = req.DisposedBy,
            Reason                = req.Reason,
            BuyerName             = req.BuyerName,
            GLGainLossAccountId   = req.GLGainLossAccountId
        };

        _db.AssetDisposals.Add(disposal);
        await _db.SaveChangesAsync(ct);

        return ToDisposalDto(disposal, asset);
    }

    public async Task<List<AssetDisposalDto>> GetDisposalsAsync(CancellationToken ct = default)
        => await _db.AssetDisposals
            .Include(d => d.Asset)
            .OrderByDescending(d => d.DisposalDate)
            .Select(d => ToDisposalDto(d, d.Asset))
            .ToListAsync(ct);

    public async Task<AssetDisposalDto?> GetDisposalAsync(Guid id, CancellationToken ct = default)
    {
        var d = await _db.AssetDisposals
            .Include(x => x.Asset)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return d is null ? null : ToDisposalDto(d, d.Asset);
    }

    // ── Transfers ─────────────────────────────────────────────────────────────

    public async Task<AssetTransferDto> CreateTransferAsync(
        CreateTransferRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets.FindAsync([req.AssetId], ct)
            ?? throw new InvalidOperationException("Asset not found.");

        if (asset.Status == AssetStatus.Disposed)
            throw new InvalidOperationException("Cannot transfer a disposed asset.");

        var transfer = new AssetTransfer
        {
            OrganizationId = asset.OrganizationId,
            AssetId        = req.AssetId,
            TransferDate   = req.TransferDate,
            FromLocation   = asset.Location ?? "(unknown)",
            ToLocation     = req.ToLocation,
            ToDepartment   = req.ToDepartment,
            TransferredBy  = req.TransferredBy,
            Notes          = req.Notes
        };

        asset.UpdateLocation(req.ToLocation);

        _db.AssetTransfers.Add(transfer);
        await _db.SaveChangesAsync(ct);
        return ToTransferDto(transfer, asset);
    }

    public async Task<List<AssetTransferDto>> GetTransfersAsync(
        Guid? assetId, CancellationToken ct = default)
    {
        var q = _db.AssetTransfers.Include(t => t.Asset).AsQueryable();
        if (assetId.HasValue)
            q = q.Where(t => t.AssetId == assetId.Value);

        return await q
            .OrderByDescending(t => t.TransferDate)
            .Select(t => ToTransferDto(t, t.Asset))
            .ToListAsync(ct);
    }

    // ── Maintenance ───────────────────────────────────────────────────────────

    public async Task<AssetMaintenanceDto> AddMaintenanceAsync(
        CreateMaintenanceRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets.FindAsync([req.AssetId], ct)
            ?? throw new InvalidOperationException("Asset not found.");

        var record = new AssetMaintenance
        {
            OrganizationId   = asset.OrganizationId,
            AssetId          = req.AssetId,
            MaintenanceDate  = req.MaintenanceDate,
            MaintenanceType  = req.MaintenanceType,
            Cost             = req.Cost,
            CapitalizeCost   = req.CapitalizeCost,
            Vendor           = req.Vendor,
            Description      = req.Description,
            PerformedBy      = req.PerformedBy,
            NextMaintenanceDue = req.NextMaintenanceDue
        };

        // If cost is capitalised, add it to acquisition cost (increases depreciable base)
        if (req.CapitalizeCost)
            asset.AcquisitionCost += req.Cost;

        _db.AssetMaintenances.Add(record);
        await _db.SaveChangesAsync(ct);
        return ToMaintenanceDto(record);
    }

    public async Task<List<AssetMaintenanceDto>> GetMaintenanceAsync(
        Guid assetId, CancellationToken ct = default)
        => await _db.AssetMaintenances
            .Where(m => m.AssetId == assetId)
            .OrderByDescending(m => m.MaintenanceDate)
            .Select(m => ToMaintenanceDto(m))
            .ToListAsync(ct);

    // ── Impairment ────────────────────────────────────────────────────────────

    public async Task<AssetDepreciationDto> ImpairAssetAsync(
        Guid assetId, ImpairAssetRequest req, CancellationToken ct = default)
    {
        var asset = await _db.FixedAssets
            .Include(a => a.DepreciationEntries)
            .FirstOrDefaultAsync(a => a.Id == assetId, ct)
            ?? throw new InvalidOperationException("Asset not found.");

        asset.Impair(req.ImpairmentAmount, req.PostedBy);

        var entry = new AssetDepreciation
        {
            AssetId     = assetId,
            Amount      = req.ImpairmentAmount,
            Date        = DateTime.UtcNow,
            PostedBy    = req.PostedBy,
            Reference   = $"Impairment: {req.Reason}",
            RunningNBV  = asset.NetBookValue
        };
        _db.AssetDepreciations.Add(entry);
        await _db.SaveChangesAsync(ct);
        return ToDepreciationDto(entry);
    }

    // ── Projection helpers ────────────────────────────────────────────────────

    private static FixedAssetSummaryDto ToSummary(FixedAsset a) => new(
        a.Id, a.AssetCode, a.AssetName,
        a.Category.ToString(), a.Status.ToString(),
        a.AcquisitionDate, a.AcquisitionCost,
        a.AccumulatedDepreciation, a.NetBookValue,
        a.DepreciationMethod.ToString(), a.UsefulLifeYears,
        a.Location, a.LastDepreciationDate);

    private static FixedAssetDetailDto ToDetail(FixedAsset a) => new(
        a.Id, a.AssetCode, a.AssetName, a.Description,
        a.Category.ToString(), a.Status.ToString(),
        a.AcquisitionDate, a.AcquisitionCost,
        a.PurchaseOrderRef, a.Supplier, a.SerialNumber, a.Location,
        a.GLAssetAccountId, a.GLAccumulatedDepreciationAccountId, a.GLDepreciationExpenseAccountId,
        a.DepreciationMethod.ToString(), a.UsefulLifeYears, a.SalvageValue,
        a.DepreciationStartDate, a.AccumulatedDepreciation, a.NetBookValue,
        a.DepreciableBase, a.IsFullyDepreciated, a.LastDepreciationDate,
        a.TotalEstimatedUnits, a.UnitsProducedToDate,
        a.DepreciationEntries.Select(ToDepreciationDto).ToList(),
        a.MaintenanceRecords.Select(ToMaintenanceDto).ToList());

    private static AssetDepreciationDto ToDepreciationDto(AssetDepreciation d) => new(
        d.Id, d.AssetId, d.Date, d.Amount, d.RunningNBV, d.PostedBy, d.Reference, d.JournalEntryId);

    private static AssetDisposalDto ToDisposalDto(AssetDisposal d, FixedAsset a) => new(
        d.Id, d.AssetId, a.AssetCode, a.AssetName,
        d.DisposalDate, d.DisposalType, d.DisposalProceeds,
        d.NetBookValueAtDisposal, d.GainLoss, d.DisposedBy,
        d.Reason, d.BuyerName, d.GLGainLossAccountId, d.JournalEntryId);

    private static AssetTransferDto ToTransferDto(AssetTransfer t, FixedAsset a) => new(
        t.Id, t.AssetId, a.AssetCode, a.AssetName,
        t.TransferDate, t.FromLocation, t.ToLocation,
        t.FromDepartment, t.ToDepartment, t.TransferredBy, t.Notes);

    private static AssetMaintenanceDto ToMaintenanceDto(AssetMaintenance m) => new(
        m.Id, m.AssetId, m.MaintenanceDate, m.MaintenanceType,
        m.Cost, m.CapitalizeCost, m.Vendor, m.Description,
        m.PerformedBy, m.NextMaintenanceDue);
}
