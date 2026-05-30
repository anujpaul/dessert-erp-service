using DessertERP.Domain.Common;
using DessertERP.Domain.Modules.ProductManagement;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public class SalesOrderLine : BaseEntity
{
    public Guid SalesOrderId { get; private set; }
    public Guid ProductVariantId { get; private set; }    // references ProductManagement.ProductVariant
    public string Sku { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public string? VariantDescription { get; private set; }  // e.g. "Blue / XL / Cotton"
    public string UnitOfMeasure { get; private set; } = "Each";
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountPct { get; private set; }
    public decimal TaxRate { get; private set; }

    // Computed — not mapped to DB columns
    public decimal LineSubTotal   => Math.Round(Quantity * UnitPrice, 4);
    public decimal DiscountAmount => Math.Round(LineSubTotal * DiscountPct / 100, 4);
    public decimal TaxableAmount  => LineSubTotal - DiscountAmount;
    public decimal TaxAmount      => Math.Round(TaxableAmount * TaxRate / 100, 4);
    public decimal LineTotal      => TaxableAmount + TaxAmount;

    public SalesOrder? SalesOrder { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }

    private SalesOrderLine() { }

    public SalesOrderLine(Guid salesOrderId, Guid productVariantId, string sku,
        string productName, string? variantDescription, string unitOfMeasure,
        decimal quantity, decimal unitPrice, decimal taxRate, decimal discountPct = 0)
    {
        SalesOrderId = salesOrderId;
        ProductVariantId = productVariantId;
        Sku = sku;
        ProductName = productName;
        VariantDescription = variantDescription;
        UnitOfMeasure = unitOfMeasure;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TaxRate = taxRate;
        DiscountPct = discountPct;
    }

    public void Update(decimal quantity, decimal unitPrice, decimal discountPct)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPct = discountPct;
        SetUpdated();
    }
}
