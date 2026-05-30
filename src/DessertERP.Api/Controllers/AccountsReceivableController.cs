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

    [HttpGet("reports/aging")]
    public async Task<IActionResult> GetAgingReport(CancellationToken ct)
        => Ok(await _svc.GetAgingReportAsync(ct));
}
