using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Modules.Retail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[Authorize(Policy = PermissionKeys.DataAccess)]
[ApiController]
[Route("api/retail-statements")]
[Produces("application/json")]
public class RetailStatementsController : ControllerBase
{
    private readonly IRetailStatementService _service;
    private readonly ICurrentOrganizationService _organization;

    public RetailStatementsController(IRetailStatementService service,
        ICurrentOrganizationService organization)
    {
        _service = service;
        _organization = organization;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatements(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 100,
        CancellationToken ct = default)
        => Ok(await _service.GetStatementsAsync(
            _organization.OrganizationId, page, pageSize, ct));

    [HttpGet("settlements")]
    public async Task<IActionResult> GetSettlements(
        [FromQuery] string? status, CancellationToken ct)
        => Ok(await _service.GetSettlementsAsync(
            _organization.OrganizationId, status, ct));

    [HttpPost("import")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> Import(IFormFile file,
        [FromQuery] bool post = true, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No POSLog XML file was uploaded." });
        if (!Path.GetExtension(file.FileName).Equals(".xml", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "Retail transaction imports must be XML files." });

        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _service.ImportPosLogAsync(
                _organization.OrganizationId, stream, file.FileName, ct);
            if (post && !result.Duplicate)
                await _service.PostOpenStatementsAsync(_organization.OrganizationId, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct)
    {
        try
        {
            await _service.PostStatementAsync(id, ct);
            return Ok(new { message = "Retail statement posted." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("settlements/{id:guid}/settle")]
    public async Task<IActionResult> Settle(Guid id,
        [FromBody] SettleRetailCardRequest request, CancellationToken ct)
    {
        try
        {
            await _service.SettleCardAsync(id, request.BankAccountId,
                request.MerchantFee, request.ProcessorReference,
                User.Identity?.Name ?? "system", ct);
            return Ok(new { message = "Card settlement posted to Cash & Bank." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public record SettleRetailCardRequest(Guid BankAccountId, decimal MerchantFee = 0m,
    string? ProcessorReference = null);
