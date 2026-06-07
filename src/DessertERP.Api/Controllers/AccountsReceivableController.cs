using DessertERP.Application.Modules.AccountsReceivable.DTOs;
using DessertERP.Application.Modules.AccountsReceivable.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/ar")]
[Produces("application/json")]
public class AccountsReceivableController : ControllerBase
{
    private readonly IAccountsReceivableService _svc;
    public AccountsReceivableController(IAccountsReceivableService svc) => _svc = svc;

    // ── Customers ─────────────────────────────────────────────────────────────

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers(CancellationToken ct)
        => Ok(await _svc.GetCustomersAsync(ct));

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest req, CancellationToken ct)
        => StatusCode(201, await _svc.CreateCustomerAsync(req, ct));

    [HttpPut("customers/{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateCustomerAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    //[HttpGet("customers/{id:guid}/ledger")]
    //public async Task<IActionResult> GetCustomerLedger(Guid id, CancellationToken ct)
    //{
    //    try { return Ok(await _svc.GetCustomerLedgerAsync(id, ct)); }
    //    catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    //}

    // ── Sales Orders ──────────────────────────────────────────────────────────

    [HttpGet("sales-orders")]
    public async Task<IActionResult> GetSalesOrders(
        [FromQuery] string? status, [FromQuery] Guid? customerId, CancellationToken ct)
        => Ok(await _svc.GetSalesOrdersAsync(status, customerId, ct));

    [HttpGet("sales-orders/{id:guid}")]
    public async Task<IActionResult> GetSalesOrder(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetSalesOrderAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("sales-orders")]
    public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateSalesOrderAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid id, [FromBody] AddSalesOrderLineRequest req, CancellationToken ct)
    {
        Console.WriteLine($"Adding line to order {id}: ProductVariantId={req.ProductVariantId}, Quantity={req.Quantity}, OverrideUnitPrice={req.OverrideUnitPrice}, DiscountPct={req.DiscountPct}");
        try { return Ok(await _svc.AddSalesOrderLineAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message, detail = ex.InnerException?.Message }); }
    }

    [HttpDelete("sales-orders/{id:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> RemoveLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemoveSalesOrderLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Apply a flat discount % to every line on a Draft order (used by coupon application).</summary>
    [HttpPost("sales-orders/{id:guid}/apply-discount")]
    public async Task<IActionResult> ApplyDiscount(Guid id, [FromBody] ApplyDiscountRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApplyDiscountToOrderAsync(id, req.DiscountPct, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken ct)
    {
        try { await _svc.ConfirmSalesOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/picking")]
    public async Task<IActionResult> StartPicking(Guid id, CancellationToken ct)
    {
        try { await _svc.StartPickingAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/ship")]
    public async Task<IActionResult> Ship(Guid id, [FromBody] ShipOrderRequest req, CancellationToken ct)
    {
        try { await _svc.ShipSalesOrderAsync(id, req, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelSalesOrderAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AR Invoices ───────────────────────────────────────────────────────────

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices([FromQuery] Guid? customerId, CancellationToken ct)
        => Ok(await _svc.GetInvoicesAsync(customerId, ct));

    [HttpPost("invoices")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateARInvoiceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateInvoiceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("sales-orders/{id:guid}/generate-invoice")]
    public async Task<IActionResult> GenerateInvoice(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GenerateInvoiceFromOrderAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/issue")]
    public async Task<IActionResult> IssueInvoice(Guid id, CancellationToken ct)
    {
        try { await _svc.IssueInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("invoices/{id:guid}/void")]
    public async Task<IActionResult> VoidInvoice(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidInvoiceAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── AR Payments ───────────────────────────────────────────────────────────

    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments([FromQuery] Guid? customerId, CancellationToken ct)
        => Ok(await _svc.GetPaymentsAsync(customerId, ct));

    [HttpPost("payments")]
    public async Task<IActionResult> CreatePayment([FromBody] CreateARPaymentRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePaymentAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    [HttpGet("aging")]
    public async Task<IActionResult> GetAging(CancellationToken ct)
        => Ok(await _svc.GetAgingReportAsync(ct));

    [HttpGet("customers/{id:guid}/ledger")]
    public async Task<IActionResult> GetCustomerLedger(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetCustomerLedgerAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Customer Addresses ────────────────────────────────────────────────────

    [HttpGet("customers/{customerId:guid}/addresses")]
    public async Task<IActionResult> GetAddresses(Guid customerId, CancellationToken ct)
        => Ok(await _svc.GetCustomerAddressesAsync(customerId, ct));

    [HttpPost("customers/{customerId:guid}/addresses")]
    public async Task<IActionResult> CreateAddress(Guid customerId, [FromBody] SaveCustomerAddressRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveCustomerAddressAsync(customerId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("customers/{customerId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid customerId, Guid addressId, [FromBody] SaveCustomerAddressRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveCustomerAddressAsync(customerId, addressId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("customers/{customerId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> DeleteAddress(Guid customerId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.DeleteCustomerAddressAsync(customerId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("customers/{customerId:guid}/addresses/{addressId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryAddress(Guid customerId, Guid addressId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryCustomerAddressAsync(customerId, addressId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Customer Contacts ─────────────────────────────────────────────────────

    [HttpGet("customers/{customerId:guid}/contacts")]
    public async Task<IActionResult> GetContacts(Guid customerId, CancellationToken ct)
        => Ok(await _svc.GetCustomerContactsAsync(customerId, ct));

    [HttpPost("customers/{customerId:guid}/contacts")]
    public async Task<IActionResult> CreateContact(Guid customerId, [FromBody] SaveCustomerContactRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.SaveCustomerContactAsync(customerId, null, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("customers/{customerId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> UpdateContact(Guid customerId, Guid contactId, [FromBody] SaveCustomerContactRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveCustomerContactAsync(customerId, contactId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("customers/{customerId:guid}/contacts/{contactId:guid}")]
    public async Task<IActionResult> DeleteContact(Guid customerId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.DeleteCustomerContactAsync(customerId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("customers/{customerId:guid}/contacts/{contactId:guid}/set-primary")]
    public async Task<IActionResult> SetPrimaryContact(Guid customerId, Guid contactId, CancellationToken ct)
    {
        try { await _svc.SetPrimaryCustomerContactAsync(customerId, contactId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }
}
