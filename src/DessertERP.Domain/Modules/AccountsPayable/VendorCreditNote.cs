using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum CreditNoteStatus  { Draft, Posted, Applied, Voided }
public enum CreditNoteReason  { Return, PriceCorrection, Dispute, Overpayment, Other }

/// <summary>
/// Vendor Credit Note — a reduction to amounts owed to a vendor.
/// Created for returns, price corrections, or dispute resolutions.
/// Can be applied to an existing AP invoice or held as a vendor credit balance.
/// </summary>
public class VendorCreditNote : BaseEntity
{
    public Guid   OrganizationId   { get; private set; }
    public string CreditNoteNumber { get; private set; } = string.Empty;
    public Guid   VendorId         { get; private set; }
    public Guid?  APInvoiceId      { get; private set; }   // optional — the invoice being corrected
    public Guid?  PurchaseOrderId  { get; private set; }   // optional — the PO related to the return

    public DateTime  CreditDate    { get; private set; }
    public string    Description   { get; private set; } = string.Empty;
    public string?   VendorCNRef   { get; private set; }   // vendor's own credit note number

    public decimal   SubTotal      { get; private set; }
    public decimal   TaxAmount     { get; private set; }
    public decimal   TotalAmount   { get; private set; }
    public decimal   AppliedAmount { get; private set; }   // amount consumed against invoices
    public decimal   AvailableCredit => TotalAmount - AppliedAmount;

    public CreditNoteStatus Status { get; private set; } = CreditNoteStatus.Draft;
    public CreditNoteReason Reason { get; private set; } = CreditNoteReason.Other;
    public string?   Notes         { get; private set; }

    public Vendor?    Vendor    { get; private set; }
    public APInvoice? APInvoice { get; private set; }

    private VendorCreditNote() { }

    public VendorCreditNote(Guid organizationId, string creditNoteNumber,
        Guid vendorId, DateTime creditDate, string description,
        decimal subTotal, decimal taxAmount,
        CreditNoteReason reason = CreditNoteReason.Other,
        Guid? apInvoiceId = null, Guid? purchaseOrderId = null,
        string? vendorCnRef = null, string? notes = null)
    {
        OrganizationId   = organizationId;
        CreditNoteNumber = creditNoteNumber.Trim();
        VendorId         = vendorId;
        CreditDate       = creditDate;
        Description      = description.Trim();
        SubTotal         = subTotal;
        TaxAmount        = taxAmount;
        TotalAmount      = subTotal + taxAmount;
        Reason           = reason;
        APInvoiceId      = apInvoiceId;
        PurchaseOrderId  = purchaseOrderId;
        VendorCNRef      = vendorCnRef;
        Notes            = notes;
    }

    /// <summary>Post the credit note — it becomes available to apply against invoices.</summary>
    public void Post()
    {
        if (Status != CreditNoteStatus.Draft)
            throw new InvalidOperationException("Only Draft credit notes can be posted.");
        Status = CreditNoteStatus.Posted;
        SetUpdated();
    }

    /// <summary>Apply some or all of the available credit against an invoice payment.</summary>
    public void ApplyCredit(decimal amount)
    {
        if (Status == CreditNoteStatus.Voided)
            throw new InvalidOperationException("Cannot apply a voided credit note.");
        if (Status == CreditNoteStatus.Draft)
            throw new InvalidOperationException("Credit note must be posted before applying.");
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        if (amount > AvailableCredit + 0.01m)
            throw new InvalidOperationException($"Cannot apply {amount:C} — only {AvailableCredit:C} available.");

        AppliedAmount += amount;
        if (AvailableCredit <= 0.01m)
            Status = CreditNoteStatus.Applied;
        SetUpdated();
    }

    public void Void()
    {
        if (Status == CreditNoteStatus.Applied)
            throw new InvalidOperationException("Cannot void a fully-applied credit note.");
        if (Status == CreditNoteStatus.Voided)
            throw new InvalidOperationException("Already voided.");
        Status = CreditNoteStatus.Voided;
        SetUpdated();
    }

    public void UpdateNotes(string? notes) { Notes = notes; SetUpdated(); }
}
