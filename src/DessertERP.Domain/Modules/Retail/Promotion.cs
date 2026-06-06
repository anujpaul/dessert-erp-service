using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Retail;

public enum DiscountType
{
    PercentageOff,   // e.g. 10% off
    FixedAmountOff,  // e.g. $5 off
    BuyXGetY,        // e.g. buy 2 get 1 free
}

public enum PromotionStatus { Active, Inactive, Expired, Scheduled }

public class Promotion : BaseEntity
{
    public Guid          OrganizationId   { get; private set; }
    public string        Name             { get; private set; } = string.Empty;
    public string?       Description      { get; private set; }
    public DiscountType  DiscountType     { get; private set; }
    public PromotionStatus Status         { get; private set; } = PromotionStatus.Active;

    // Discount value — meaning depends on DiscountType:
    //   PercentageOff:   DiscountValue = 10  (= 10%)
    //   FixedAmountOff:  DiscountValue = 5   (= $5)
    //   BuyXGetY:        BuyQuantity = 2, GetQuantity = 1, DiscountValue = 100 (100% off the free item)
    public decimal DiscountValue   { get; private set; }
    public int?    BuyQuantity     { get; private set; }   // BuyXGetY only
    public int?    GetQuantity     { get; private set; }   // BuyXGetY only

    // Conditions
    public decimal MinimumOrderAmount { get; private set; }   // 0 = no minimum
    public int     MaxUsesTotal       { get; private set; }   // 0 = unlimited
    public int     MaxUsesPerCustomer { get; private set; }   // 0 = unlimited
    public int     UsedCount          { get; private set; }

    // Validity window
    public DateTime  StartDate { get; private set; }
    public DateTime? EndDate   { get; private set; }

    // Applicability (null = applies to all)
    public bool ApplyToAllProducts   { get; private set; } = true;
    public string? ApplicableSkus    { get; private set; } // comma-separated SKUs

    private Promotion() { }

    public Promotion(Guid organizationId, string name, DiscountType discountType,
        decimal discountValue, DateTime startDate, DateTime? endDate = null,
        string? description = null, decimal minimumOrderAmount = 0,
        int maxUsesTotal = 0, int maxUsesPerCustomer = 0,
        int? buyQuantity = null, int? getQuantity = null,
        bool applyToAllProducts = true, string? applicableSkus = null)
    {
        OrganizationId      = organizationId;
        Name                = name.Trim();
        Description         = description?.Trim();
        DiscountType        = discountType;
        DiscountValue       = discountValue;
        StartDate           = startDate;
        EndDate             = endDate;
        MinimumOrderAmount  = minimumOrderAmount;
        MaxUsesTotal        = maxUsesTotal;
        MaxUsesPerCustomer  = maxUsesPerCustomer;
        BuyQuantity         = buyQuantity;
        GetQuantity         = getQuantity;
        ApplyToAllProducts  = applyToAllProducts;
        ApplicableSkus      = applicableSkus?.Trim().ToUpperInvariant();
        UpdateStatus();
    }

    public void Update(string name, string? description, decimal discountValue,
        DateTime startDate, DateTime? endDate, decimal minimumOrderAmount,
        int maxUsesTotal, int maxUsesPerCustomer,
        bool applyToAllProducts, string? applicableSkus)
    {
        Name               = name.Trim();
        Description        = description?.Trim();
        DiscountValue      = discountValue;
        StartDate          = startDate;
        EndDate            = endDate;
        MinimumOrderAmount = minimumOrderAmount;
        MaxUsesTotal       = maxUsesTotal;
        MaxUsesPerCustomer = maxUsesPerCustomer;
        ApplyToAllProducts = applyToAllProducts;
        ApplicableSkus     = applicableSkus?.Trim().ToUpperInvariant();
        UpdateStatus();
        SetUpdated();
    }

    public void IncrementUsage()
    {
        UsedCount++;
        UpdateStatus();
        SetUpdated();
    }

    public bool IsValid(decimal orderAmount, DateTime asOf)
    {
        if (Status == PromotionStatus.Expired || Status == PromotionStatus.Inactive) return false;
        if (asOf < StartDate) return false;
        if (EndDate.HasValue && asOf < EndDate.Value) return false;
        if (orderAmount < MinimumOrderAmount) return false;
        if (MaxUsesTotal > 0 && UsedCount >= MaxUsesTotal) return false;
        return true;
    }

    public decimal CalculateDiscount(decimal orderAmount, decimal itemAmount = 0)
    {
        return DiscountType switch
        {
            DiscountType.PercentageOff  => orderAmount * (DiscountValue / 100m),
            DiscountType.FixedAmountOff => Math.Min(DiscountValue, orderAmount),
            DiscountType.BuyXGetY       => itemAmount * (DiscountValue / 100m),
            _ => 0m
        };
    }

    private void UpdateStatus()
    {
        var now = DateTime.UtcNow;
        if (Status == PromotionStatus.Inactive) return;
        if (EndDate.HasValue && now > EndDate.Value)
            Status = PromotionStatus.Expired;
        else if (now < StartDate)
            Status = PromotionStatus.Scheduled;
        else
            Status = PromotionStatus.Active;
    }

    public void Activate()   { Status = PromotionStatus.Active;   SetUpdated(); }
    public void Deactivate() { Status = PromotionStatus.Inactive; SetUpdated(); }
}

public class Coupon : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid   PromotionId    { get; private set; }
    public string Code           { get; private set; } = string.Empty;
    public bool   IsActive       { get; private set; } = true;
    public int    MaxUses        { get; private set; } = 1;    // 0 = unlimited
    public int    UsedCount      { get; private set; }
    public DateTime? ExpiresAt   { get; private set; }

    // Navigation
    public Promotion? Promotion  { get; private set; }

    private Coupon() { }

    public Coupon(Guid organizationId, Guid promotionId, string code,
        int maxUses = 1, DateTime? expiresAt = null)
    {
        OrganizationId = organizationId;
        PromotionId    = promotionId;
        Code           = code.Trim().ToUpperInvariant();
        MaxUses        = maxUses;
        ExpiresAt      = expiresAt;
    }

    public bool IsValid(DateTime asOf)
    {
        if (!IsActive) return false;
        if (ExpiresAt.HasValue && asOf > ExpiresAt.Value) return false;
        if (MaxUses > 0 && UsedCount >= MaxUses) return false;
        return true;
    }

    public void Redeem()
    {
        UsedCount++;
        if (MaxUses > 0 && UsedCount >= MaxUses)
            IsActive = false;
        SetUpdated();
    }

    public void Deactivate() { IsActive = false; SetUpdated(); }
}

public class CouponRedemption : BaseEntity
{
    public Guid      OrganizationId   { get; private set; }
    public Guid      CouponId         { get; private set; }
    public Guid      POSTransactionId { get; private set; }
    public decimal   DiscountApplied  { get; private set; }
    public DateTime  RedeemedAt       { get; private set; }

    private CouponRedemption() { }

    public CouponRedemption(Guid organizationId, Guid couponId,
        Guid posTransactionId, decimal discountApplied)
    {
        OrganizationId   = organizationId;
        CouponId         = couponId;
        POSTransactionId = posTransactionId;
        DiscountApplied  = discountApplied;
        RedeemedAt       = DateTime.UtcNow;
    }
}
