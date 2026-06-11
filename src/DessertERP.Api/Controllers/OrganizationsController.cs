using DessertERP.Application.Modules.Organization.DTOs;
using DessertERP.Application.Modules.Organization.Services;
using DessertERP.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/organizations")]
[Produces("application/json")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _svc;
    public OrganizationsController(IOrganizationService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _svc.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationRequest req, CancellationToken ct)
    {
        try { await _svc.UpdateAsync(id, req, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("{id:guid}/suspend")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> Suspend(Guid id, CancellationToken ct)
    {
        try { await _svc.SuspendAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        try { await _svc.ActivateAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = PermissionKeys.SystemSettingsManage)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
