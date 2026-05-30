using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum AccountStatus { Active, Inactive, Suspended }

public class Account : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid AccountTypeId { get; private set; }
    public Guid? ParentAccountId { get; private set; }
    public bool IsHeaderAccount { get; private set; }
    public bool AllowManualEntry { get; private set; }
    public AccountStatus Status { get; private set; } = AccountStatus.Active;
    public string Currency { get; private set; } = "USD";
    public int Level { get; private set; } = 1;

    public AccountType? AccountType { get; private set; }
    public Account? ParentAccount { get; private set; }

    private Account() { }

    public Account(Guid organizationId, string accountNumber, string name, Guid accountTypeId,
        bool isHeaderAccount, Guid? parentAccountId = null,
        string? description = null, string currency = "USD", int level = 1)
    {
        OrganizationId = organizationId;
        AccountNumber = accountNumber;
        Name = name;
        AccountTypeId = accountTypeId;
        IsHeaderAccount = isHeaderAccount;
        AllowManualEntry = !isHeaderAccount;
        ParentAccountId = parentAccountId;
        Description = description;
        Currency = currency;
        Level = level;
    }

    public void Deactivate()
    {
        Status = AccountStatus.Inactive;
        SetUpdated();
    }

    public void Activate()
    {
        Status = AccountStatus.Active;
        SetUpdated();
    }
}
