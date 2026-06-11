using DessertERP.Application.Modules.ProductManagement.DTOs;
using DessertERP.Application.Modules.ProductManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/pm")]
public class ProductManagementController : ControllerBase
{
    private readonly IProductManagementService _pm;

    public ProductManagementController(IProductManagementService pm) => _pm = pm;

    // ── Categories ────────────────────────────────────────────────────────────

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
        => Ok(await _pm.GetCategoriesAsync(ct));

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest req, CancellationToken ct)
        => Ok(await _pm.CreateCategoryAsync(req, ct));

    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
    {
        await _pm.DeleteCategoryAsync(id, ct);
        return NoContent();
    }

    // ── Brands ────────────────────────────────────────────────────────────────

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands(CancellationToken ct)
        => Ok(await _pm.GetBrandsAsync(ct));

    [HttpPost("brands")]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequest req, CancellationToken ct)
        => Ok(await _pm.CreateBrandAsync(req, ct));

    [HttpDelete("brands/{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteBrand(Guid id, CancellationToken ct)
    {
        try { await _pm.DeleteBrandAsync(id, ct); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("categories/{id:guid}/force")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteCategoryGuarded(Guid id, CancellationToken ct)
    {
        try { await _pm.DeleteCategoryAsync(id, ct); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Products ──────────────────────────────────────────────────────────────

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? brandId,
        [FromQuery] string? status,
        [FromQuery] string? search,
        CancellationToken ct)
        => Ok(await _pm.GetProductsAsync(
            categoryId?.ToString(), brandId?.ToString(), status, search, ct));

    [HttpGet("products/{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken ct)
    {
        var product = await _pm.GetProductAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest req, CancellationToken ct)
        => Ok(await _pm.CreateProductAsync(req, ct));

    [HttpPut("products/{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest req, CancellationToken ct)
    {
        await _pm.UpdateProductAsync(id, req, ct);
        return NoContent();
    }

    [HttpPost("products/{id:guid}/status/{status}")]
    public async Task<IActionResult> ChangeProductStatus(Guid id, string status, CancellationToken ct)
    {
        await _pm.ChangeProductStatusAsync(id, status, ct);
        return NoContent();
    }

    // ── Variants ──────────────────────────────────────────────────────────────

    [HttpPost("products/{productId:guid}/variants")]
    public async Task<IActionResult> AddVariant(Guid productId, [FromBody] CreateVariantRequest req, CancellationToken ct)
        => Ok(await _pm.AddVariantAsync(productId, req, ct));

    [HttpPut("variants/{variantId:guid}/pricing")]
    public async Task<IActionResult> UpdateVariantPricing(Guid variantId, [FromBody] UpdateVariantPricingRequest req, CancellationToken ct)
    {
        await _pm.UpdateVariantPricingAsync(variantId, req, ct);
        return NoContent();
    }

    [HttpPost("variants/{variantId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateVariant(Guid variantId, CancellationToken ct)
    {
        await _pm.DeactivateVariantAsync(variantId, ct);
        return NoContent();
    }

    // ── Inventory ─────────────────────────────────────────────────────────────

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory([FromQuery] bool needsReorder = false, CancellationToken ct = default)
        => Ok(await _pm.GetInventoryAsync(needsReorder, ct));

    [HttpPost("inventory/{variantId:guid}/adjust")]
    public async Task<IActionResult> AdjustInventory(Guid variantId, [FromBody] AdjustInventoryRequest req, CancellationToken ct)
    {
        await _pm.AdjustInventoryAsync(variantId, req, ct);
        return NoContent();
    }

    [HttpPost("inventory/{variantId:guid}/set")]
    public async Task<IActionResult> SetInventory(Guid variantId, [FromBody] SetInventoryRequest req, CancellationToken ct)
    {
        await _pm.SetInventoryAsync(variantId, req, ct);
        return NoContent();
    }

    [HttpPut("inventory/{variantId:guid}/thresholds")]
    public async Task<IActionResult> UpdateThresholds(Guid variantId, [FromBody] UpdateThresholdsRequest req, CancellationToken ct)
    {
        await _pm.UpdateThresholdsAsync(variantId, req, ct);
        return NoContent();
    }

    // ── Preferred Vendor ──────────────────────────────────────────────────────

    [HttpPut("products/{id:guid}/preferred-vendor")]
    public async Task<IActionResult> SetPreferredVendor(Guid id, [FromBody] SetPreferredVendorRequest req, CancellationToken ct)
    {
        try { await _pm.SetPreferredVendorAsync(id, req.VendorId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Variant Search (for AR + AP picker) ───────────────────────────────────

    [HttpGet("variants/search")]
    public async Task<IActionResult> SearchVariants([FromQuery] string? q, CancellationToken ct)
        => Ok(await _pm.SearchVariantsAsync(q ?? string.Empty, ct));
}
