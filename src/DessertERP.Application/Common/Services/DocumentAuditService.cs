using System.Text.Json;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Common.Services;

public record DocumentAuditDto(
    Guid Id,
    string Username,
    string Action,
    string? OldValues,
    string? NewValues,
    DateTime OccurredAt);

public interface IDocumentAuditService
{
    void Add(string module, string action, Guid entityId, string entityType, object? oldValues = null, object? newValues = null);
    Task<IReadOnlyList<DocumentAuditDto>> GetAsync(string entityType, Guid entityId, CancellationToken ct = default);
}

public class DocumentAuditService : IDocumentAuditService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;
    private readonly ICurrentUserService _user;

    public DocumentAuditService(
        IAppDbContext db,
        ICurrentOrganizationService org,
        ICurrentUserService user)
    {
        _db = db;
        _org = org;
        _user = user;
    }

    public void Add(
        string module,
        string action,
        Guid entityId,
        string entityType,
        object? oldValues = null,
        object? newValues = null)
    {
        _db.AuditLogs.Add(new AuditLogEntry(
            _org.OrganizationId,
            _user.UserId,
            _user.Username,
            module,
            action,
            entityId.ToString(),
            entityType,
            Serialize(oldValues),
            Serialize(newValues),
            _user.IpAddress));
    }

    public async Task<IReadOnlyList<DocumentAuditDto>> GetAsync(
        string entityType,
        Guid entityId,
        CancellationToken ct = default)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityType == entityType && a.EntityId == entityId.ToString())
            .OrderByDescending(a => a.OccurredAt)
            .Select(a => new DocumentAuditDto(
                a.Id, a.Username, a.Action, a.OldValues, a.NewValues, a.OccurredAt))
            .ToListAsync(ct);
    }

    private static string? Serialize(object? value) =>
        value is null ? null : JsonSerializer.Serialize(value, JsonOptions);
}
