using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public enum ProductStatus   { Active, Inactive, Discontinued }
public enum ProductType     { Clothing, Footwear, Accessory, Food, PersonalCare, Other }
public enum GenderTarget    { Men, Women, Unisex, Kids, None }

public class Product : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Sku { get; private set; } = string.Empty;          // Base SKU (e.g. DTC-PANT-001)
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? LongDescription { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid? BrandId { get; private set; }
    public ProductType ProductType { get; private set; } = ProductType.Other;
    public GenderTarget GenderTarget { get; private set; } = GenderTarget.Unisex;
    public string UnitOfMeasure { get; private set; } = "Each";
    public decimal BasePrice { get; private set; }
    public decimal BaseCost { get; private set; }
    public decimal TaxRate { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string? Tags { get; private set; }           // comma-separated
    public string? ImageUrl { get; private set; }       // primary image
    public ProductStatus Status { get; private set; } = ProductStatus.Active;
    public Guid? PreferredVendorId { get; private set; } // FK → Vendor (AP module)

    public Category? Category { get; private set; }
    public Brand? Brand { get; private set; }

    private readonly List<ProductVariant> _variants = new();
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    private Product() { }

    public Product(Guid organizationId, string sku, string name, Guid categoryId,
        ProductType productType, decimal basePrice, decimal baseCost, decimal taxRate,
        string unitOfMeasure = "Each", Guid? brandId = null,
        GenderTarget genderTarget = GenderTarget.Unisex,
        string? description = null, string? tags = null, string currency = "USD")
    {
        OrganizationId = organizationId;
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        CategoryId = categoryId;
        ProductType = productType;
        BasePrice = basePrice;
        BaseCost = baseCost;
        TaxRate = taxRate;
        UnitOfMeasure = unitOfMeasure;
        BrandId = brandId;
        GenderTarget = genderTarget;
        Description = description;
        Tags = tags;
        Currency = currency;
    }

    public void Update(string name, string? description, string? longDescription,
        Guid categoryId, Guid? brandId, decimal basePrice, decimal baseCost,
        decimal taxRate, ProductType productType, GenderTarget genderTarget,
        string? tags, string? imageUrl)
    {
        Name = name.Trim();
        Description = description;
        LongDescription = longDescription;
        CategoryId = categoryId;
        BrandId = brandId;
        BasePrice = basePrice;
        BaseCost = baseCost;
        TaxRate = taxRate;
        ProductType = productType;
        GenderTarget = genderTarget;
        Tags = tags;
        ImageUrl = imageUrl;
        SetUpdated();
    }

    public ProductVariant AddVariant(string size, string? color, string? material,
        string? barcode = null, decimal? priceOverride = null, decimal? costOverride = null)
    {
        // Build variant SKU: base-SKU + size + color abbreviation
        var suffix = new[] { size, color?.Substring(0, Math.Min(3, color?.Length ?? 0)) }
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s!.ToUpperInvariant().Replace(" ", ""));
        var variantSku = $"{Sku}-{string.Join("-", suffix)}";

        var variant = new ProductVariant(OrganizationId, Id, variantSku,
            size, color, material, barcode, priceOverride, costOverride);
        _variants.Add(variant);
        SetUpdated();
        return variant;
    }

    public void SetPreferredVendor(Guid? vendorId) { PreferredVendorId = vendorId; SetUpdated(); }

    public void Discontinue() { Status = ProductStatus.Discontinued; SetUpdated(); }
    public void Activate()    { Status = ProductStatus.Active;        SetUpdated(); }
    public void Deactivate()  { Status = ProductStatus.Inactive;      SetUpdated(); }
}
