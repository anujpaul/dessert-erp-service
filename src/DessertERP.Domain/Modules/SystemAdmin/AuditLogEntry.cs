using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.SystemAdmin;

public class AuditLogEntry : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid?  UserId         { get; private set; }
    public string Username       { get; private set; } = string.Empty;
    public string Module         { get; private set; } = string.Empty;  // "AR", "AP", "GL"…
    public string Action         { get; private set; } = string.Empty;  // "CreateSalesOrder"
    public string? EntityId      { get; private set; }
    public string? EntityType    { get; private set; }
    public string? OldValues     { get; private set; }  // JSON snapshot
    public string? NewValues     { get; private set; }  // JSON snapshot
    public string? IpAddress     { get; private set; }
    public DateTime OccurredAt   { get; private set; }

    private AuditLogEntry() { }

    public AuditLogEntry(Guid organizationId, Guid? userId, string username,
        string module, string action,
        string? entityId = null, string? entityType = null,
        string? oldValues = null, string? newValues = null,
        string? ipAddress = null)
    {
        OrganizationId = organizationId;
        UserId         = userId;
        Username       = username;
        Module         = module;
        Action         = action;
        EntityId       = entityId;
        EntityType     = entityType;
        OldValues      = oldValues;
        NewValues      = newValues;
        IpAddress      = ipAddress;
        OccurredAt     = DateTime.UtcNow;
    }
}
