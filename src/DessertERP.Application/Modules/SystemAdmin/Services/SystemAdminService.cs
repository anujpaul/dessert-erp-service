using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Modules.SystemAdmin.DTOs;
using DessertERP.Domain.Common;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.SystemAdmin.Services;

public class SystemAdminService : ISystemAdminService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;
    private readonly IPasswordHasher _hasher;

    public SystemAdminService(IAppDbContext db, ICurrentOrganizationService org, IPasswordHasher hasher)
    {
        _db     = db;
        _org    = org;
        _hasher = hasher;
    }

    private Guid OrgId => _org.OrganizationId;

    // ── Users ─────────────────────────────────────────────────────────────────

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _db.AppUsers
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Where(u => u.OrganizationId == OrgId && !u.IsDeleted)
            .OrderBy(u => u.FullName)
            .ToListAsync(ct);
        return users.Select(ToUserDto);
    }

    public async Task<UserDto?> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        var u = await _db.AppUsers
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id && u.OrganizationId == OrgId && !u.IsDeleted, ct);
        return u is null ? null : ToUserDto(u);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest req, CancellationToken ct = default)
    {
        if (await _db.AppUsers.AnyAsync(u => u.Username == req.Username.ToLowerInvariant() && u.OrganizationId == OrgId, ct))
            throw new InvalidOperationException($"Username '{req.Username}' is already taken.");

        var hash = _hasher.Hash(req.Password);
        var user = new AppUser(OrgId, req.Username, req.Email, req.FullName, hash);

        foreach (var roleId in req.RoleIds ?? [])
            user.AssignRole(roleId);

        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync(ct);
        return (await GetUserAsync(user.Id, ct))!;
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequest req, CancellationToken ct = default)
    {
        var user = await _db.AppUsers
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id && u.OrganizationId == OrgId && !u.IsDeleted, ct)
            ?? throw new InvalidOperationException("User not found.");

        user.UpdateProfile(req.Email, req.FullName);

        var currentRoles = user.UserRoles.Select(r => r.RoleId).ToHashSet();
        var newRoles     = (req.RoleIds ?? []).ToHashSet();
        foreach (var r in newRoles.Except(currentRoles)) user.AssignRole(r);
        foreach (var r in currentRoles.Except(newRoles)) user.RemoveRole(r);

        await _db.SaveChangesAsync(ct);
        return (await GetUserAsync(id, ct))!;
    }

    public async Task DeactivateUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await LoadUser(id, ct);
        user.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    public async Task ActivateUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await LoadUser(id, ct);
        user.Activate();
        await _db.SaveChangesAsync(ct);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct = default)
    {
        var user = await LoadUser(req.UserId, ct);
        user.SetPasswordHash(_hasher.Hash(req.NewPassword));
        await _db.SaveChangesAsync(ct);
    }

    // ── Roles ─────────────────────────────────────────────────────────────────

    public async Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await _db.Roles
            .Include(r => r.Permissions)
            .Where(r => r.OrganizationId == OrgId && !r.IsDeleted)
            .OrderBy(r => r.Name)
            .ToListAsync(ct);
        return roles.Select(ToRoleDto);
    }

    public async Task<RoleDto?> GetRoleAsync(Guid id, CancellationToken ct = default)
    {
        var r = await _db.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == OrgId && !r.IsDeleted, ct);
        return r is null ? null : ToRoleDto(r);
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest req, CancellationToken ct = default)
    {
        var role = new Role(OrgId, req.Name, req.Description);
        foreach (var p in req.Permissions ?? [])
            role.GrantPermission(p.Module, p.Action);
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(ct);
        return (await GetRoleAsync(role.Id, ct))!;
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleRequest req, CancellationToken ct = default)
    {
        var role = await _db.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == OrgId && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException("Role not found.");

        if (role.IsSystemRole) throw new InvalidOperationException("Cannot modify system roles.");

        role.Update(req.Name, req.Description);

        // Sync permissions
        var existing = role.Permissions.Select(p => (p.Module, p.Action)).ToHashSet();
        var desired  = (req.Permissions ?? []).Select(p => (p.Module, p.Action)).ToHashSet();
        foreach (var (m, a) in desired.Except(existing)) role.GrantPermission(m, a);
        foreach (var (m, a) in existing.Except(desired)) role.RevokePermission(m, a);

        await _db.SaveChangesAsync(ct);
        return (await GetRoleAsync(id, ct))!;
    }

    public async Task DeleteRoleAsync(Guid id, CancellationToken ct = default)
    {
        var role = await _db.Roles
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == OrgId && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException("Role not found.");
        if (role.IsSystemRole) throw new InvalidOperationException("Cannot delete system roles.");
        role.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Audit Log ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<AuditLogDto>> GetAuditLogAsync(
        string? module = null, Guid? userId = null,
        DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        var q = _db.AuditLogs
            .Where(a => a.OrganizationId == OrgId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(module)) q = q.Where(a => a.Module == module);
        if (userId.HasValue)                    q = q.Where(a => a.UserId == userId);
        if (from.HasValue)                      q = q.Where(a => a.OccurredAt >= from.Value);
        if (to.HasValue)                        q = q.Where(a => a.OccurredAt <= to.Value);

        var entries = await q.OrderByDescending(a => a.OccurredAt).Take(500).ToListAsync(ct);
        return entries.Select(a => new AuditLogDto(
            a.Id, a.Username, a.Module, a.Action,
            a.EntityId, a.EntityType, a.OldValues, a.NewValues,
            a.IpAddress, a.OccurredAt));
    }

    // ── Org Settings ──────────────────────────────────────────────────────────

    public async Task<OrgSettingsDto> GetOrgSettingsAsync(CancellationToken ct = default)
    {
        var org = await _db.Organizations
            .FirstOrDefaultAsync(o => o.Id == OrgId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Organization not found.");
        return ToOrgSettingsDto(org);
    }

    public async Task<OrgSettingsDto> UpdateOrgSettingsAsync(UpdateOrgSettingsRequest req, CancellationToken ct = default)
    {
        var org = await _db.Organizations
            .FirstOrDefaultAsync(o => o.Id == OrgId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Organization not found.");
        if (!Enum.TryParse<MoneyRoundingMethod>(req.MoneyRoundingMethod, true, out var roundingMethod))
            throw new InvalidOperationException("Invalid money rounding method.");
        if (!Enum.TryParse<MoneyRoundingLevel>(req.MoneyRoundingLevel, true, out var roundingLevel))
            throw new InvalidOperationException("Invalid money rounding level.");
        org.UpdateSettings(
            req.Name, req.LogoUrl, req.DefaultCurrency, req.Timezone, req.TaxId, req.Address,
            req.MoneyDecimalPlaces, roundingMethod, roundingLevel);
        // Also update contact details via the Update method
        org.Update(req.Name, req.Address, req.Phone, req.Email, req.TaxId, req.LogoUrl);
        await _db.SaveChangesAsync(ct);
        return ToOrgSettingsDto(org);
    }

    private static OrgSettingsDto ToOrgSettingsDto(Domain.Modules.Organization.Organization org) =>
        new(org.Id, org.Code, org.Name, org.LogoUrl,
            org.BaseCurrency, org.DefaultCurrency, org.Timezone,
            org.TaxId, org.Address, org.Phone, org.Email,
            org.MoneyDecimalPlaces, org.MoneyRoundingMethod.ToString(), org.MoneyRoundingLevel.ToString());

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<AppUser> LoadUser(Guid id, CancellationToken ct) =>
        await _db.AppUsers.FirstOrDefaultAsync(u => u.Id == id && u.OrganizationId == OrgId && !u.IsDeleted, ct)
        ?? throw new InvalidOperationException("User not found.");

    private static UserDto ToUserDto(AppUser u) => new(
        u.Id, u.OrganizationId, u.Username, u.Email, u.FullName,
        u.Status.ToString(), u.LastLoginAt,
        u.UserRoles.Where(r => r.Role != null).Select(r => r.Role!.Name).ToList(),
        PermissionCatalog.ExpandForRoles(u.UserRoles
            .Where(r => r.Role != null)
            .SelectMany(r => r.Role!.Permissions)
            .Select(p => $"{p.Module}:{p.Action}"),
            u.UserRoles.Where(r => r.Role != null).Select(r => r.Role!.Name))
            .Order()
            .ToList(),
        u.CreatedAt);

    private static RoleDto ToRoleDto(Role r) => new(
        r.Id, r.Name, r.Description, r.IsSystemRole,
        r.Permissions.Select(p => new PermissionDto(p.Module, p.Action)).ToList());
}
