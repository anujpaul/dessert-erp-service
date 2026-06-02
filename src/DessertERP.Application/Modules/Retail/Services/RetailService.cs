using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.Retail.DTOs;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.Retail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DessertERP.Application.Modules.Retail.Services;

public interface IRetailService
{
    // Stores
    Task<List<RetailStoreDto>> GetStoresAsync(Guid orgId, CancellationToken ct = default);
    Task<RetailStoreDto?> GetStoreAsync(Guid storeId, CancellationToken ct = default);
    Task<RetailStoreDto> CreateStoreAsync(Guid orgId, CreateStoreRequest req, CancellationToken ct = default);
    Task<RetailStoreDto> UpdateStoreAsync(Guid storeId, UpdateStoreRequest req, CancellationToken ct = default);
    Task ToggleStoreAsync(Guid storeId, bool active, CancellationToken ct = default);

    // POS Transactions
    Task<List<POSTransactionSummaryDto>> GetTransactionsAsync(Guid orgId, int page = 1, int pageSize = 50, string? status = null, CancellationToken ct = default);
    Task<POSTransactionDto?> GetTransactionAsync(Guid txId, CancellationToken ct = default);
    Task<POSTransactionDto> CreateTransactionAsync(Guid orgId, CreatePOSTransactionRequest req, CancellationToken ct = default);
    Task<POSTransactionDto> CreateOnlineOrderAsync(Guid orgId, OnlineOrderRequest req, CancellationToken ct = default);
    Task ProcessTransactionAsync(Guid txId, CancellationToken ct = default);
    Task VoidTransactionAsync(Guid txId, CancellationToken ct = default);
    Task UpdateFulfillmentStatusAsync(Guid txId, string fulfillmentStatus, CancellationToken ct = default);

    // Promotions
    Task<List<PromotionDto>> GetPromotionsAsync(Guid orgId, CancellationToken ct = default);
    Task<PromotionDto?> GetPromotionAsync(Guid promoId, CancellationToken ct = default);
    Task<PromotionDto> CreatePromotionAsync(Guid orgId, CreatePromotionRequest req, CancellationToken ct = default);
    Task<PromotionDto> UpdatePromotionAsync(Guid promoId, CreatePromotionRequest req, CancellationToken ct = default);
    Task TogglePromotionAsync(Guid promoId, bool active, CancellationToken ct = default);

    // Coupons
    Task<List<CouponDto>> GetCouponsAsync(Guid orgId, Guid? promotionId = null, CancellationToken ct = default);
    Task<CouponDto> CreateCouponAsync(Guid orgId, CreateCouponRequest req, CancellationToken ct = default);
    Task<List<CouponDto>> BulkCreateCouponsAsync(Guid orgId, BulkCreateCouponsRequest req, CancellationToken ct = default);
    Task<CouponValidationResult> ValidateCouponAsync(Guid orgId, ValidateCouponRequest req, CancellationToken ct = default);
    Task DeactivateCouponAsync(Guid couponId, CancellationToken ct = default);

    // Reports
    Task<RetailSummaryDto> GetSummaryAsync(Guid orgId, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
}

public class RetailService : IRetailService
{
    private readonly IAppDbContext _db;
    private readonly ILogger<RetailService> _logger;

    public RetailService(IAppDbContext db, ILogger<RetailService> logger)
    {
        _db     = db;
        _logger = logger;
    }

    // ── Stores ────────────────────────────────────────────────────────────────

    public async Task<List<RetailStoreDto>> GetStoresAsync(Guid orgId, CancellationToken ct = default)
    {
        var stores = await _db.RetailStores
            .Where(s => s.OrganizationId == orgId)
            .OrderBy(s => s.StoreCode)
            .ToListAsync(ct);
        return stores.Select(StoreToDto).ToList();
    }

    public async Task<RetailStoreDto?> GetStoreAsync(Guid storeId, CancellationToken ct = default)
    {
        var s = await _db.RetailStores.FirstOrDefaultAsync(x => x.Id == storeId, ct);
        return s is null ? null : StoreToDto(s);
    }

    public async Task<RetailStoreDto> CreateStoreAsync(Guid orgId, CreateStoreRequest req, CancellationToken ct = default)
    {
        var store = new RetailStore(orgId, req.StoreCode, req.Name, req.Address, req.Phone, req.ManagerName);
        _db.RetailStores.Add(store);
        await _db.SaveChangesAsync(ct);
        return StoreToDto(store);
    }

    public async Task<RetailStoreDto> UpdateStoreAsync(Guid storeId, UpdateStoreRequest req, CancellationToken ct = default)
    {
        var store = await _db.RetailStores.FirstOrDefaultAsync(x => x.Id == storeId, ct)
            ?? throw new InvalidOperationException("Store not found.");
        store.Update(req.Name, req.Address, req.Phone, req.ManagerName);
        await _db.SaveChangesAsync(ct);
        return StoreToDto(store);
    }

    public async Task ToggleStoreAsync(Guid storeId, bool active, CancellationToken ct = default)
    {
        var store = await _db.RetailStores.FirstOrDefaultAsync(x => x.Id == storeId, ct)
            ?? throw new InvalidOperationException("Store not found.");
        if (active) store.Activate(); else store.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── POS Transactions ──────────────────────────────────────────────────────

    public async Task<List<POSTransactionSummaryDto>> GetTransactionsAsync(Guid orgId,
        int page = 1, int pageSize = 50, string? status = null, CancellationToken ct = default)
    {
        var q = _db.POSTransactions
            .Include(t => t.Lines)
            .Where(t => t.OrganizationId == orgId);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<POSTransactionStatus>(status, true, out var s))
            q = q.Where(t => t.Status == s);

        var txs = await q
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

        var storeIds = txs.Select(t => t.StoreId).Distinct().ToList();
        var stores   = await _db.RetailStores.Where(s => storeIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id, s => s.Name, ct);

        return txs.Select(t => new POSTransactionSummaryDto(
            t.Id, t.TransactionNumber,
            stores.GetValueOrDefault(t.StoreId, "Unknown"),
            t.ExternalRef, t.TransactionDate, t.TransactionType.ToString(),
            t.Status.ToString(), t.Channel.ToString(), t.FulfillmentStatus.ToString(),
            t.GrandTotal, t.Lines.Count,
            t.CouponCode, t.CustomerName, t.ExternalOrderRef,
            t.ARInvoiceId, t.CreatedAt)).ToList();
    }

    public async Task<POSTransactionDto?> GetTransactionAsync(Guid txId, CancellationToken ct = default)
    {
        var tx = await _db.POSTransactions
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .FirstOrDefaultAsync(t => t.Id == txId, ct);
        if (tx is null) return null;

        var store = await _db.RetailStores.FirstOrDefaultAsync(s => s.Id == tx.StoreId, ct);
        return TxToDto(tx, store?.Name ?? "Unknown");
    }

    public async Task<POSTransactionDto> CreateTransactionAsync(Guid orgId, CreatePOSTransactionRequest req, CancellationToken ct = default)
    {
        var store = await _db.RetailStores.FirstOrDefaultAsync(s => s.Id == req.StoreId, ct)
            ?? throw new InvalidOperationException("Store not found.");

        var txType  = Enum.Parse<POSTransactionType>(req.TransactionType, true);
        var channel = Enum.Parse<SalesChannel>(req.Channel, true);

        var tx = new POSTransaction(orgId, req.StoreId, req.TransactionNumber,
            req.TransactionDate, txType, req.ExternalRef, req.CashierId, req.CashierName,
            req.Currency, req.CouponCode, req.SourceFile,
            channel, req.CustomerName, req.CustomerEmail, req.CustomerPhone,
            req.DeliveryAddress, req.ExternalOrderRef, req.ChannelNotes);

        _db.POSTransactions.Add(tx);
        await _db.SaveChangesAsync(ct); // save to get tx.Id

        // Add lines
        foreach (var l in req.Lines)
        {
            var line = new POSTransactionLine(tx.Id, l.Sku, l.ProductName,
                l.Quantity, l.UnitPrice, l.DiscountPct, l.TaxRate,
                l.ProductVariantId, l.UnitOfMeasure, l.IsReturn);
            _db.POSTransactionLines.Add(line);
        }

        // Add payments
        foreach (var p in req.Payments)
        {
            var method  = Enum.Parse<POSPaymentMethod>(p.PaymentMethod, true);
            var payment = new POSPayment(tx.Id, method, p.Amount, p.Reference);
            _db.POSPayments.Add(payment);
        }

        await _db.SaveChangesAsync(ct);

        // Apply coupon if provided
        if (!string.IsNullOrEmpty(req.CouponCode))
        {
            var couponResult = await ValidateCouponAsync(orgId,
                new ValidateCouponRequest(req.CouponCode, tx.SubTotal), ct);
            if (couponResult.IsValid)
                tx.ApplyCouponDiscount(couponResult.DiscountAmount);
        }

        tx.RecalculateTotals();
        await _db.SaveChangesAsync(ct);

        // Auto-process: create AR Invoice + update inventory + post GL
        await ProcessTransactionAsync(tx.Id, ct);

        // Reload with lines + payments
        var saved = await _db.POSTransactions
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .FirstAsync(t => t.Id == tx.Id, ct);

        return TxToDto(saved, store.Name);
    }

    public async Task ProcessTransactionAsync(Guid txId, CancellationToken ct = default)
    {
        var tx = await _db.POSTransactions
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .FirstOrDefaultAsync(t => t.Id == txId, ct)
            ?? throw new InvalidOperationException("Transaction not found.");

        if (tx.Status != POSTransactionStatus.Pending)
            return;

        try
        {
            // 1. Find or create a "Walk-in / Retail" customer
            var org = await _db.Organizations.FirstOrDefaultAsync(ct)
                ?? throw new InvalidOperationException("No organization found.");

            var retailCustomer = await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerNumber == "RETAIL-WALKIN", ct);

            if (retailCustomer is null)
            {
                retailCustomer = new Customer(tx.OrganizationId,
                    "RETAIL-WALKIN", "Walk-in Customer", paymentTermsDays: 0);
                _db.Customers.Add(retailCustomer);
                await _db.SaveChangesAsync(ct);
            }

            // 2. Create AR Invoice
            var invoice = new ARInvoice(
                tx.OrganizationId,
                GenerateInvoiceNumber(),
                retailCustomer.Id,
                tx.TransactionDate,
                tx.TransactionDate,           // due immediately (retail sale)
                $"POS Sale – {tx.TransactionNumber}",
                tx.SubTotal,
                tx.TaxTotal,
                tx.DiscountTotal,
                null);                        // no linked sales order

            _db.ARInvoices.Add(invoice);

            // Issue and mark as paid immediately (cash/card retail sale)
            invoice.Issue();
            invoice.ApplyPayment(tx.GrandTotal);

            // 3. Update inventory for each line
            foreach (var line in tx.Lines.Where(l => l.ProductVariantId.HasValue))
            {
                var inv = await _db.InventoryRecords
                    .FirstOrDefaultAsync(i => i.ProductVariantId == line.ProductVariantId, ct);
                if (inv != null)
                {
                    var delta  = line.IsReturn ? line.Quantity : -line.Quantity;
                    var reason = line.IsReturn ? "POS Return" : "POS Sale";
                    inv.AdjustQuantity(delta, reason);
                }
            }

            // 4. Redeem coupon if used
            if (!string.IsNullOrEmpty(tx.CouponCode))
            {
                var coupon = await _db.Coupons
                    .FirstOrDefaultAsync(c => c.Code == tx.CouponCode && c.OrganizationId == tx.OrganizationId, ct);
                if (coupon != null)
                {
                    coupon.Redeem();
                    var redemption = new CouponRedemption(tx.OrganizationId, coupon.Id, tx.Id, tx.CouponDiscount);
                    _db.CouponRedemptions.Add(redemption);

                    var promo = await _db.Promotions.FindAsync(new object[] { coupon.PromotionId }, ct);
                    promo?.IncrementUsage();
                }
            }

            await _db.SaveChangesAsync(ct);

            tx.MarkProcessed(invoice.Id);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("POS Transaction {TxNum} processed → Invoice {InvId}",
                tx.TransactionNumber, invoice.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process POS transaction {TxId}", txId);
            tx.MarkFailed(ex.Message);
            await _db.SaveChangesAsync(ct);
            throw;
        }
    }

    public async Task<POSTransactionDto> CreateOnlineOrderAsync(Guid orgId, OnlineOrderRequest req, CancellationToken ct = default)
    {
        var store = await _db.RetailStores.FirstOrDefaultAsync(s => s.Id == req.StoreId, ct)
            ?? throw new InvalidOperationException("Store not found.");

        var channel = Enum.Parse<SalesChannel>(req.Channel, true);
        var txNumber = $"ONL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..5].ToUpperInvariant()}";

        var tx = new POSTransaction(orgId, req.StoreId, txNumber,
            DateTime.UtcNow, POSTransactionType.Sale, null, null, null,
            req.Currency, req.CouponCode, null,
            channel, req.CustomerName, req.CustomerEmail, req.CustomerPhone,
            req.DeliveryAddress, null, req.ChannelNotes);

        _db.POSTransactions.Add(tx);
        await _db.SaveChangesAsync(ct);

        foreach (var l in req.Lines)
        {
            var line = new POSTransactionLine(tx.Id, l.Sku, l.ProductName,
                l.Quantity, l.UnitPrice, l.DiscountPct, l.TaxRate,
                l.ProductVariantId, l.UnitOfMeasure, false);
            _db.POSTransactionLines.Add(line);
        }

        await _db.SaveChangesAsync(ct);

        if (!string.IsNullOrEmpty(req.CouponCode))
        {
            var couponResult = await ValidateCouponAsync(orgId,
                new ValidateCouponRequest(req.CouponCode, tx.SubTotal), ct);
            if (couponResult.IsValid)
                tx.ApplyCouponDiscount(couponResult.DiscountAmount);
        }

        tx.RecalculateTotals();
        await _db.SaveChangesAsync(ct);

        var saved = await _db.POSTransactions
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .FirstAsync(t => t.Id == tx.Id, ct);

        return TxToDto(saved, store.Name);
    }

    public async Task UpdateFulfillmentStatusAsync(Guid txId, string fulfillmentStatus, CancellationToken ct = default)
    {
        var tx = await _db.POSTransactions.FirstOrDefaultAsync(t => t.Id == txId, ct)
            ?? throw new InvalidOperationException("Transaction not found.");
        var status = Enum.Parse<FulfillmentStatus>(fulfillmentStatus, true);
        tx.UpdateFulfillmentStatus(status);
        await _db.SaveChangesAsync(ct);
    }

    public async Task VoidTransactionAsync(Guid txId, CancellationToken ct = default)
    {
        var tx = await _db.POSTransactions.FirstOrDefaultAsync(t => t.Id == txId, ct)
            ?? throw new InvalidOperationException("Transaction not found.");
        tx.Void();
        await _db.SaveChangesAsync(ct);
    }

    // ── Promotions ────────────────────────────────────────────────────────────

    public async Task<List<PromotionDto>> GetPromotionsAsync(Guid orgId, CancellationToken ct = default)
    {
        var promos = await _db.Promotions
            .Where(p => p.OrganizationId == orgId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync(ct);
        return promos.Select(PromoToDto).ToList();
    }

    public async Task<PromotionDto?> GetPromotionAsync(Guid promoId, CancellationToken ct = default)
    {
        var p = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == promoId, ct);
        return p is null ? null : PromoToDto(p);
    }

    public async Task<PromotionDto> CreatePromotionAsync(Guid orgId, CreatePromotionRequest req, CancellationToken ct = default)
    {
        var discountType = Enum.Parse<DiscountType>(req.DiscountType, true);
        var promo = new Promotion(orgId, req.Name, discountType, req.DiscountValue,
            req.StartDate, req.EndDate, req.Description,
            req.MinimumOrderAmount, req.MaxUsesTotal, req.MaxUsesPerCustomer,
            req.BuyQuantity, req.GetQuantity, req.ApplyToAllProducts, req.ApplicableSkus);
        _db.Promotions.Add(promo);
        await _db.SaveChangesAsync(ct);
        return PromoToDto(promo);
    }

    public async Task<PromotionDto> UpdatePromotionAsync(Guid promoId, CreatePromotionRequest req, CancellationToken ct = default)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(p => p.Id == promoId, ct)
            ?? throw new InvalidOperationException("Promotion not found.");
        promo.Update(req.Name, req.Description, req.DiscountValue,
            req.StartDate, req.EndDate, req.MinimumOrderAmount,
            req.MaxUsesTotal, req.MaxUsesPerCustomer,
            req.ApplyToAllProducts, req.ApplicableSkus);
        await _db.SaveChangesAsync(ct);
        return PromoToDto(promo);
    }

    public async Task TogglePromotionAsync(Guid promoId, bool active, CancellationToken ct = default)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(p => p.Id == promoId, ct)
            ?? throw new InvalidOperationException("Promotion not found.");
        if (active) promo.Activate(); else promo.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── Coupons ───────────────────────────────────────────────────────────────

    public async Task<List<CouponDto>> GetCouponsAsync(Guid orgId, Guid? promotionId = null, CancellationToken ct = default)
    {
        var q = _db.Coupons.Include(c => c.Promotion)
            .Where(c => c.OrganizationId == orgId);
        if (promotionId.HasValue)
            q = q.Where(c => c.PromotionId == promotionId.Value);
        var coupons = await q.OrderBy(c => c.Code).ToListAsync(ct);
        return coupons.Select(c => CouponToDto(c)).ToList();
    }

    public async Task<CouponDto> CreateCouponAsync(Guid orgId, CreateCouponRequest req, CancellationToken ct = default)
    {
        if (await _db.Coupons.AnyAsync(c => c.Code == req.Code.ToUpperInvariant() && c.OrganizationId == orgId, ct))
            throw new InvalidOperationException($"Coupon code '{req.Code}' already exists.");

        var coupon = new Coupon(orgId, req.PromotionId, req.Code, req.MaxUses, req.ExpiresAt);
        _db.Coupons.Add(coupon);
        await _db.SaveChangesAsync(ct);

        var promo = await _db.Promotions.FindAsync(new object[] { req.PromotionId }, ct);
        return CouponToDto(coupon, promo?.Name);
    }

    public async Task<List<CouponDto>> BulkCreateCouponsAsync(Guid orgId, BulkCreateCouponsRequest req, CancellationToken ct = default)
    {
        var promo = await _db.Promotions.FindAsync(new object[] { req.PromotionId }, ct)
            ?? throw new InvalidOperationException("Promotion not found.");

        var coupons = new List<Coupon>();
        for (int i = 0; i < req.Count; i++)
        {
            var code = $"{req.Prefix}{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
            var coupon = new Coupon(orgId, req.PromotionId, code, req.MaxUsesEach, req.ExpiresAt);
            coupons.Add(coupon);
            _db.Coupons.Add(coupon);
        }
        await _db.SaveChangesAsync(ct);
        return coupons.Select(c => CouponToDto(c, promo.Name)).ToList();
    }

    public async Task<CouponValidationResult> ValidateCouponAsync(Guid orgId, ValidateCouponRequest req, CancellationToken ct = default)
    {
        var code   = req.Code.Trim().ToUpperInvariant();
        var coupon = await _db.Coupons
            .Include(c => c.Promotion)
            .FirstOrDefaultAsync(c => c.Code == code && c.OrganizationId == orgId, ct);

        if (coupon is null)
            return new CouponValidationResult(false, "Coupon code not found.",
                null, null, 0, 0, null, 0);

        var now = DateTime.UtcNow;

        if (!coupon.IsValid(now))
            return new CouponValidationResult(false,
                coupon.UsedCount >= coupon.MaxUses ? "Coupon has reached its maximum uses." : "Coupon is expired or inactive.",
                null, null, 0, 0, coupon.ExpiresAt, 0);

        var promo = coupon.Promotion;
        if (promo is null || !promo.IsValid(req.OrderAmount, now))
            return new CouponValidationResult(false,
                $"Promotion requires a minimum order of {promo?.MinimumOrderAmount:C}.",
                null, null, 0, 0, coupon.ExpiresAt, 0);

        var discountAmount = promo.CalculateDiscount(req.OrderAmount);
        var remaining      = coupon.MaxUses == 0 ? int.MaxValue : coupon.MaxUses - coupon.UsedCount;

        return new CouponValidationResult(true, "Coupon is valid.",
            promo.Name, promo.DiscountType.ToString(),
            promo.DiscountValue, discountAmount,
            coupon.ExpiresAt, remaining);
    }

    public async Task DeactivateCouponAsync(Guid couponId, CancellationToken ct = default)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.Id == couponId, ct)
            ?? throw new InvalidOperationException("Coupon not found.");
        coupon.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    public async Task<RetailSummaryDto> GetSummaryAsync(Guid orgId,
        DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var q = _db.POSTransactions
            .Include(t => t.Lines)
            .Where(t => t.OrganizationId == orgId && t.Status != POSTransactionStatus.Voided);

        if (from.HasValue) q = q.Where(t => t.TransactionDate >= from.Value);
        if (to.HasValue)   q = q.Where(t => t.TransactionDate <= to.Value);

        var txs = await q.ToListAsync(ct);

        var storeRevenue = txs
            .GroupBy(t => t.StoreId)
            .OrderByDescending(g => g.Sum(t => t.GrandTotal))
            .FirstOrDefault();

        string topStore = "N/A";
        if (storeRevenue != null)
        {
            var store = await _db.RetailStores.FindAsync(new object[] { storeRevenue.Key }, ct);
            topStore  = store?.Name ?? "Unknown";
        }

        return new RetailSummaryDto(
            TotalTransactions:     txs.Count,
            ProcessedTransactions: txs.Count(t => t.Status == POSTransactionStatus.Processed),
            FailedTransactions:    txs.Count(t => t.Status == POSTransactionStatus.Failed),
            TotalRevenue:          txs.Sum(t => t.GrandTotal),
            TotalDiscounts:        txs.Sum(t => t.DiscountTotal),
            TotalTax:              txs.Sum(t => t.TaxTotal),
            TotalItemsSold:        txs.SelectMany(t => t.Lines).Sum(l => (int)l.Quantity),
            TopStore:              topStore);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string GenerateInvoiceNumber() =>
        $"RET-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

    private static RetailStoreDto StoreToDto(RetailStore s) =>
        new(s.Id, s.StoreCode, s.Name, s.Address, s.Phone, s.ManagerName, s.IsActive, s.CreatedAt);

    private static PromotionDto PromoToDto(Promotion p) =>
        new(p.Id, p.Name, p.Description, p.DiscountType.ToString(), p.Status.ToString(),
            p.DiscountValue, p.BuyQuantity, p.GetQuantity,
            p.MinimumOrderAmount, p.MaxUsesTotal, p.MaxUsesPerCustomer, p.UsedCount,
            p.StartDate, p.EndDate, p.ApplyToAllProducts, p.ApplicableSkus, p.CreatedAt);

    private static CouponDto CouponToDto(Coupon c, string? promoName = null)
    {
        promoName ??= c.Promotion?.Name ?? "Unknown";
        var remaining = c.MaxUses == 0 ? int.MaxValue : c.MaxUses - c.UsedCount;
        return new(c.Id, c.PromotionId, promoName, c.Code, c.IsActive,
            c.MaxUses, c.UsedCount, remaining, c.ExpiresAt, c.CreatedAt);
    }

    private static POSTransactionDto TxToDto(POSTransaction t, string storeName) =>
        new(t.Id, t.TransactionNumber, t.StoreId, storeName, t.ExternalRef,
            t.CashierId, t.CashierName, t.TransactionDate,
            t.TransactionType.ToString(), t.Status.ToString(),
            t.Channel.ToString(), t.FulfillmentStatus.ToString(),
            t.CustomerName, t.CustomerEmail, t.CustomerPhone,
            t.DeliveryAddress, t.ExternalOrderRef, t.ChannelNotes,
            t.Currency,
            t.SubTotal, t.DiscountTotal, t.TaxTotal, t.GrandTotal,
            t.TenderedAmount, t.ChangeAmount, t.CouponCode, t.CouponDiscount,
            t.ARInvoiceId, t.JournalEntryId, t.ProcessingError, t.SourceFile,
            t.CreatedAt,
            t.Lines.Select(l => new POSTransactionLineDto(l.Id, l.ProductVariantId, l.Sku,
                l.ProductName, l.UnitOfMeasure, l.Quantity, l.UnitPrice,
                l.DiscountPct, l.DiscountAmount, l.TaxRate,
                l.LineSubTotal, l.TaxAmount, l.LineTotal, l.IsReturn)).ToList(),
            t.Payments.Select(p => new POSPaymentDto(p.Id, p.PaymentMethod.ToString(), p.Amount, p.Reference)).ToList());
}
