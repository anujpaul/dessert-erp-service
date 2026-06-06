namespace DessertERP.Application.Modules.Marketing.DTOs;

// ── Campaign DTOs ────────────────────────────────────────────────────────────

public record CampaignDto(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    string Status,
    string TargetAudience,
    DateTime StartDate,
    DateTime? EndDate,
    decimal Budget,
    decimal ActualSpend,
    Guid? LinkedPromotionId,
    string? LinkedPromotionName,
    string? Tags,
    int ReachCount,
    int ConversionCount,
    DateTime CreatedAt
);

public record CreateCampaignRequest(
    string Name,
    string? Description,
    string Type,           // Email | SMS | Social | InStore | MultiChannel
    string TargetAudience, // AllCustomers | NewCustomers | LoyaltyMembers | HighValue | Custom
    DateTime StartDate,
    DateTime? EndDate,
    decimal Budget,
    Guid? LinkedPromotionId,
    string? Tags
);

public record UpdateCampaignRequest(
    string Name,
    string? Description,
    string Type,
    string TargetAudience,
    DateTime StartDate,
    DateTime? EndDate,
    decimal Budget,
    Guid? LinkedPromotionId,
    string? Tags
);

public record SetCampaignStatusRequest(string Status);

public record RecordCampaignMetricsRequest(int Reach, int Conversions, decimal Spend);

// ── Promotion DTOs ────────────────────────────────────────────────────────────

public record PromotionDto(
    Guid Id,
    string Name,
    string? Description,
    string DiscountType,
    string Status,
    decimal DiscountValue,
    int? BuyQuantity,
    int? GetQuantity,
    decimal MinimumOrderAmount,
    int MaxUsesTotal,
    int MaxUsesPerCustomer,
    int UsedCount,
    DateTime StartDate,
    DateTime? EndDate,
    bool ApplyToAllProducts,
    string? ApplicableSkus,
    DateTime CreatedAt
);

public record CreatePromotionRequest(
    string Name,
    string? Description,
    string DiscountType,   // PercentageOff | FixedAmountOff | BuyXGetY
    decimal DiscountValue,
    DateTime StartDate,
    DateTime? EndDate,
    decimal MinimumOrderAmount,
    int MaxUsesTotal,
    int MaxUsesPerCustomer,
    int? BuyQuantity,
    int? GetQuantity,
    bool ApplyToAllProducts,
    string? ApplicableSkus
);

public record UpdatePromotionRequest(
    string Name,
    string? Description,
    decimal DiscountValue,
    DateTime StartDate,
    DateTime? EndDate,
    decimal MinimumOrderAmount,
    int MaxUsesTotal,
    int MaxUsesPerCustomer,
    bool ApplyToAllProducts,
    string? ApplicableSkus
);

// ── Coupon DTOs ───────────────────────────────────────────────────────────────

public record CouponDto(
    Guid Id,
    Guid PromotionId,
    string PromotionName,
    string Code,
    bool IsActive,
    int MaxUses,
    int UsedCount,
    int RemainingUses,
    DateTime? ExpiresAt,
    DateTime CreatedAt
);

public record CreateCouponRequest(
    Guid PromotionId,
    string Code,
    int MaxUses,
    DateTime? ExpiresAt
);

public record BulkCreateCouponsRequest(
    Guid PromotionId,
    int Count,
    string Prefix,
    int MaxUses,
    DateTime? ExpiresAt
);

public record CouponValidationResultDto(
    bool IsValid,
    string? Message,
    string? PromotionName,
    string? DiscountType,
    decimal DiscountValue,
    decimal DiscountAmount,
    DateTime? ExpiresAt,
    int RemainingUses
);

// ── Loyalty DTOs ──────────────────────────────────────────────────────────────

public record LoyaltyProgramDto(
    Guid Id,
    string Name,
    string? Description,
    decimal PointsPerDollar,
    decimal DollarPerPoint,
    int RedemptionThreshold,
    int SilverThreshold,
    int GoldThreshold,
    int PlatinumThreshold,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateLoyaltyProgramRequest(
    string Name,
    string? Description,
    decimal PointsPerDollar,
    decimal DollarPerPoint,
    int RedemptionThreshold,
    int SilverThreshold,
    int GoldThreshold,
    int PlatinumThreshold
);

public record UpdateLoyaltyProgramRequest(
    string Name,
    string? Description,
    decimal PointsPerDollar,
    decimal DollarPerPoint,
    int RedemptionThreshold,
    int SilverThreshold,
    int GoldThreshold,
    int PlatinumThreshold
);

public record CustomerLoyaltyAccountDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string? CustomerEmail,
    int TotalPoints,
    int RedeemedPoints,
    int AvailablePoints,
    string Tier,
    DateTime? LastActivityAt,
    DateTime CreatedAt
);

public record EnrollCustomerRequest(
    Guid CustomerId,
    string CustomerName,
    string? CustomerEmail
);

public record AwardPointsRequest(Guid CustomerId, int Points);

public record RedeemPointsRequest(Guid CustomerId, int Points);
