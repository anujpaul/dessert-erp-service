using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.SystemAdmin;

public class Role : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public string Name           { get; private set; } = string.Empty;
    public string Description    { get; private set; } = string.Empty;
    public bool   IsSystemRole   { get; private set; }   // built-in, cannot be deleted

    private readonly List<RolePermission> _permissions = new();
    public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();

    private Role() { }

    public Role(Guid organizationId, string name, string description, bool isSystemRole = false)
    {
        OrganizationId = organizationId;
        Name           = name.Trim();
        Description    = description.Trim();
        IsSystemRole   = isSystemRole;
    }

    public void Update(string name, string description) { Name = name.Trim(); Description = description.Trim(); SetUpdated(); }

    public void GrantPermission(string module, string action)
    {
        if (_permissions.Any(p => p.Module == module && p.Action == action)) return;
        _permissions.Add(new RolePermission(Id, module, action));
        SetUpdated();
    }

    public void RevokePermission(string module, string action)
    {
        var p = _permissions.FirstOrDefault(p => p.Module == module && p.Action == action);
        if (p != null) { _permissions.Remove(p); SetUpdated(); }
    }

    public bool HasPermission(string module, string action) =>
        _permissions.Any(p => p.Module == module && p.Action == action);
}
