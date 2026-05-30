using DessertERP.Application.Modules.AccountsPayable.DTOs;
using DessertERP.Application.Modules.AccountsPayable.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/ap")]
[Produces("application/json")]
public class AccountsPayableController : ControllerBase
{
    private readonly IAccountsPayableService _svc;
    public AccountsPayableController(IAccountsPayableService svc) => _svc = svc;

    // ── Vendors ───────────────────────────────────────────────────────────────

    [HttpGet("vendors")]
    public async Task<IActionResult> GetVendors(CancellationToken ct)
        => Ok(await _svc.GetVendorsAsync(ct));

    [HttpPost("vendors")]
    public async Task<IActionResult> CreateVendor([FromBody] CreateVendorRequest req, CancellationToken ct)
        => StatusCode(201, await _svc.CreateVendorAsync(req, ct));

    [HttpPut("vendors/{id:guid}")]
    public async Task<IActionResult> UpdateVendor(Guid id, [FromBody] UpdateVendorRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateVendorAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{id:guid}")]
    public async Task<IActionResult> DeleteVendor(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteVendorAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Purchase Orders ───────────────────────────────────────────────────────

    [HttpGet("purchase-orders")]
    public async Task<IActionResult> GetPurchaseOrders(
        [FromQuery] string? status, [FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetPurchaseOrdersAsync(status, vendorId, ct));

    [HttpGet("purchase-orders/{id:guid}")]
    public async Task<IActionResult> GetPurchaseOrder(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetPurchaseOrderAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("purchase-orders")]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePurchaseOrderAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid id, [FromBody] AddPOLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.AddPOLineAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("purchase-orders/{id:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> RemoveLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemovePOLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/send")]
    public async Task<IActionResult> Send(Guid id, CancellationToken ct)
    {
        try { await _svc.SendPurchaseOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/receive")]
    public async Task<IActionResult> ReceiveGoods(Guid id, [FromBody] IEnumerable<ReceiveGoodsRequest> req, CancellationToken ct)
    {
        try { await _svc.ReceiveGoodsAsync(id, req, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelPurchaseOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("purchase-orders/{id:guid}/generate-invoice")]
    public async Task<IActionResult> GenerateInvoice(Guid id, [FromQuery] string vendorInvoiceRef, CancellationToken ct)
    {
        try { return Ok(await _svc.GenerateInvoiceFromPOAsync(id, vendorInvoiceRef, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AP Invoices ───────────────────────────────────────────────────────────

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices([FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetInvoicesAsync(vendorId, ct));

    [HttpPost("invoices")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateAPInvoiceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateInvoiceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
    {
        try { await _svc.ApproveInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/void")]
    public async Task<IActionResult> Void(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AP Payments ───────────────────────────────────────────────────────────

    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments([FromQuery] Guid? vendorId, CancellationToken ct)
        => Ok(await _svc.GetPaymentsAsync(vendorId, ct));

    [HttpPost("payments")]
    public async Task<IActionResult> CreatePayment([FromBody] CreateAPPaymentRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePaymentAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    [HttpGet("reports/aging")]
    public async Task<IActionResult> GetAgingReport(CancellationToken ct)
        => Ok(await _svc.GetAgingReportAsync(ct));
}
