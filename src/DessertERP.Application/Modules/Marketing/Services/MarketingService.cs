using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.Marketing.DTOs;
using DessertERP.Domain.Modules.Marketing;
using DessertERP.Domain.Modules.Retail;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.Marketing.Services;

public class MarketingService : IMarketingService
{
    private readonly IAppDbContext _db;

    public MarketingService(IAppDbContext db)
    {
        _db = db;
    }

    // ── Campaigns ──────────────────────────────────────────────────────────────

    public async Task<List<CampaignDto>> GetCampaignsAsync(Guid orgId)
    {
        var list = await _db.Campaigns
            .Where(c => c.OrganizationId == orgId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return list.Select(CampaignToDto).ToList();
    }

    public async Task<CampaignDto> GetCampaignAsync(Guid orgId, Guid id)
    {
        var c = await _db.Campaigns.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Campaign not found.");
        return CampaignToDto(c);
    }

    public async Task<CampaignDto> CreateCampaignAsync(Guid orgId, CreateCampaignRequest req)
    {
        var type = Enum.Parse<CampaignType>(req.Type);
        var audience = Enum.Parse<TargetAudience>(req.TargetAudience);

        var campaign = new Campaign(orgId, req.Name, req.Description,
            type, audience, req.StartDate, req.EndDate, req.Budget,
            req.LinkedPromotionId, req.Tags);

        _db.Campaigns.Add(campaign);
        await _db.SaveChangesAsync();
        return CampaignToDto(campaign);
    }

    public async Task<CampaignDto> UpdateCampaignAsync(Guid orgId, Guid id, UpdateCampaignRequest req)
    {
        var campaign = await _db.Campaigns.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Campaign not found.");

        var type = Enum.Parse<CampaignType>(req.Type);
        var audience = Enum.Parse<TargetAudience>(req.TargetAudience);

        campaign.Update(req.Name, req.Description, type, audience,
            req.StartDate, req.EndDate, req.Budget, req.LinkedPromotionId, req.Tags);

        await _db.SaveChangesAsync();
        return CampaignToDto(campaign);
    }

    public async Task<CampaignDto> SetCampaignStatusAsync(Guid orgId, Guid id, SetCampaignStatusRequest req)
    {
        var campaign = await _db.Campaigns.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Campaign not found.");

        var status = Enum.Parse<CampaignStatus>(req.Status);
        campaign.SetStatus(status);
        await _db.SaveChangesAsync();
        return CampaignToDto(campaign);
    }

    public async Task<CampaignDto> RecordMetricsAsync(Guid orgId, Guid id, RecordCampaignMetricsRequest req)
    {
        var campaign = await _db.Campaigns.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Campaign not found.");

        campaign.RecordMetrics(req.Reach, req.Conversions, req.Spend);
        await _db.SaveChangesAsync();
        return CampaignToDto(campaign);
    }

    public async Task DeleteCampaignAsync(Guid orgId, Guid id)
    {
        var campaign = await _db.Campaigns.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Campaign not found.");
        _db.Campaigns.Remove(campaign);
        await _db.SaveChangesAsync();
    }

    // ── Promotions ─────────────────────────────────────────────────────────────

    public async Task<List<PromotionDto>> GetPromotionsAsync(Guid orgId)
    {
        var list = await _db.Promotions
            .Where(p => p.OrganizationId == orgId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return list.Select(PromotionToDto).ToList();
    }

    public async Task<PromotionDto> GetPromotionAsync(Guid orgId, Guid id)
    {
        var p = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");
        return PromotionToDto(p);
    }

    public async Task<PromotionDto> CreatePromotionAsync(Guid orgId, CreatePromotionRequest req)
    {
        var discountType = Enum.Parse<DiscountType>(req.DiscountType);
        var promo = new Promotion(orgId, req.Name, discountType, req.DiscountValue,
            req.StartDate, req.EndDate, req.Description, req.MinimumOrderAmount,
            req.MaxUsesTotal, req.MaxUsesPerCustomer, req.BuyQuantity, req.GetQuantity,
            req.ApplyToAllProducts, req.ApplicableSkus);

        _db.Promotions.Add(promo);
        await _db.SaveChangesAsync();
        return PromotionToDto(promo);
    }

    public async Task<PromotionDto> UpdatePromotionAsync(Guid orgId, Guid id, UpdatePromotionRequest req)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");

        promo.Update(req.Name, req.Description, req.DiscountValue, req.StartDate,
            req.EndDate, req.MinimumOrderAmount, req.MaxUsesTotal, req.MaxUsesPerCustomer,
            req.ApplyToAllProducts, req.ApplicableSkus);

        await _db.SaveChangesAsync();
        return PromotionToDto(promo);
    }

    public async Task<PromotionDto> TogglePromotionAsync(Guid orgId, Guid id)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");

        if (promo.Status == PromotionStatus.Inactive) promo.Activate();
        else promo.Deactivate();

        await _db.SaveChangesAsync();
        return PromotionToDto(promo);
    }

    public async Task DeletePromotionAsync(Guid orgId, Guid id)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");
        _db.Promotions.Remove(promo);
        await _db.SaveChangesAsync();
    }

    // ── Coupons ────────────────────────────────────────────────────────────────

    public async Task<List<CouponDto>> GetCouponsAsync(Guid orgId, Guid? promotionId = null)
    {
        var query = _db.Coupons
            .Include(c => c.Promotion)
            .Where(c => c.OrganizationId == orgId);

        if (promotionId.HasValue)
            query = query.Where(c => c.PromotionId == promotionId.Value);

        var list = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        return list.Select(CouponToDto).ToList();
    }

    public async Task<CouponDto> CreateCouponAsync(Guid orgId, CreateCouponRequest req)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == req.PromotionId && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");

        var coupon = new Coupon(orgId, req.PromotionId, req.Code, req.MaxUses, req.ExpiresAt);
        _db.Coupons.Add(coupon);
        await _db.SaveChangesAsync();

        // Re-load with navigation
        await _db.SaveChangesAsync();
        coupon = await _db.Coupons.Include(c => c.Promotion).FirstAsync(c => c.Id == coupon.Id);
        return CouponToDto(coupon);
    }

    public async Task<List<CouponDto>> BulkCreateCouponsAsync(Guid orgId, BulkCreateCouponsRequest req)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(x => x.Id == req.PromotionId && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Promotion not found.");

        var coupons = new List<Coupon>();
        for (int i = 0; i < req.Count; i++)
        {
            var code = req.Prefix + "-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            coupons.Add(new Coupon(orgId, req.PromotionId, code, req.MaxUses, req.ExpiresAt));
        }

        _db.Coupons.AddRange(coupons);
        await _db.SaveChangesAsync();

        var ids = coupons.Select(c => c.Id).ToList();
        var result = await _db.Coupons.Include(c => c.Promotion)
            .Where(c => ids.Contains(c.Id)).ToListAsync();

        return result.Select(CouponToDto).ToList();
    }

    public async Task<CouponValidationResultDto> ValidateCouponAsync(Guid orgId, string code, decimal orderAmount)
    {
        var coupon = await _db.Coupons
            .Include(c => c.Promotion)
            .FirstOrDefaultAsync(c => c.OrganizationId == orgId &&
                                      c.Code == code.Trim().ToUpperInvariant());

        if (coupon == null)
            return new CouponValidationResultDto(false, "Coupon not found.", null, null, 0, 0, null, 0);

        var now = DateTime.UtcNow;

        if (!coupon.IsValid(now) || !coupon.Promotion!.IsValid(orderAmount, now))
            return new CouponValidationResultDto(false, "Coupon is not valid or has expired.",
                coupon.Promotion?.Name, coupon.Promotion?.DiscountType.ToString(), 0, 0,
                coupon.ExpiresAt, Math.Max(0, coupon.MaxUses - coupon.UsedCount));

        var discount = coupon.Promotion!.CalculateDiscount(orderAmount);
        var remaining = coupon.MaxUses == 0 ? 999 : coupon.MaxUses - coupon.UsedCount;

        return new CouponValidationResultDto(true, "Valid", coupon.Promotion.Name,
            coupon.Promotion.DiscountType.ToString(), coupon.Promotion.DiscountValue,
            discount, coupon.ExpiresAt, remaining);
    }

    public async Task<CouponDto> DeactivateCouponAsync(Guid orgId, Guid id)
    {
        var coupon = await _db.Coupons.Include(c => c.Promotion)
            .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Coupon not found.");

        coupon.Deactivate();
        await _db.SaveChangesAsync();
        return CouponToDto(coupon);
    }

    // ── Loyalty Programs ───────────────────────────────────────────────────────

    public async Task<List<LoyaltyProgramDto>> GetLoyaltyProgramsAsync(Guid orgId)
    {
        var list = await _db.LoyaltyPrograms
            .Where(p => p.OrganizationId == orgId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return list.Select(LoyaltyProgramToDto).ToList();
    }

    public async Task<LoyaltyProgramDto> GetLoyaltyProgramAsync(Guid orgId, Guid id)
    {
        var p = await _db.LoyaltyPrograms.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Loyalty program not found.");
        return LoyaltyProgramToDto(p);
    }

    public async Task<LoyaltyProgramDto> CreateLoyaltyProgramAsync(Guid orgId, CreateLoyaltyProgramRequest req)
    {
        var program = new LoyaltyProgram(orgId, req.Name, req.Description,
            req.PointsPerDollar, req.DollarPerPoint, req.RedemptionThreshold,
            req.SilverThreshold, req.GoldThreshold, req.PlatinumThreshold);

        _db.LoyaltyPrograms.Add(program);
        await _db.SaveChangesAsync();
        return LoyaltyProgramToDto(program);
    }

    public async Task<LoyaltyProgramDto> UpdateLoyaltyProgramAsync(Guid orgId, Guid id, UpdateLoyaltyProgramRequest req)
    {
        var program = await _db.LoyaltyPrograms.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Loyalty program not found.");

        program.Update(req.Name, req.Description, req.PointsPerDollar, req.DollarPerPoint,
            req.RedemptionThreshold, req.SilverThreshold, req.GoldThreshold, req.PlatinumThreshold);

        await _db.SaveChangesAsync();
        return LoyaltyProgramToDto(program);
    }

    public async Task<LoyaltyProgramDto> ToggleLoyaltyProgramAsync(Guid orgId, Guid id)
    {
        var program = await _db.LoyaltyPrograms.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Loyalty program not found.");
        program.Toggle();
        await _db.SaveChangesAsync();
        return LoyaltyProgramToDto(program);
    }

    // ── Loyalty Accounts ───────────────────────────────────────────────────────

    public async Task<List<CustomerLoyaltyAccountDto>> GetLoyaltyAccountsAsync(Guid orgId, Guid programId)
    {
        var list = await _db.CustomerLoyaltyAccounts
            .Include(a => a.Program)
            .Where(a => a.OrganizationId == orgId && a.LoyaltyProgramId == programId)
            .OrderByDescending(a => a.TotalPoints)
            .ToListAsync();
        return list.Select(AccountToDto).ToList();
    }

    public async Task<CustomerLoyaltyAccountDto> GetCustomerLoyaltyAccountAsync(Guid orgId, Guid customerId)
    {
        var account = await _db.CustomerLoyaltyAccounts
            .Include(a => a.Program)
            .FirstOrDefaultAsync(a => a.OrganizationId == orgId && a.CustomerId == customerId)
            ?? throw new KeyNotFoundException("Loyalty account not found.");
        return AccountToDto(account);
    }

    public async Task<CustomerLoyaltyAccountDto> EnrollCustomerAsync(Guid orgId, Guid programId, EnrollCustomerRequest req)
    {
        var existing = await _db.CustomerLoyaltyAccounts
            .FirstOrDefaultAsync(a => a.OrganizationId == orgId &&
                                      a.LoyaltyProgramId == programId &&
                                      a.CustomerId == req.CustomerId);

        if (existing != null) throw new InvalidOperationException("Customer already enrolled in this program.");

        var account = new CustomerLoyaltyAccount(orgId, programId, req.CustomerId, req.CustomerName, req.CustomerEmail);
        _db.CustomerLoyaltyAccounts.Add(account);
        await _db.SaveChangesAsync();

        account = await _db.CustomerLoyaltyAccounts.Include(a => a.Program).FirstAsync(a => a.Id == account.Id);
        return AccountToDto(account);
    }

    public async Task<CustomerLoyaltyAccountDto> AwardPointsAsync(Guid orgId, Guid programId, AwardPointsRequest req)
    {
        var program = await _db.LoyaltyPrograms.FirstOrDefaultAsync(x => x.Id == programId && x.OrganizationId == orgId)
            ?? throw new KeyNotFoundException("Loyalty program not found.");

        var account = await _db.CustomerLoyaltyAccounts.Include(a => a.Program)
            .FirstOrDefaultAsync(a => a.OrganizationId == orgId &&
                                      a.LoyaltyProgramId == programId &&
                                      a.CustomerId == req.CustomerId)
            ?? throw new KeyNotFoundException("Loyalty account not found.");

        account.EarnPoints(req.Points, program);
        await _db.SaveChangesAsync();
        return AccountToDto(account);
    }

    public async Task<CustomerLoyaltyAccountDto> RedeemPointsAsync(Guid orgId, Guid programId, RedeemPointsRequest req)
    {
        var account = await _db.CustomerLoyaltyAccounts.Include(a => a.Program)
            .FirstOrDefaultAsync(a => a.OrganizationId == orgId &&
                                      a.LoyaltyProgramId == programId &&
                                      a.CustomerId == req.CustomerId)
            ?? throw new KeyNotFoundException("Loyalty account not found.");

        account.RedeemPoints(req.Points);
        await _db.SaveChangesAsync();
        return AccountToDto(account);
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static CampaignDto CampaignToDto(Campaign c) => new(
        c.Id, c.Name, c.Description, c.Type.ToString(), c.Status.ToString(),
        c.TargetAudience.ToString(), c.StartDate, c.EndDate, c.Budget, c.ActualSpend,
        c.LinkedPromotionId, null, c.Tags, c.ReachCount, c.ConversionCount, c.CreatedAt
    );

    private static PromotionDto PromotionToDto(Promotion p) => new(
        p.Id, p.Name, p.Description, p.DiscountType.ToString(), p.Status.ToString(),
        p.DiscountValue, p.BuyQuantity, p.GetQuantity, p.MinimumOrderAmount,
        p.MaxUsesTotal, p.MaxUsesPerCustomer, p.UsedCount, p.StartDate, p.EndDate,
        p.ApplyToAllProducts, p.ApplicableSkus, p.CreatedAt
    );

    private static CouponDto CouponToDto(Coupon c) => new(
        c.Id, c.PromotionId, c.Promotion?.Name ?? "",
        c.Code, c.IsActive, c.MaxUses, c.UsedCount,
        c.MaxUses == 0 ? 999 : Math.Max(0, c.MaxUses - c.UsedCount),
        c.ExpiresAt, c.CreatedAt
    );

    private static LoyaltyProgramDto LoyaltyProgramToDto(LoyaltyProgram p) => new(
        p.Id, p.Name, p.Description, p.PointsPerDollar, p.DollarPerPoint,
        p.RedemptionThreshold, p.SilverThreshold, p.GoldThreshold, p.PlatinumThreshold,
        p.IsActive, p.CreatedAt
    );

    private static CustomerLoyaltyAccountDto AccountToDto(CustomerLoyaltyAccount a) => new(
        a.Id, a.CustomerId, a.CustomerName, a.CustomerEmail,
        a.TotalPoints, a.RedeemedPoints, a.AvailablePoints,
        a.Tier.ToString(), a.LastActivityAt, a.CreatedAt
    );
}
