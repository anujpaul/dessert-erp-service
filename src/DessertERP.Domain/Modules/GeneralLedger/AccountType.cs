using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum AccountNature { Debit, Credit }

public class AccountType : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public AccountNature Nature { get; private set; }
    public int DisplayOrder { get; private set; }

    private AccountType() { }

    public AccountType(string code, string name, AccountNature nature, int displayOrder)
    {
        Code = code;
        Name = name;
        Nature = nature;
        DisplayOrder = displayOrder;
    }
}
