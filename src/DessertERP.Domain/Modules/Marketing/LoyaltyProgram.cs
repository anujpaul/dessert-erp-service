namespace DessertERP.Domain.Modules.Marketing;

public enum LoyaltyTier { Bronze, Silver, Gold, Platinum }

public class LoyaltyProgram
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    /// <summary>Points earned per dollar spent</summary>
    public decimal PointsPerDollar { get; private set; }
    /// <summary>Dollar value of one point when redeeming</summary>
    public decimal DollarPerPoint { get; private set; }
    /// <summary>Minimum points needed to redeem</summary>
    public int RedemptionThreshold { get; private set; }
    // Tier thresholds (points)
    public int SilverThreshold { get; private set; }
    public int GoldThreshold { get; private set; }
    public int PlatinumThreshold { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private LoyaltyProgram() { }

    public LoyaltyProgram(Guid organizationId, string name, string? description,
        decimal pointsPerDollar, decimal dollarPerPoint, int redemptionThreshold,
        int silverThreshold = 500, int goldThreshold = 2000, int platinumThreshold = 5000)
    {
        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        PointsPerDollar = pointsPerDollar;
        DollarPerPoint = dollarPerPoint;
        RedemptionThreshold = redemptionThreshold;
        SilverThreshold = silverThreshold;
        GoldThreshold = goldThreshold;
        PlatinumThreshold = platinumThreshold;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, decimal pointsPerDollar,
        decimal dollarPerPoint, int redemptionThreshold,
        int silverThreshold, int goldThreshold, int platinumThreshold)
    {
        Name = name;
        Description = description;
        PointsPerDollar = pointsPerDollar;
        DollarPerPoint = dollarPerPoint;
        RedemptionThreshold = redemptionThreshold;
        SilverThreshold = silverThreshold;
        GoldThreshold = goldThreshold;
        PlatinumThreshold = platinumThreshold;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Toggle() { IsActive = !IsActive; UpdatedAt = DateTime.UtcNow; }
}

public class CustomerLoyaltyAccount
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid LoyaltyProgramId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public string? CustomerEmail { get; private set; }
    public int TotalPoints { get; private set; }
    public int RedeemedPoints { get; private set; }
    public int AvailablePoints => TotalPoints - RedeemedPoints;
    public LoyaltyTier Tier { get; private set; }
    public DateTime? LastActivityAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Nav properties
    public LoyaltyProgram Program { get; private set; } = null!;

    private CustomerLoyaltyAccount() { }

    public CustomerLoyaltyAccount(Guid organizationId, Guid loyaltyProgramId,
        Guid customerId, string customerName, string? customerEmail)
    {
        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        LoyaltyProgramId = loyaltyProgramId;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        TotalPoints = 0;
        RedeemedPoints = 0;
        Tier = LoyaltyTier.Bronze;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void EarnPoints(int points, LoyaltyProgram program)
    {
        TotalPoints += points;
        LastActivityAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        RecalculateTier(program);
    }

    public void RedeemPoints(int points)
    {
        if (points > AvailablePoints)
            throw new InvalidOperationException("Insufficient points.");
        RedeemedPoints += points;
        LastActivityAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTier(LoyaltyProgram program)
    {
        Tier = TotalPoints >= program.PlatinumThreshold ? LoyaltyTier.Platinum
             : TotalPoints >= program.GoldThreshold ? LoyaltyTier.Gold
             : TotalPoints >= program.SilverThreshold ? LoyaltyTier.Silver
             : LoyaltyTier.Bronze;
    }
}
