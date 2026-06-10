using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Modules.ProductManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[ApiController]
[Route("api/price-agreements")]
[Authorize]
[Authorize(Policy = PermissionKeys.MarketingAccess)]
[Produces("application/json")]
public class PriceAgreementController : ControllerBase
{
    private readonly IPriceAgreementService _svc;
    private readonly ICurrentOrganizationService _org;

    public PriceAgreementController(IPriceAgreementService svc, ICurrentOrganizationService org)
    {
        _svc = svc;
        _org = org;
    }

    /// <summary>
    /// List price agreements. Filter by productId to see all Item- and SKU-level
    /// agreements for a specific product.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? productId,
        [FromQuery] bool activeOnly = false,
        CancellationToken ct = default)
        => Ok(await _svc.GetAgreementsAsync(productId, activeOnly, ct));

    /// <summary>Create a new price agreement (Item or SKU level, SalesPrice or Cost).</summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePriceAgreementRequest req,
        CancellationToken ct = default)
    {
        try { return StatusCode(201, await _svc.CreateAsync(_org.OrganizationId, req, ct)); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Update name, price type, value, dates, active flag, or notes.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id,
        [FromBody] UpdatePriceAgreementRequest req,
        CancellationToken ct = default)
    {
        try { return Ok(await _svc.UpdateAsync(_org.OrganizationId, id, req, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex)    { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Delete a price agreement.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        try { await _svc.DeleteAsync(_org.OrganizationId, id, ct); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    /// <summary>
    /// Get the effective Sales Price and Cost for a variant today,
    /// showing which agreement won (Variant-level beats Item-level).
    /// </summary>
    [HttpGet("effective-price/{variantId:guid}")]
    public async Task<IActionResult> GetEffectivePrice(
        Guid variantId, CancellationToken ct = default)
    {
        try { return Ok(await _svc.GetEffectivePriceAsync(_org.OrganizationId, variantId, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    /// <summary>
    /// Bulk-set Sales Price or Cost for every SKU of a product in one shot.
    /// Creates/updates one Variant-level agreement per SKU.
    /// </summary>
    [HttpPost("bulk-apply")]
    public async Task<IActionResult> BulkApply(
        [FromQuery] Guid productId,
        [FromBody] CreatePriceAgreementRequest req,
        CancellationToken ct = default)
    {
        try
        {
            var count = await _svc.BulkApplyToProductAsync(_org.OrganizationId, productId, req, ct);
            return Ok(new { applied = count, message = $"Applied to {count} SKU(s)." });
        }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>
    /// Get all variants of a product with their current base values and
    /// any existing agreed Sales Price and Cost.
    /// </summary>
    [HttpGet("suggestions")]
    public async Task<IActionResult> GetSuggestions(
        [FromQuery] Guid productId,
        CancellationToken ct = default)
        => Ok(await _svc.GetVariantSuggestionsAsync(_org.OrganizationId, productId, ct));
}
