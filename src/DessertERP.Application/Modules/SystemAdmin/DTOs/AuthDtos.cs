namespace DessertERP.Application.Modules.SystemAdmin.DTOs;

// ── Auth ──────────────────────────────────────────────────────────────────────
public record LoginRequest(string Username, string Password);

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User);

public record RefreshTokenRequest(string RefreshToken);

// ── Users ─────────────────────────────────────────────────────────────────────
public record UserDto(
    Guid Id, Guid OrganizationId,
    string Username, string Email, string FullName,
    string Status, DateTime? LastLoginAt,
    IReadOnlyList<string> Roles, DateTime CreatedAt);

public record CreateUserRequest(
    string Username, string Email, string FullName,
    string Password, IList<Guid> RoleIds);

public record UpdateUserRequest(string Email, string FullName, IList<Guid> RoleIds);

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public record ResetPasswordRequest(Guid UserId, string NewPassword);

// ── Roles ─────────────────────────────────────────────────────────────────────
public record RoleDto(
    Guid Id, string Name, string Description,
    bool IsSystemRole,
    IReadOnlyList<PermissionDto> Permissions);

public record PermissionDto(string Module, string Action);

public record CreateRoleRequest(string Name, string Description, IList<PermissionDto> Permissions);

public record UpdateRoleRequest(string Name, string Description, IList<PermissionDto> Permissions);

// ── Audit Log ─────────────────────────────────────────────────────────────────
public record AuditLogDto(
    Guid Id, string Username, string Module, string Action,
    string? EntityId, string? EntityType,
    string? OldValues, string? NewValues,
    string? IpAddress, DateTime OccurredAt);

// ── Org Settings ──────────────────────────────────────────────────────────────
public record OrgSettingsDto(
    Guid Id, string Code, string Name, string? LogoUrl,
    string BaseCurrency, string DefaultCurrency, string? Timezone,
    string? TaxId, string? Address, string? Phone, string? Email);

public record UpdateOrgSettingsRequest(
    string Name, string? LogoUrl,
    string DefaultCurrency, string? Timezone,
    string? TaxId, string? Address, string? Phone, string? Email);
