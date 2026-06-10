using System.Xml.Linq;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Modules.Marketing.DTOs;
using DessertERP.Application.Modules.Marketing.Services;
using DessertERP.Domain.Modules.Retail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Api.Controllers;

[ApiController]
[Route("api/marketing")]
[Authorize]
[Authorize(Policy = PermissionKeys.MarketingAccess)]
public class MarketingController : ControllerBase
{
    private readonly IMarketingService _svc;
    private readonly ICurrentOrganizationService _org;
    private readonly IAppDbContext _db;

    public MarketingController(IMarketingService svc, ICurrentOrganizationService org, IAppDbContext db)
    {
        _svc = svc;
        _org = org;
        _db  = db;
    }

    // ── Campaigns ──────────────────────────────────────────────────────────────

    [HttpGet("campaigns")]
    public async Task<IActionResult> GetCampaigns()
        => Ok(await _svc.GetCampaignsAsync(_org.OrganizationId));

    [HttpGet("campaigns/{id:guid}")]
    public async Task<IActionResult> GetCampaign(Guid id)
    {
        try { return Ok(await _svc.GetCampaignAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpPost("campaigns")]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest req)
    {
        try { return Ok(await _svc.CreateCampaignAsync(_org.OrganizationId, req)); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPut("campaigns/{id:guid}")]
    public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] UpdateCampaignRequest req)
    {
        try { return Ok(await _svc.UpdateCampaignAsync(_org.OrganizationId, id, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPatch("campaigns/{id:guid}/status")]
    public async Task<IActionResult> SetCampaignStatus(Guid id, [FromBody] SetCampaignStatusRequest req)
    {
        try { return Ok(await _svc.SetCampaignStatusAsync(_org.OrganizationId, id, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPost("campaigns/{id:guid}/metrics")]
    public async Task<IActionResult> RecordMetrics(Guid id, [FromBody] RecordCampaignMetricsRequest req)
    {
        try { return Ok(await _svc.RecordMetricsAsync(_org.OrganizationId, id, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpDelete("campaigns/{id:guid}")]
    public async Task<IActionResult> DeleteCampaign(Guid id)
    {
        try { await _svc.DeleteCampaignAsync(_org.OrganizationId, id); return NoContent(); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    // ── Promotions ─────────────────────────────────────────────────────────────

    [HttpGet("promotions")]
    public async Task<IActionResult> GetPromotions()
        => Ok(await _svc.GetPromotionsAsync(_org.OrganizationId));

    [HttpGet("promotions/{id:guid}")]
    public async Task<IActionResult> GetPromotion(Guid id)
    {
        try { return Ok(await _svc.GetPromotionAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpPost("promotions")]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionRequest req)
    {
        try { return Ok(await _svc.CreatePromotionAsync(_org.OrganizationId, req)); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPut("promotions/{id:guid}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequest req)
    {
        try { return Ok(await _svc.UpdatePromotionAsync(_org.OrganizationId, id, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPatch("promotions/{id:guid}/toggle")]
    public async Task<IActionResult> TogglePromotion(Guid id)
    {
        try { return Ok(await _svc.TogglePromotionAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpDelete("promotions/{id:guid}")]
    public async Task<IActionResult> DeletePromotion(Guid id)
    {
        try { await _svc.DeletePromotionAsync(_org.OrganizationId, id); return NoContent(); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    // ── Coupons ────────────────────────────────────────────────────────────────

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCoupons([FromQuery] Guid? promotionId)
        => Ok(await _svc.GetCouponsAsync(_org.OrganizationId, promotionId));

    [HttpPost("coupons")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest req)
    {
        try { return Ok(await _svc.CreateCouponAsync(_org.OrganizationId, req)); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPost("coupons/bulk")]
    public async Task<IActionResult> BulkCreateCoupons([FromBody] BulkCreateCouponsRequest req)
    {
        try { return Ok(await _svc.BulkCreateCouponsAsync(_org.OrganizationId, req)); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpGet("coupons/validate")]
    public async Task<IActionResult> ValidateCoupon([FromQuery] string code, [FromQuery] decimal orderAmount = 0)
        => Ok(await _svc.ValidateCouponAsync(_org.OrganizationId, code, orderAmount));

    [HttpPatch("coupons/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCoupon(Guid id)
    {
        try { return Ok(await _svc.DeactivateCouponAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    // ── Loyalty Programs ───────────────────────────────────────────────────────

    [HttpGet("loyalty")]
    public async Task<IActionResult> GetLoyaltyPrograms()
        => Ok(await _svc.GetLoyaltyProgramsAsync(_org.OrganizationId));

    [HttpGet("loyalty/{id:guid}")]
    public async Task<IActionResult> GetLoyaltyProgram(Guid id)
    {
        try { return Ok(await _svc.GetLoyaltyProgramAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpPost("loyalty")]
    public async Task<IActionResult> CreateLoyaltyProgram([FromBody] CreateLoyaltyProgramRequest req)
    {
        try { return Ok(await _svc.CreateLoyaltyProgramAsync(_org.OrganizationId, req)); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPut("loyalty/{id:guid}")]
    public async Task<IActionResult> UpdateLoyaltyProgram(Guid id, [FromBody] UpdateLoyaltyProgramRequest req)
    {
        try { return Ok(await _svc.UpdateLoyaltyProgramAsync(_org.OrganizationId, id, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPatch("loyalty/{id:guid}/toggle")]
    public async Task<IActionResult> ToggleLoyaltyProgram(Guid id)
    {
        try { return Ok(await _svc.ToggleLoyaltyProgramAsync(_org.OrganizationId, id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    // ── Loyalty Accounts ───────────────────────────────────────────────────────

    [HttpGet("loyalty/{programId:guid}/accounts")]
    public async Task<IActionResult> GetLoyaltyAccounts(Guid programId)
        => Ok(await _svc.GetLoyaltyAccountsAsync(_org.OrganizationId, programId));

    [HttpGet("loyalty/accounts/customer/{customerId:guid}")]
    public async Task<IActionResult> GetCustomerAccount(Guid customerId)
    {
        try { return Ok(await _svc.GetCustomerLoyaltyAccountAsync(_org.OrganizationId, customerId)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpPost("loyalty/{programId:guid}/enroll")]
    public async Task<IActionResult> EnrollCustomer(Guid programId, [FromBody] EnrollCustomerRequest req)
    {
        try { return Ok(await _svc.EnrollCustomerAsync(_org.OrganizationId, programId, req)); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPost("loyalty/{programId:guid}/award")]
    public async Task<IActionResult> AwardPoints(Guid programId, [FromBody] AwardPointsRequest req)
    {
        try { return Ok(await _svc.AwardPointsAsync(_org.OrganizationId, programId, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (Exception e) { return BadRequest(e.Message); }
    }

    [HttpPost("loyalty/{programId:guid}/redeem")]
    public async Task<IActionResult> RedeemPoints(Guid programId, [FromBody] RedeemPointsRequest req)
    {
        try { return Ok(await _svc.RedeemPointsAsync(_org.OrganizationId, programId, req)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return BadRequest(e.Message); }
    }

    // ── Promo Items Export ─────────────────────────────────────────────────────

    /// <summary>
    /// Exports all products that have an active promotion applied,
    /// grouped by Category → SubCategory → Product with variant/promo details.
    /// GET /api/marketing/export/promo-items
    /// </summary>
    [HttpGet("export/promo-items")]
    public async Task<IActionResult> ExportPromoItems()
    {
        var orgId = _org.OrganizationId;
        var today = DateTime.UtcNow.Date;

        // Active promotions for this org
        var activePromos = await _db.Promotions
            .Where(p => p.OrganizationId == orgId && p.Status == PromotionStatus.Active
                && p.StartDate <= today && (p.EndDate == null || p.EndDate >= today))
            .ToListAsync();

        if (!activePromos.Any())
            return Ok(new { message = "No active promotions found." });

        // All products with their category chain
        var products = await _db.CatalogProducts
            .Where(p => p.OrganizationId == orgId && p.Status == DessertERP.Domain.Modules.ProductManagement.ProductStatus.Active)
            .Include(p => p.Category)
            .ThenInclude(c => c!.ParentCategory)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
            .ToListAsync();

        // All categories for lookup
        var allCats = await _db.Categories
            .Where(c => c.OrganizationId == orgId && c.IsActive)
            .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Code)
            .ToListAsync();

        // Build XML grouped by top-level category → subcategory → product
        var topLevelCats = allCats.Where(c => c.ParentCategoryId == null).ToList();

        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("PromoItemsCatalog",
                new XAttribute("exportedAt",      today.ToString("yyyy-MM-dd")),
                new XAttribute("totalPromotions", activePromos.Count),

                new XElement("ActivePromotions",
                    activePromos.Select(p => new XElement("Promotion",
                        new XElement("Name",        p.Name),
                        new XElement("Type",        p.DiscountType.ToString()),
                        new XElement("Value",       p.DiscountValue.ToString("F2")),
                        new XElement("StartDate",   p.StartDate.ToString("yyyy-MM-dd")),
                        new XElement("EndDate",     p.EndDate?.ToString("yyyy-MM-dd") ?? "ongoing"),
                        new XElement("MinOrderAmt", p.MinimumOrderAmount.ToString("F2") ?? "0.00")
                    ))
                ),

                new XElement("Categories",
                    topLevelCats.Select(top =>
                    {
                        var subCats = allCats.Where(c => c.ParentCategoryId == top.Id).ToList();

                        return new XElement("Category",
                            new XAttribute("code", top.Code),
                            new XElement("Name", top.Name),

                            subCats.Count > 0
                                ? subCats.Select(sub =>
                                {
                                    var subProds = products.Where(p => p.CategoryId == sub.Id).ToList();
                                    if (!subProds.Any()) return null!;
                                    return new XElement("SubCategory",
                                        new XAttribute("code", sub.Code),
                                        new XElement("Name", sub.Name),
                                        subProds.Select(p => BuildProductXml(p, activePromos))
                                    );
                                }).Where(x => x != null)
                                : products.Where(p => p.CategoryId == top.Id)
                                    .Select(p => BuildProductXml(p, activePromos))
                        );
                    })
                )
            )
        );

        using var ms = new System.IO.MemoryStream();
        using (var writer = System.Xml.XmlWriter.Create(ms, new System.Xml.XmlWriterSettings
        {
            Indent = true, IndentChars = "\t",
            Encoding = new System.Text.UTF8Encoding(false)
        }))
            doc.Save(writer);

        return File(ms.ToArray(), "application/xml",
            $"promo-items-{today:yyyyMMdd}.xml");
    }

    private static XElement BuildProductXml(
        DessertERP.Domain.Modules.ProductManagement.Product p,
        List<Promotion> promos)
    {
        // Pick the best applicable promo (highest discount value)
        var bestPromo = promos
            .Where(pr => pr.MinimumOrderAmount == null || pr.MinimumOrderAmount == 0)
            .OrderByDescending(pr => pr.DiscountValue)
            .FirstOrDefault();

        var originalPrice = p.BasePrice;
        var discountedPrice = bestPromo?.DiscountType == DiscountType.PercentageOff
            ? Math.Round(originalPrice * (1 - bestPromo.DiscountValue / 100), 2)
            : bestPromo?.DiscountType == DiscountType.FixedAmountOff
                ? Math.Max(0, originalPrice - (bestPromo.DiscountValue))
                : originalPrice;

        return new XElement("Product",
            new XElement("Sku",            p.Sku),
            new XElement("Name",           p.Name),
            new XElement("Description",    p.Description ?? ""),
            new XElement("Brand",          p.Brand?.Name ?? ""),
            new XElement("Gender",         p.GenderTarget.ToString()),
            new XElement("UnitOfMeasure",  p.UnitOfMeasure),
            new XElement("Currency",       p.Currency),
            new XElement("Pricing",
                new XElement("OriginalPrice",   originalPrice.ToString("F2")),
                new XElement("DiscountedPrice", discountedPrice.ToString("F2")),
                new XElement("Saving",         (originalPrice - discountedPrice).ToString("F2")),
                new XElement("PromoApplied",   bestPromo?.Name ?? "None"),
                new XElement("PromoType",      bestPromo?.DiscountType.ToString() ?? ""),
                new XElement("PromoValue",     bestPromo?.DiscountValue.ToString("F2") ?? "0.00")
            ),
            new XElement("Variants",
                p.Variants.Where(v => !v.IsDeleted).Select(v => new XElement("Variant",
                    new XElement("VariantSku",  v.Sku),
                    new XElement("Size",        v.Size ?? ""),
                    new XElement("Color",       v.Color ?? ""),
                    new XElement("Material",    v.Material ?? ""),
                    new XElement("Price",       v.EffectivePrice(p.BasePrice).ToString("F2")),
                    new XElement("PromoPrice",  (bestPromo?.DiscountType == DiscountType.PercentageOff
                        ? Math.Round(v.EffectivePrice(p.BasePrice) * (1 - bestPromo.DiscountValue / 100), 2)
                        : discountedPrice).ToString("F2"))
                ))
            )
        );
    }
}
