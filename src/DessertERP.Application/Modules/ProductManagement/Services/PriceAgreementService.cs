using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.ProductManagement.Services;

// ── DTOs ─────────────────────────────────────────────────────────────────────

public record PriceAgreementDto(
    Guid   Id,
    string Name,
    string? Notes,
    string Level,            // "Product" | "Variant"
    Guid?  ProductId,  string? ProductName,
    Guid?  VariantId,  string? VariantSku,
    string PriceType,        // "SalesPrice" | "Cost"
    decimal Value,
    string Currency,
    DateTime StartDate,
    DateTime EndDate,
    bool   IsActive,
    int    Priority,
    DateTime CreatedAt);

public record CreatePriceAgreementRequest(
    string  Name,
    string  Level,       // "Product" | "Variant"
    string  PriceType,   // "SalesPrice" | "Cost"
    decimal Value,
    string  Currency   = "USD",
    Guid?   ProductId  = null,
    Guid?   VariantId  = null,
    DateTime? StartDate = null,
    DateTime? EndDate   = null,  // defaults to 2159-12-31
    string? Notes = null);

public record UpdatePriceAgreementRequest(
    string  Name,
    string  PriceType,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    bool    IsActive,
    string? Notes);

/// <summary>
/// Effective pricing for a variant — shows the agreed Sales Price and Cost
/// (whichever agreement won: Variant-level beats Product-level).
/// </summary>
public record EffectivePriceResult(
    // Resolved values (null = no agreement, use product base)
    decimal? AgreedSalesPrice,
    decimal? AgreedCost,
    // Base values from the product/variant
    decimal BasePrice,
    decimal BaseCost,
    // Which agreement drove each value
    string? SalesPriceAgreementName,
    string? SalesPriceLevel,
    string? CostAgreementName,
    string? CostLevel);

/// <summary>
/// Variant suggestion — shown in the bulk-set UI so users can see
/// existing agreed prices and update them quickly.
/// </summary>
public record VariantPriceSuggestion(
    Guid    VariantId,
    string  VariantSku,
    string  ProductName,
    string? Size,
    string? Color,
    string? Material,
    decimal CurrentBasePrice,
    decimal CurrentBaseCost,
    decimal? AgreedSalesPrice,
    decimal? AgreedCost,
    string?  SalesPriceAgreementName,
    string?  CostAgreementName);

// ── Interface ─────────────────────────────────────────────────────────────────

public interface IPriceAgreementService
{
    Task<IEnumerable<PriceAgreementDto>> GetAgreementsAsync(
        Guid? productId = null, bool activeOnly = false,
        CancellationToken ct = default);

    Task<PriceAgreementDto> CreateAsync(Guid orgId, CreatePriceAgreementRequest req,
        CancellationToken ct = default);

    Task<PriceAgreementDto> UpdateAsync(Guid orgId, Guid id, UpdatePriceAgreementRequest req,
        CancellationToken ct = default);

    Task DeleteAsync(Guid orgId, Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the agreed Sales Price and Cost for a variant,
    /// showing which agreement level won for each.
    /// </summary>
    Task<EffectivePriceResult> GetEffectivePriceAsync(Guid orgId, Guid variantId,
        CancellationToken ct = default);

    /// <summary>
    /// Bulk-set Sales Price or Cost for all SKUs of a product in one shot.
    /// Creates or updates one Variant-level agreement per SKU.
    /// </summary>
    Task<int> BulkApplyToProductAsync(Guid orgId, Guid productId,
        CreatePriceAgreementRequest req, CancellationToken ct = default);

    /// <summary>
    /// Returns all variants of a product with their current base values
    /// and any existing agreements — used to pre-fill the bulk-set form.
    /// </summary>
    Task<IEnumerable<VariantPriceSuggestion>> GetVariantSuggestionsAsync(
        Guid orgId, Guid productId, CancellationToken ct = default);
}

// ── Implementation ────────────────────────────────────────────────────────────

public class PriceAgreementService(IAppDbContext _db) : IPriceAgreementService
{
    private static readonly DateTime FarFuture = new(2159, 12, 31);

    public async Task<IEnumerable<PriceAgreementDto>> GetAgreementsAsync(
        Guid? productId = null, bool activeOnly = false, CancellationToken ct = default)
    {
        var q = _db.PriceAgreements
            .Include(a => a.Product)
            .Include(a => a.Variant)
            .AsQueryable();

        if (productId.HasValue)
            q = q.Where(a => a.ProductId == productId.Value
                          || a.Variant!.ProductId == productId.Value);
        if (activeOnly)
            q = q.Where(a => a.IsActive);

        var list = await q.OrderByDescending(a => a.Priority)
                          .ThenBy(a => a.Name)
                          .ToListAsync(ct);
        return list.Select(ToDto);
    }

    public async Task<PriceAgreementDto> CreateAsync(Guid orgId,
        CreatePriceAgreementRequest req, CancellationToken ct = default)
    {
        if (!Enum.TryParse<AgreementLevel>(req.Level, true, out var level))
            throw new ArgumentException($"Invalid level '{req.Level}'. Use 'Product' or 'Variant'.");
        if (!Enum.TryParse<AgreementPriceType>(req.PriceType, true, out var priceType))
            throw new ArgumentException($"Invalid price type '{req.PriceType}'. Use 'SalesPrice' or 'Cost'.");
        if (req.Value < 0)
            throw new ArgumentException("Value must be zero or greater.");

        var agreement = new PriceAgreement(
            orgId, req.Name, level, priceType, req.Value, req.Currency,
            req.StartDate ?? DateTime.UtcNow.Date,
            req.EndDate ?? FarFuture,
            req.ProductId, req.VariantId, req.Notes);

        _db.PriceAgreements.Add(agreement);
        await _db.SaveChangesAsync(ct);
        return await ReloadDto(agreement.Id, ct);
    }

    public async Task<PriceAgreementDto> UpdateAsync(Guid orgId, Guid id,
        UpdatePriceAgreementRequest req, CancellationToken ct = default)
    {
        var a = await _db.PriceAgreements.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException("Price agreement not found.");

        if (!Enum.TryParse<AgreementPriceType>(req.PriceType, true, out var priceType))
            throw new ArgumentException($"Invalid price type '{req.PriceType}'.");

        a.Update(req.Name, priceType, req.Value, req.StartDate, req.EndDate, req.IsActive, req.Notes);
        await _db.SaveChangesAsync(ct);
        return await ReloadDto(id, ct);
    }

    public async Task DeleteAsync(Guid orgId, Guid id, CancellationToken ct = default)
    {
        var a = await _db.PriceAgreements.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException("Price agreement not found.");
        _db.PriceAgreements.Remove(a);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<EffectivePriceResult> GetEffectivePriceAsync(
        Guid orgId, Guid variantId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;

        var variant = await _db.ProductVariants
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == variantId, ct)
            ?? throw new KeyNotFoundException("Variant not found.");

        var basePrice = variant.EffectivePrice(variant.Product!.BasePrice);
        var baseCost  = variant.CostOverride ?? variant.Product.BaseCost;

        // Load all active agreements for this variant or its parent product
        var agreements = await _db.PriceAgreements
            .Where(a => a.IsActive && a.StartDate <= today && a.EndDate >= today
                && (a.VariantId == variantId || a.ProductId == variant.ProductId))
            .OrderByDescending(a => a.Priority)   // Variant (20) beats Product (10)
            .ToListAsync(ct);

        // Find best SalesPrice and Cost agreements independently
        var bestSP   = agreements.FirstOrDefault(a => a.PriceType == AgreementPriceType.SalesPrice);
        var bestCost = agreements.FirstOrDefault(a => a.PriceType == AgreementPriceType.Cost);

        return new EffectivePriceResult(
            AgreedSalesPrice: bestSP?.ResolveValue(),
            AgreedCost:       bestCost?.ResolveValue(),
            BasePrice:        basePrice,
            BaseCost:         baseCost,
            SalesPriceAgreementName: bestSP?.Name,
            SalesPriceLevel:         bestSP?.Level.ToString(),
            CostAgreementName:       bestCost?.Name,
            CostLevel:               bestCost?.Level.ToString());
    }

    public async Task<int> BulkApplyToProductAsync(Guid orgId, Guid productId,
        CreatePriceAgreementRequest req, CancellationToken ct = default)
    {
        if (!Enum.TryParse<AgreementPriceType>(req.PriceType, true, out var priceType))
            throw new ArgumentException($"Invalid price type '{req.PriceType}'.");

        var startDate = req.StartDate ?? DateTime.UtcNow.Date;
        var endDate   = req.EndDate   ?? FarFuture;

        var variants = await _db.ProductVariants
            .Where(v => v.ProductId == productId && !v.IsDeleted)
            .ToListAsync(ct);

        foreach (var v in variants)
        {
            // Update existing Variant-level agreement of this price type, or create new
            var existing = await _db.PriceAgreements
                .FirstOrDefaultAsync(a => a.VariantId == v.Id
                    && a.PriceType == priceType, ct);

            if (existing != null)
                existing.Update(req.Name, priceType, req.Value, startDate, endDate, true, req.Notes);
            else
                _db.PriceAgreements.Add(new PriceAgreement(
                    orgId, req.Name, AgreementLevel.Variant, priceType,
                    req.Value, req.Currency, startDate, endDate,
                    variantId: v.Id, notes: req.Notes));
        }

        await _db.SaveChangesAsync(ct);
        return variants.Count;
    }

    public async Task<IEnumerable<VariantPriceSuggestion>> GetVariantSuggestionsAsync(
        Guid orgId, Guid productId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;

        var variants = await _db.ProductVariants
            .Include(v => v.Product)
            .Where(v => v.ProductId == productId && !v.IsDeleted)
            .OrderBy(v => v.Sku)
            .ToListAsync(ct);

        var variantIds = variants.Select(v => v.Id).ToList();

        var agreements = await _db.PriceAgreements
            .Where(a => a.IsActive && a.StartDate <= today && a.EndDate >= today
                && (variantIds.Contains(a.VariantId ?? Guid.Empty)
                    || a.ProductId == productId))
            .OrderByDescending(a => a.Priority)
            .ToListAsync(ct);

        return variants.Select(v =>
        {
            var basePrice = v.EffectivePrice(v.Product!.BasePrice);
            var baseCost  = v.CostOverride ?? v.Product.BaseCost;

            var variantAgreements = agreements
                .Where(a => a.VariantId == v.Id || a.ProductId == v.ProductId)
                .OrderByDescending(a => a.Priority)
                .ToList();

            var bestSP   = variantAgreements.FirstOrDefault(a => a.PriceType == AgreementPriceType.SalesPrice);
            var bestCost = variantAgreements.FirstOrDefault(a => a.PriceType == AgreementPriceType.Cost);

            return new VariantPriceSuggestion(
                v.Id, v.Sku, v.Product!.Name,
                v.Size, v.Color, v.Material,
                basePrice, baseCost,
                bestSP?.ResolveValue(), bestCost?.ResolveValue(),
                bestSP?.Name, bestCost?.Name);
        });
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<PriceAgreementDto> ReloadDto(Guid id, CancellationToken ct)
    {
        var a = await _db.PriceAgreements
            .Include(x => x.Product)
            .Include(x => x.Variant)
            .FirstAsync(x => x.Id == id, ct);
        return ToDto(a);
    }

    private static PriceAgreementDto ToDto(PriceAgreement a) => new(
        a.Id, a.Name, a.Notes,
        a.Level.ToString(),
        a.ProductId, a.Product?.Name,
        a.VariantId, a.Variant?.Sku,
        a.PriceType.ToString(), a.Value, a.Currency,
        a.StartDate, a.EndDate, a.IsActive, a.Priority, a.CreatedAt);
}
