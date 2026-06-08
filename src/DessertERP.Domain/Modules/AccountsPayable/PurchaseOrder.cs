using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

/// <summary>Lifecycle status — tracks physical receipt progress.</summary>
public enum PurchaseOrderStatus { Draft, PendingApproval, Sent, PartiallyReceived, FullyReceived, Closed, Cancelled }

/// <summary>Invoice status — independent of receive status (a PO can be partially invoiced and still receive more goods).</summary>
public enum POInvoiceStatus { NotInvoiced, PartiallyInvoiced, FullyInvoiced }

public class PurchaseOrder : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string PONumber { get; private set; } = string.Empty;
    public Guid VendorId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Currency { get; private set; } = "USD";
    public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
    public POInvoiceStatus InvoiceStatus { get; private set; } = POInvoiceStatus.NotInvoiced;
    public decimal SubTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal GrandTotal { get; private set; }

    /// <summary>Amount invoiced to date (cumulative across all AP invoices linked to this PO).</summary>
    public decimal InvoicedAmount { get; private set; }

    // Workflow linkage
    public Guid?  WorkflowInstanceId { get; private set; }
    public string? RejectionReason   { get; private set; }

    // Export tracking
    public bool      IsExported { get; private set; }
    public DateTime? ExportedAt { get; private set; }

    public Vendor? Vendor { get; private set; }

    private readonly List<PurchaseOrderLine> _lines = new();
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private readonly List<PurchaseOrderReceipt> _receipts = new();
    public IReadOnlyCollection<PurchaseOrderReceipt> Receipts => _receipts.AsReadOnly();

    private PurchaseOrder() { }

    public PurchaseOrder(Guid organizationId, string poNumber, Guid vendorId, DateTime orderDate,
        string description, string currency = "USD", DateTime? expectedDate = null)
    {
        OrganizationId = organizationId;
        PONumber = poNumber;
        VendorId = vendorId;
        OrderDate = orderDate;
        Description = description;
        Currency = currency;
        ExpectedDate = expectedDate;
    }

    // ── Receiving ─────────────────────────────────────────────────────────────

    /// <summary>True when more goods can still be received (quantities not yet fully received and PO not closed/cancelled).</summary>
    public bool CanReceive =>
        Status != PurchaseOrderStatus.Draft &&
        Status != PurchaseOrderStatus.Closed &&
        Status != PurchaseOrderStatus.Cancelled &&
        _lines.Any(l => !l.IsFullyReceived);

    /// <summary>Recalculates receive status from line quantities. Safe to call at any invoice status.</summary>
    public void UpdateReceiptStatus()
    {
        if (Status == PurchaseOrderStatus.Draft || Status == PurchaseOrderStatus.Closed || Status == PurchaseOrderStatus.Cancelled)
            return;

        bool anyReceived = _lines.Any(l => l.ReceivedQty > 0);
        bool allReceived = _lines.All(l => l.IsFullyReceived);

        Status = allReceived ? PurchaseOrderStatus.FullyReceived
               : anyReceived ? PurchaseOrderStatus.PartiallyReceived
               : PurchaseOrderStatus.Sent;

        SetUpdated();
    }

    // ── Invoicing ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Records that an invoice has been raised against this PO for <paramref name="invoicedAmount"/>.
    /// Does NOT change the receive status — a PO can still accept more goods after being partially invoiced.
    /// </summary>
    public void RecordInvoice(decimal invoicedAmount)
    {
        if (Status == PurchaseOrderStatus.Draft || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot invoice a Draft or Cancelled PO.");
        if (Status == PurchaseOrderStatus.Sent)
            throw new InvalidOperationException("Goods must be received before invoicing.");

        var receivedValue = _lines.Sum(l => Math.Round(l.ReceivedQty * l.UnitCost * (1 + l.TaxRate / 100), 4));
        if (InvoicedAmount + invoicedAmount > receivedValue + 0.01m) // 1-cent tolerance
            throw new InvalidOperationException("Cannot invoice more than the total received value.");

        InvoicedAmount += invoicedAmount;

        // Update invoice status
        InvoiceStatus = InvoicedAmount >= receivedValue - 0.01m
            ? POInvoiceStatus.FullyInvoiced
            : POInvoiceStatus.PartiallyInvoiced;

        SetUpdated();
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public PurchaseOrderLine AddLine(Guid productVariantId, string productCode, string description,
        string uom, decimal qty, decimal unitCost, decimal taxRate = 0)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft PO.");
        var line = new PurchaseOrderLine(Id, productVariantId, productCode, description, uom, qty, unitCost, taxRate);
        _lines.Add(line);
        RecalcTotals();
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft PO.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        RecalcTotals();
        SetUpdated();
    }

    /// <summary>
    /// Submit the PO for workflow approval.
    /// The service layer creates a WorkflowInstance and then calls this.
    /// </summary>
    public void SubmitForApproval(Guid workflowInstanceId)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only a Draft PO can be submitted for approval.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot submit a PO with no lines.");
        Status             = PurchaseOrderStatus.PendingApproval;
        WorkflowInstanceId = workflowInstanceId;
        SetUpdated();
    }

    /// <summary>Called by the workflow engine after all steps are approved — activates the PO.</summary>
    public void WorkflowApproved()
    {
        if (Status != PurchaseOrderStatus.PendingApproval)
            throw new InvalidOperationException("PO is not pending approval.");
        Status = PurchaseOrderStatus.Sent;
        SetUpdated();
    }

    /// <summary>Called by the workflow engine when a step is rejected — returns PO to Draft.</summary>
    public void WorkflowRejected(string reason)
    {
        if (Status != PurchaseOrderStatus.PendingApproval)
            throw new InvalidOperationException("PO is not pending approval.");
        Status          = PurchaseOrderStatus.Draft;   // allow correction & resubmission
        RejectionReason = reason;
        SetUpdated();
    }

    public void Send()
    {
        // Direct-send path: for orgs without PO approval workflow configured
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only a Draft PO can be sent.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot send a PO with no lines.");
        Status = PurchaseOrderStatus.Sent;
        SetUpdated();
    }

    public void Close()
    {
        if (Status == PurchaseOrderStatus.Draft || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot close a Draft or Cancelled PO.");
        if (InvoiceStatus != POInvoiceStatus.FullyInvoiced)
            throw new InvalidOperationException("PO must be fully invoiced before closing.");
        Status = PurchaseOrderStatus.Closed;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.FullyReceived || Status == PurchaseOrderStatus.Closed)
            throw new InvalidOperationException("Cannot cancel a PO that has been fully received or closed.");
        if (InvoiceStatus != POInvoiceStatus.NotInvoiced)
            throw new InvalidOperationException("Cannot cancel a PO that has already been invoiced.");
        Status = PurchaseOrderStatus.Cancelled;
        SetUpdated();
    }

    public void MarkExported()  { IsExported = true; ExportedAt = DateTime.UtcNow; SetUpdated(); }
    public void ResetExport()   { IsExported = false; ExportedAt = null; SetUpdated(); }

    // ── Service-layer helpers ─────────────────────────────────────────────────

    public void ValidateCanRemoveLine()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft PO.");
    }

    public void RecalcTotalsFromLines(IEnumerable<PurchaseOrderLine> lines)
    {
        var list = lines.ToList();
        SubTotal   = list.Sum(l => Math.Round(l.OrderedQty * l.UnitCost, 4));
        TaxTotal   = list.Sum(l => Math.Round(l.OrderedQty * l.UnitCost * l.TaxRate / 100, 4));
        GrandTotal = SubTotal + TaxTotal;
        SetUpdated();
    }

    private void RecalcTotals()
    {
        SubTotal   = _lines.Sum(l => Math.Round(l.OrderedQty * l.UnitCost, 4));
        TaxTotal   = _lines.Sum(l => Math.Round(l.OrderedQty * l.UnitCost * l.TaxRate / 100, 4));
        GrandTotal = SubTotal + TaxTotal;
    }
}
