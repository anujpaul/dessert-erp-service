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

    /// <summary>Record a goods receipt (partial or full). Can be called multiple times on the same PO.</summary>
    [HttpPost("purchase-orders/{id:guid}/receipts")]
    public async Task<IActionResult> RecordReceipt(Guid id, [FromBody] RecordReceiptRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.RecordReceiptAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>List all goods-receipt events for a PO.</summary>
    [HttpGet("purchase-orders/{id:guid}/receipts")]
    public async Task<IActionResult> GetReceipts(Guid id, CancellationToken ct)
        => Ok(await _svc.GetReceiptsAsync(id, ct));

    [HttpPost("purchase-orders/{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken ct)
    {
        try { await _svc.ClosePurchaseOrderAsync(id, ct); return NoContent(); }
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

    /// <summary>Raise a prepayment / advance-payment invoice against a PO before goods arrive.</summary>
    [HttpPost("invoices/prepayment")]
    public async Task<IActionResult> CreatePrepayment([FromBody] CreatePrepaymentInvoiceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePrepaymentInvoiceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Run three-way match (PO → GRN → Invoice). Returns match result and updates MatchStatus.</summary>
    [HttpPost("invoices/{id:guid}/match")]
    public async Task<IActionResult> RunMatch(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.RunThreeWayMatchAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Manager bypass: override a match exception with a documented reason.</summary>
    [HttpPost("invoices/{id:guid}/bypass-match")]
    public async Task<IActionResult> BypassMatch(Guid id, [FromBody] BypassMatchRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.BypassMatchAsync(id, req.Reason, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Apply an existing prepayment invoice to a standard AP invoice (offset).</summary>
    [HttpPost("invoices/{id:guid}/apply-prepayment")]
    public async Task<IActionResult> ApplyPrepayment(Guid id, [FromBody] ApplyPrepaymentRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApplyPrepaymentAsync(id, req.PrepaymentInvoiceId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/approve")]
    public async Task<IActionResult> ApproveInvoice(Guid id, CancellationToken ct)
    {
        try { await _svc.ApproveInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/void")]
    public async Task<IActionResult> VoidInvoice(Guid id, CancellationToken ct)
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

    [HttpGet("aging")]
    public async Task<IActionResult> GetAging(CancellationToken ct)
        => Ok(await _svc.GetAgingReportAsync(ct));

    [HttpGet("vendors/{id:guid}/ledger")]
    public async Task<IActionResult> GetVendorLedger(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetVendorLedgerAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Vendor Addresses ──────────────────────────────────────────────────────

    [HttpGet("vendors/{vendorId:guid}/addresses")]
    public async Task<IActionResult> GetVendorAddresses(Guid vendorId, CancellationToken ct)
        => Ok(await _svc.GetVendorAddressesAsync(vendorId, ct));

    [HttpPost("vendors/{vendorId:guid}/addresses")]
    public async Task<IActionResult> CreateVendorAddress(Guid vendorId, [FromBody] SaveVendorAddressRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveVendorAddressAsync(vendorId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("vendors/{vendorId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> UpdateVendorAddress(Guid vendorId, Guid addressId, [FromBody] SaveVendorAddressRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveVendorAddressAsync(vendorId, addressId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{vendorId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> DeleteVendorAddress(Guid vendorId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.DeleteVendorAddressAsync(vendorId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("vendors/{vendorId:guid}/addresses/{addressId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryVendorAddress(Guid vendorId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryVendorAddressAsync(vendorId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Vendor Contacts ───────────────────────────────────────────────────────

    [HttpGet("vendors/{vendorId:guid}/contacts")]
    public async Task<IActionResult> GetVendorContacts(Guid vendorId, CancellationToken ct)
        => Ok(await _svc.GetVendorContactsAsync(vendorId, ct));

    [HttpPost("vendors/{vendorId:guid}/contacts")]
    public async Task<IActionResult> CreateVendorContact(Guid vendorId, [FromBody] SaveVendorContactRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveVendorContactAsync(vendorId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("vendors/{vendorId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> UpdateVendorContact(Guid vendorId, Guid contactId, [FromBody] SaveVendorContactRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveVendorContactAsync(vendorId, contactId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("vendors/{vendorId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> DeleteVendorContact(Guid vendorId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.DeleteVendorContactAsync(vendorId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("vendors/{vendorId:guid}/contacts/{contactId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryVendorContact(Guid vendorId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryVendorContactAsync(vendorId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }
}
