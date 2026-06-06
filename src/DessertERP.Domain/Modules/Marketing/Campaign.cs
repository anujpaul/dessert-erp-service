namespace DessertERP.Domain.Modules.Marketing;

public enum CampaignType { Email, SMS, Social, InStore, MultiChannel }

public enum CampaignStatus { Draft, Scheduled, Active, Paused, Completed, Cancelled }

public enum TargetAudience { AllCustomers, NewCustomers, LoyaltyMembers, HighValue, Custom }

public class Campaign
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public CampaignType Type { get; private set; }
    public CampaignStatus Status { get; private set; }
    public TargetAudience TargetAudience { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public decimal Budget { get; private set; }
    public decimal ActualSpend { get; private set; }
    public Guid? LinkedPromotionId { get; private set; }
    public string? Tags { get; private set; }
    public int ReachCount { get; private set; }
    public int ConversionCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Campaign() { }

    public Campaign(Guid organizationId, string name, string? description,
        CampaignType type, TargetAudience targetAudience,
        DateTime startDate, DateTime? endDate, decimal budget,
        Guid? linkedPromotionId = null, string? tags = null)
    {
        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        Type = type;
        Status = CampaignStatus.Draft;
        TargetAudience = targetAudience;
        StartDate = startDate;
        EndDate = endDate;
        Budget = budget;
        ActualSpend = 0;
        LinkedPromotionId = linkedPromotionId;
        Tags = tags;
        ReachCount = 0;
        ConversionCount = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, CampaignType type,
        TargetAudience targetAudience, DateTime startDate, DateTime? endDate,
        decimal budget, Guid? linkedPromotionId, string? tags)
    {
        Name = name;
        Description = description;
        Type = type;
        TargetAudience = targetAudience;
        StartDate = startDate;
        EndDate = endDate;
        Budget = budget;
        LinkedPromotionId = linkedPromotionId;
        Tags = tags;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStatus(CampaignStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordMetrics(int reach, int conversions, decimal spend)
    {
        ReachCount += reach;
        ConversionCount += conversions;
        ActualSpend += spend;
        UpdatedAt = DateTime.UtcNow;
    }
}
