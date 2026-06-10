using DessertERP.Application.Modules.SystemAdmin.DTOs;
using DessertERP.Application.Modules.SystemAdmin.Services;
using DessertERP.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[ApiController]
[Route("api/sysadmin")]
[Authorize(Roles = "Admin")]
[Authorize(Policy = PermissionKeys.SystemAccess)]
public class SystemAdminController : ControllerBase
{
    private readonly ISystemAdminService _svc;
    public SystemAdminController(ISystemAdminService svc) => _svc = svc;

    // ── Users ────────────────────────────────────────────────────────────────

    [HttpGet("users")]
    [Authorize(Policy = PermissionKeys.SystemUsersView)]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
        => Ok(await _svc.GetUsersAsync(ct));

    [HttpGet("users/{id:guid}")]
    [Authorize(Policy = PermissionKeys.SystemUsersView)]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken ct)
    {
        var user = await _svc.GetUserAsync(id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost("users")]
    [Authorize(Policy = PermissionKeys.SystemUsersManage)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        try
        {
            var user = await _svc.CreateUserAsync(req, ct);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("users/{id:guid}")]
    [Authorize(Policy = PermissionKeys.SystemUsersManage)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest req, CancellationToken ct)
    {
        try   { return Ok(await _svc.UpdateUserAsync(id, req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("users/{id:guid}/activate")]
    [Authorize(Policy = PermissionKeys.SystemUsersManage)]
    public async Task<IActionResult> ActivateUser(Guid id, CancellationToken ct)
    {
        await _svc.ActivateUserAsync(id, ct);
        return NoContent();
    }

    [HttpPost("users/{id:guid}/deactivate")]
    [Authorize(Policy = PermissionKeys.SystemUsersManage)]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken ct)
    {
        await _svc.DeactivateUserAsync(id, ct);
        return NoContent();
    }

    [HttpPost("users/reset-password")]
    [Authorize(Policy = PermissionKeys.SystemUsersManage)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req, CancellationToken ct)
    {
        try   { await _svc.ResetPasswordAsync(req, ct); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Roles ─────────────────────────────────────────────────────────────────

    [HttpGet("roles")]
    [Authorize(Policy = PermissionKeys.SystemRolesView)]
    public async Task<IActionResult> GetRoles(CancellationToken ct)
        => Ok(await _svc.GetRolesAsync(ct));

    [HttpGet("roles/{id:guid}")]
    [Authorize(Policy = PermissionKeys.SystemRolesView)]
    public async Task<IActionResult> GetRole(Guid id, CancellationToken ct)
    {
        var role = await _svc.GetRoleAsync(id, ct);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpPost("roles")]
    [Authorize(Policy = PermissionKeys.SystemRolesManage)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest req, CancellationToken ct)
    {
        try
        {
            var role = await _svc.CreateRoleAsync(req, ct);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("roles/{id:guid}")]
    [Authorize(Policy = PermissionKeys.SystemRolesManage)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest req, CancellationToken ct)
    {
        try   { return Ok(await _svc.UpdateRoleAsync(id, req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("roles/{id:guid}")]
    [Authorize(Policy = PermissionKeys.SystemRolesManage)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken ct)
    {
        try   { await _svc.DeleteRoleAsync(id, ct); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Audit Log ─────────────────────────────────────────────────────────────

    [HttpGet("audit-log")]
    [Authorize(Policy = PermissionKeys.SystemAuditView)]
    public async Task<IActionResult> GetAuditLog(
        [FromQuery] string? module,
        [FromQuery] Guid?   userId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
        => Ok(await _svc.GetAuditLogAsync(module, userId, from, to, ct));

    // ── Org Settings ──────────────────────────────────────────────────────────

    [HttpGet("org-settings")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> GetOrgSettings(CancellationToken ct)
        => Ok(await _svc.GetOrgSettingsAsync(ct));

    [HttpPut("org-settings")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> UpdateOrgSettings([FromBody] UpdateOrgSettingsRequest req, CancellationToken ct)
    {
        try   { return Ok(await _svc.UpdateOrgSettingsAsync(req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }
}
