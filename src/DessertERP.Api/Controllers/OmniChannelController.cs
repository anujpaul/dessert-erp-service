using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.Retail.DTOs;
using DessertERP.Application.Modules.Retail.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/omnichannel")]
[Produces("application/json")]
public class OmniChannelController : ControllerBase
{
    private readonly IRetailService _svc;
    private readonly ICurrentOrganizationService _org;

    public OmniChannelController(IRetailService svc, ICurrentOrganizationService org)
    {
        _svc = svc;
        _org = org;
    }

    // ── Stores ────────────────────────────────────────────────────────────────

    [HttpGet("stores")]
    public async Task<IActionResult> GetStores(CancellationToken ct)
        => Ok(await _svc.GetStoresAsync(_org.OrganizationId, ct));

    [HttpGet("stores/{id:guid}")]
    public async Task<IActionResult> GetStore(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetStoreAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("stores")]
    public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateStoreAsync(_org.OrganizationId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("stores/{id:guid}")]
    public async Task<IActionResult> UpdateStore(Guid id, [FromBody] UpdateStoreRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateStoreAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("stores/{id:guid}/toggle")]
    public async Task<IActionResult> ToggleStore(Guid id, [FromQuery] bool active, CancellationToken ct)
    {
        try { await _svc.ToggleStoreAsync(id, active, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Transactions ──────────────────────────────────────────────────────────

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50,
        [FromQuery] string? status = null, [FromQuery] string? channel = null,
        CancellationToken ct = default)
        => Ok(await _svc.GetTransactionsAsync(_org.OrganizationId, page, pageSize, status, ct));

    [HttpGet("transactions/{id:guid}")]
    public async Task<IActionResult> GetTransaction(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetTransactionAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("transactions")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreatePOSTransactionRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateTransactionAsync(_org.OrganizationId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Online / delivery order intake — can be called without cashier context.</summary>
    [HttpPost("orders/online")]
    [AllowAnonymous]  // external systems (website, app) can POST without JWT
    public async Task<IActionResult> CreateOnlineOrder([FromBody] OnlineOrderRequest req, CancellationToken ct)
    {
        // Resolve org from header or default — for now require X-Organization-Id header
        var orgHeader = Request.Headers["X-Organization-Id"].FirstOrDefault();
        if (!Guid.TryParse(orgHeader, out var orgId))
            return BadRequest(new { error = "X-Organization-Id header is required." });

        try { return StatusCode(201, await _svc.CreateOnlineOrderAsync(orgId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transactions/{id:guid}/process")]
    public async Task<IActionResult> ProcessTransaction(Guid id, CancellationToken ct)
    {
        try { await _svc.ProcessTransactionAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transactions/{id:guid}/void")]
    public async Task<IActionResult> VoidTransaction(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidTransactionAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPatch("transactions/{id:guid}/fulfillment")]
    public async Task<IActionResult> UpdateFulfillment(Guid id,
        [FromBody] UpdateFulfillmentStatusRequest req, CancellationToken ct)
    {
        try { await _svc.UpdateFulfillmentStatusAsync(id, req.FulfillmentStatus, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Promotions ────────────────────────────────────────────────────────────

    [HttpGet("promotions")]
    public async Task<IActionResult> GetPromotions(CancellationToken ct)
        => Ok(await _svc.GetPromotionsAsync(_org.OrganizationId, ct));

    [HttpGet("promotions/{id:guid}")]
    public async Task<IActionResult> GetPromotion(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetPromotionAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("promotions")]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePromotionAsync(_org.OrganizationId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("promotions/{id:guid}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] CreatePromotionRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdatePromotionAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("promotions/{id:guid}/toggle")]
    public async Task<IActionResult> TogglePromotion(Guid id, [FromQuery] bool active, CancellationToken ct)
    {
        try { await _svc.TogglePromotionAsync(id, active, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Coupons ───────────────────────────────────────────────────────────────

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCoupons(
        [FromQuery] Guid? promotionId = null, CancellationToken ct = default)
        => Ok(await _svc.GetCouponsAsync(_org.OrganizationId, promotionId, ct));

    [HttpPost("coupons")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateCouponAsync(_org.OrganizationId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("coupons/bulk")]
    public async Task<IActionResult> BulkCreateCoupons([FromBody] BulkCreateCouponsRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.BulkCreateCouponsAsync(_org.OrganizationId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("coupons/validate")]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponRequest req, CancellationToken ct)
        => Ok(await _svc.ValidateCouponAsync(_org.OrganizationId, req, ct));

    [HttpPost("coupons/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCoupon(Guid id, CancellationToken ct)
    {
        try { await _svc.DeactivateCouponAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
        => Ok(await _svc.GetSummaryAsync(_org.OrganizationId, from, to, ct));
}
