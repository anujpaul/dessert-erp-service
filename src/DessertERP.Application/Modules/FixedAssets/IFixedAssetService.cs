using DessertERP.Application.Modules.FixedAssets.DTOs;

namespace DessertERP.Application.Modules.FixedAssets;

public interface IFixedAssetService
{
    // ── Assets ────────────────────────────────────────────────────────────────
    Task<List<FixedAssetSummaryDto>> GetAssetsAsync(string? category, string? status, CancellationToken ct = default);
    Task<FixedAssetDetailDto?> GetAssetAsync(Guid id, CancellationToken ct = default);
    Task<FixedAssetDetailDto> CreateAssetAsync(CreateFixedAssetRequest req, CancellationToken ct = default);
    Task<FixedAssetDetailDto> UpdateAssetAsync(Guid id, UpdateFixedAssetRequest req, CancellationToken ct = default);
    Task SetAssetStatusAsync(Guid id, string status, CancellationToken ct = default);
    Task<FixedAssetSummaryStatsDto> GetSummaryStatsAsync(CancellationToken ct = default);

    // ── Depreciation ──────────────────────────────────────────────────────────
    Task<AssetDepreciationDto> RunDepreciationAsync(Guid assetId, RunDepreciationRequest req, CancellationToken ct = default);
    Task<BulkDepreciationResultDto> RunBulkDepreciationAsync(RunBulkDepreciationRequest req, CancellationToken ct = default);
    Task<List<DepreciationScheduleRowDto>> GetDepreciationScheduleAsync(Guid assetId, CancellationToken ct = default);
    Task<List<AssetDepreciationDto>> GetDepreciationHistoryAsync(Guid assetId, CancellationToken ct = default);

    // ── Disposal ──────────────────────────────────────────────────────────────
    Task<AssetDisposalDto> DisposeAssetAsync(Guid assetId, DisposeAssetRequest req, CancellationToken ct = default);
    Task<List<AssetDisposalDto>> GetDisposalsAsync(CancellationToken ct = default);
    Task<AssetDisposalDto?> GetDisposalAsync(Guid id, CancellationToken ct = default);

    // ── Transfers ─────────────────────────────────────────────────────────────
    Task<AssetTransferDto> CreateTransferAsync(CreateTransferRequest req, CancellationToken ct = default);
    Task<List<AssetTransferDto>> GetTransfersAsync(Guid? assetId, CancellationToken ct = default);

    // ── Maintenance ───────────────────────────────────────────────────────────
    Task<AssetMaintenanceDto> AddMaintenanceAsync(CreateMaintenanceRequest req, CancellationToken ct = default);
    Task<List<AssetMaintenanceDto>> GetMaintenanceAsync(Guid assetId, CancellationToken ct = default);

    // ── Impairment ────────────────────────────────────────────────────────────
    Task<AssetDepreciationDto> ImpairAssetAsync(Guid assetId, ImpairAssetRequest req, CancellationToken ct = default);
}
