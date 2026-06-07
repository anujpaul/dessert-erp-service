using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

/// <summary>
/// Every stock movement is recorded as an immutable transaction entry.
/// Positive Quantity = stock in; Negative Quantity = stock out.
/// </summary>
public enum InventoryTransactionType
{
    OpeningBalance,       // initial stock setup
    PurchaseOrderPlaced,  // PO sent to vendor → +OnOrder
    PurchaseOrderClosed,  // PO cancelled/closed → -OnOrder (no receipt)
    PurchaseReceipt,      // GRN received → +OnHand, -OnOrder
    PurchaseReturn,       // returned to vendor → -OnHand
    SaleCommit,           // SO confirmed → +Reserved
    SaleUncommit,         // SO cancelled before shipment → -Reserved
    SaleShipment,         // goods shipped → -OnHand, -Reserved
    SaleReturn,           // customer return → +OnHand, -Reserved if applicable
    AdjustmentIn,         // cycle count / found stock → +OnHand
    AdjustmentOut,        // write-off / damage → -OnHand
    TransferOut,          // moved to another location → -OnHand
    TransferIn,           // arrived from another location → +OnHand
}

public class InventoryTransaction : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid ProductVariantId { get; private set; }

    public InventoryTransactionType TransactionType { get; private set; }

    /// <summary>Positive = stock in, Negative = stock out.</summary>
    public decimal Quantity { get; private set; }

    /// <summary>Unit cost at time of transaction (for valuation history).</summary>
    public decimal UnitCost { get; private set; }

    /// <summary>Running on-hand balance after this transaction.</summary>
    public decimal BalanceAfter { get; private set; }

    /// <summary>Reference document: PO number, SO number, adjustment ID, etc.</summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>Optional FK to a PO or SO for drill-through.</summary>
    public Guid? ReferenceDocumentId { get; private set; }

    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public ProductVariant? ProductVariant { get; private set; }

    private InventoryTransaction() { }

    public InventoryTransaction(
        Guid organizationId,
        Guid productVariantId,
        InventoryTransactionType transactionType,
        decimal quantity,
        decimal unitCost,
        decimal balanceAfter,
        string? referenceNumber = null,
        Guid? referenceDocumentId = null,
        string? notes = null,
        string? createdBy = null)
    {
        OrganizationId      = organizationId;
        ProductVariantId    = productVariantId;
        TransactionType     = transactionType;
        Quantity            = quantity;
        UnitCost            = unitCost;
        BalanceAfter        = balanceAfter;
        ReferenceNumber     = referenceNumber;
        ReferenceDocumentId = referenceDocumentId;
        Notes               = notes;
        CreatedBy           = createdBy;
        TransactionDate     = DateTime.UtcNow;
    }
}
