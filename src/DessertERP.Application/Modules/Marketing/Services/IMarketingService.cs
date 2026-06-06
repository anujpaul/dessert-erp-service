using DessertERP.Application.Modules.Marketing.DTOs;

namespace DessertERP.Application.Modules.Marketing.Services;

public interface IMarketingService
{
    // Campaigns
    Task<List<CampaignDto>> GetCampaignsAsync(Guid orgId);
    Task<CampaignDto> GetCampaignAsync(Guid orgId, Guid id);
    Task<CampaignDto> CreateCampaignAsync(Guid orgId, CreateCampaignRequest req);
    Task<CampaignDto> UpdateCampaignAsync(Guid orgId, Guid id, UpdateCampaignRequest req);
    Task<CampaignDto> SetCampaignStatusAsync(Guid orgId, Guid id, SetCampaignStatusRequest req);
    Task<CampaignDto> RecordMetricsAsync(Guid orgId, Guid id, RecordCampaignMetricsRequest req);
    Task DeleteCampaignAsync(Guid orgId, Guid id);

    // Promotions
    Task<List<PromotionDto>> GetPromotionsAsync(Guid orgId);
    Task<PromotionDto> GetPromotionAsync(Guid orgId, Guid id);
    Task<PromotionDto> CreatePromotionAsync(Guid orgId, CreatePromotionRequest req);
    Task<PromotionDto> UpdatePromotionAsync(Guid orgId, Guid id, UpdatePromotionRequest req);
    Task<PromotionDto> TogglePromotionAsync(Guid orgId, Guid id);
    Task DeletePromotionAsync(Guid orgId, Guid id);

    // Coupons
    Task<List<CouponDto>> GetCouponsAsync(Guid orgId, Guid? promotionId = null);
    Task<CouponDto> CreateCouponAsync(Guid orgId, CreateCouponRequest req);
    Task<List<CouponDto>> BulkCreateCouponsAsync(Guid orgId, BulkCreateCouponsRequest req);
    Task<CouponValidationResultDto> ValidateCouponAsync(Guid orgId, string code, decimal orderAmount);
    Task<CouponDto> DeactivateCouponAsync(Guid orgId, Guid id);

    // Loyalty
    Task<List<LoyaltyProgramDto>> GetLoyaltyProgramsAsync(Guid orgId);
    Task<LoyaltyProgramDto> GetLoyaltyProgramAsync(Guid orgId, Guid id);
    Task<LoyaltyProgramDto> CreateLoyaltyProgramAsync(Guid orgId, CreateLoyaltyProgramRequest req);
    Task<LoyaltyProgramDto> UpdateLoyaltyProgramAsync(Guid orgId, Guid id, UpdateLoyaltyProgramRequest req);
    Task<LoyaltyProgramDto> ToggleLoyaltyProgramAsync(Guid orgId, Guid id);

    Task<List<CustomerLoyaltyAccountDto>> GetLoyaltyAccountsAsync(Guid orgId, Guid programId);
    Task<CustomerLoyaltyAccountDto> GetCustomerLoyaltyAccountAsync(Guid orgId, Guid customerId);
    Task<CustomerLoyaltyAccountDto> EnrollCustomerAsync(Guid orgId, Guid programId, EnrollCustomerRequest req);
    Task<CustomerLoyaltyAccountDto> AwardPointsAsync(Guid orgId, Guid programId, AwardPointsRequest req);
    Task<CustomerLoyaltyAccountDto> RedeemPointsAsync(Guid orgId, Guid programId, RedeemPointsRequest req);
}
