using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.SystemAdmin;

/// <summary>
/// A permission granted to a Role for a specific module + action.
/// Actions: Read, Write, Delete, Approve
/// Modules: GL, AR, AP, PM, SysAdmin
/// </summary>
public class RolePermission : BaseEntity
{
    public Guid   RoleId { get; private set; }
    public string Module { get; private set; } = string.Empty;  // e.g. "AR"
    public string Action { get; private set; } = string.Empty;  // e.g. "Write"

    private RolePermission() { }

    public RolePermission(Guid roleId, string module, string action)
    {
        RoleId = roleId;
        Module = module;
        Action = action;
    }
}
