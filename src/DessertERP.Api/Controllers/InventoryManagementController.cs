using DessertERP.Application.Modules.InventoryManagement.DTOs;
using DessertERP.Application.Modules.InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/inventory")]
public class InventoryManagementController : ControllerBase
{
    private readonly IInventoryManagementService _svc;
    public InventoryManagementController(IInventoryManagementService svc) => _svc = svc;

    // ── Summary ──────────────────────────────────────────────────────────────

    /// <summary>Dashboard summary: total SKUs, low stock count, out-of-stock, total value.</summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
    {
        var dto = await _svc.GetSummaryAsync(ct);
        return Ok(dto);
    }

    // ── Items ────────────────────────────────────────────────────────────────

    /// <summary>
    /// List all inventory items.
    /// Optional: ?search=sku/name  ?filter=low-stock|out-of-stock|on-order
    /// </summary>
    [HttpGet("items")]
    public async Task<IActionResult> GetItems(
        [FromQuery] string? search = null,
        [FromQuery] string? filter = null,
        [FromQuery] string? category = null,
        [FromQuery] string? brand = null,
        [FromQuery] string? location = null,
        [FromQuery] decimal? minOnHand = null,
        [FromQuery] decimal? maxOnHand = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool descending = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var items = await _svc.GetItemsAsync(
            search, filter, category, brand, location, minOnHand, maxOnHand,
            sortBy, descending, page, pageSize, ct);
        return Ok(items);
    }

    [HttpGet("filter-options")]
    public async Task<IActionResult> GetFilterOptions(CancellationToken ct)
        => Ok(await _svc.GetFilterOptionsAsync(ct));

    /// <summary>Get a single inventory record by its ID.</summary>
    [HttpGet("items/{id:guid}")]
    public async Task<IActionResult> GetItem(Guid id, CancellationToken ct)
    {
        try
        {
            var item = await _svc.GetItemAsync(id, ct);
            return Ok(item);
        }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Stock adjustments ────────────────────────────────────────────────────

    /// <summary>
    /// Manual stock adjustment.
    /// Positive qty = stock in (AdjustmentIn), Negative qty = stock out (AdjustmentOut).
    /// </summary>
    [HttpPost("items/{id:guid}/adjust")]
    public async Task<IActionResult> AdjustStock(Guid id, [FromBody] AdjustStockRequest req, CancellationToken ct)
    {
        try
        {
            var item = await _svc.AdjustStockAsync(id, req, ct);
            return Ok(item);
        }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Cycle count — set the absolute on-hand quantity for a SKU.</summary>
    [HttpPost("items/{id:guid}/set-on-hand")]
    public async Task<IActionResult> SetOnHand(Guid id, [FromBody] SetOnHandRequest req, CancellationToken ct)
    {
        try
        {
            var item = await _svc.SetOnHandAsync(id, req, ct);
            return Ok(item);
        }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Update reorder point, min/max stock, and bin location.</summary>
    [HttpPut("items/{id:guid}/thresholds")]
    public async Task<IActionResult> UpdateThresholds(Guid id, [FromBody] UpdateThresholdsRequest req, CancellationToken ct)
    {
        try
        {
            var item = await _svc.UpdateThresholdsAsync(id, req, ct);
            return Ok(item);
        }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Transaction ledger ───────────────────────────────────────────────────

    /// <summary>Transaction history for a single product variant. ?take=100</summary>
    [HttpGet("items/{variantId:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(
        Guid variantId,
        [FromQuery] int take = 100,
        CancellationToken ct = default)
    {
        var txns = await _svc.GetTransactionsAsync(variantId, take, ct);
        return Ok(txns);
    }

    /// <summary>Most recent transactions across all SKUs. ?take=50</summary>
    [HttpGet("transactions/recent")]
    public async Task<IActionResult> GetRecentTransactions(
        [FromQuery] int take = 50,
        CancellationToken ct = default)
    {
        var txns = await _svc.GetRecentTransactionsAsync(take, ct);
        return Ok(txns);
    }

    // ── Reports ──────────────────────────────────────────────────────────────

    /// <summary>All items at or below their reorder point, with preferred vendor info.</summary>
    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock(CancellationToken ct)
    {
        var items = await _svc.GetLowStockAsync(ct);
        return Ok(items);
    }

    /// <summary>Stock valuation grouped by product category.</summary>
    [HttpGet("valuation")]
    public async Task<IActionResult> GetValuation(CancellationToken ct)
    {
        var rows = await _svc.GetValuationByCategory(ct);
        return Ok(rows);
    }
}
