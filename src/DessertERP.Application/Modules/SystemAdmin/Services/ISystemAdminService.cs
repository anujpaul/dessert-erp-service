using DessertERP.Application.Modules.SystemAdmin.DTOs;

namespace DessertERP.Application.Modules.SystemAdmin.Services;

public interface ISystemAdminService
{
    // Users
    Task<IEnumerable<UserDto>>  GetUsersAsync(CancellationToken ct = default);
    Task<UserDto?>              GetUserAsync(Guid id, CancellationToken ct = default);
    Task<UserDto>               CreateUserAsync(CreateUserRequest req, CancellationToken ct = default);
    Task<UserDto>               UpdateUserAsync(Guid id, UpdateUserRequest req, CancellationToken ct = default);
    Task                        DeactivateUserAsync(Guid id, CancellationToken ct = default);
    Task                        ActivateUserAsync(Guid id, CancellationToken ct = default);
    Task                        ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct = default);

    // Roles
    Task<IEnumerable<RoleDto>>  GetRolesAsync(CancellationToken ct = default);
    Task<RoleDto?>              GetRoleAsync(Guid id, CancellationToken ct = default);
    Task<RoleDto>               CreateRoleAsync(CreateRoleRequest req, CancellationToken ct = default);
    Task<RoleDto>               UpdateRoleAsync(Guid id, UpdateRoleRequest req, CancellationToken ct = default);
    Task                        DeleteRoleAsync(Guid id, CancellationToken ct = default);

    // Audit Log
    Task<IEnumerable<AuditLogDto>> GetAuditLogAsync(
        string? module = null, Guid? userId = null,
        DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default);

    // Org Settings
    Task<OrgSettingsDto>  GetOrgSettingsAsync(CancellationToken ct = default);
    Task<OrgSettingsDto>  UpdateOrgSettingsAsync(UpdateOrgSettingsRequest req, CancellationToken ct = default);
}
