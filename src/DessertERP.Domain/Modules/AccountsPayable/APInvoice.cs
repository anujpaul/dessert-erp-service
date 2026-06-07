using DessertERP.Domain.Common;
using System.Text.Json;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum APInvoiceStatus    { Draft, Approved, Scheduled, Paid, Overdue, Voided }
public enum APInvoiceType      { Standard, Prepayment }
public enum ThreeWayMatchStatus
{
    /// <summary>Match not yet run (e.g. manual invoice not linked to PO).</summary>
    NotMatched,
    /// <summary>All checks passed within tolerance.</summary>
    Matched,
    /// <summary>Invoice amount exceeds what was received (billing for unreceived goods).</summary>
    QtyException,
    /// <summary>Invoice price differs from PO price beyond the allowed tolerance.</summary>
    PriceException,
    /// <summary>Both qty and price exceptions.</summary>
    FullException,
    /// <summary>Exception overridden by a manager with a bypass reason.</summary>
    Bypassed
}

public class APInvoice : BaseEntity
{
    // ── Core identity ─────────────────────────────────────────────────────────
    public Guid   OrganizationId    { get; private set; }
    public string InvoiceNumber     { get; private set; } = string.Empty;
    public Guid   VendorId          { get; private set; }
    public Guid?  PurchaseOrderId   { get; private set; }
    public APInvoiceType InvoiceType { get; private set; } = APInvoiceType.Standard;

    // ── Dates / reference ─────────────────────────────────────────────────────
    public DateTime InvoiceDate      { get; private set; }
    public DateTime DueDate          { get; private set; }
    public string   Description      { get; private set; } = string.Empty;
    public string   VendorInvoiceRef { get; private set; } = string.Empty;

    // ── Amounts ───────────────────────────────────────────────────────────────
    public decimal SubTotal          { get; private set; }
    public decimal TaxAmount         { get; private set; }
    public decimal TotalAmount       { get; private set; }
    public decimal PaidAmount        { get; private set; }
    public decimal PrepaymentApplied { get; private set; }

    /// <summary>Linked prepayment invoice whose amount has been offset against this final invoice.</summary>
    public Guid? LinkedPrepaymentInvoiceId { get; private set; }

    // ── Status / matching ─────────────────────────────────────────────────────
    public APInvoiceStatus     Status      { get; private set; } = APInvoiceStatus.Draft;
    public ThreeWayMatchStatus MatchStatus { get; private set; } = ThreeWayMatchStatus.NotMatched;

    /// <summary>JSON blob with match details (amounts, variance %, timestamp).</summary>
    public string? MatchNotes  { get; private set; }

    /// <summary>Reason given by the manager when bypassing a match exception.</summary>
    public string? BypassReason { get; private set; }

    public Guid? JournalEntryId { get; private set; }

    // ── Navigations ───────────────────────────────────────────────────────────
    public Vendor?        Vendor        { get; private set; }
    public PurchaseOrder? PurchaseOrder { get; private set; }

    // ── Computed ──────────────────────────────────────────────────────────────
    public decimal OutstandingAmount =>
        TotalAmount - PaidAmount - PrepaymentApplied;

    public int DaysOutstanding =>
        Status is APInvoiceStatus.Approved or APInvoiceStatus.Scheduled or APInvoiceStatus.Overdue
            ? Math.Max(0, (DateTime.UtcNow.Date - DueDate.Date).Days)
            : 0;

    private APInvoice() { }

    // ── Constructors ──────────────────────────────────────────────────────────

    public APInvoice(Guid organizationId, string invoiceNumber, Guid vendorId,
        DateTime invoiceDate, DateTime dueDate, string description, string vendorInvoiceRef,
        decimal subTotal, decimal taxAmount,
        Guid? purchaseOrderId = null,
        APInvoiceType invoiceType = APInvoiceType.Standard)
    {
        OrganizationId   = organizationId;
        InvoiceNumber    = invoiceNumber;
        VendorId         = vendorId;
        PurchaseOrderId  = purchaseOrderId;
        InvoiceType      = invoiceType;
        InvoiceDate      = invoiceDate;
        DueDate          = dueDate;
        Description      = description;
        VendorInvoiceRef = vendorInvoiceRef;
        SubTotal         = subTotal;
        TaxAmount        = taxAmount;
        TotalAmount      = subTotal + taxAmount;

        // Prepayment invoices skip 3WM (no goods received yet)
        MatchStatus = invoiceType == APInvoiceType.Prepayment
            ? ThreeWayMatchStatus.NotMatched    // will stay NotMatched — Approve() allows it
            : ThreeWayMatchStatus.NotMatched;
    }

    // ── Three-Way Match ───────────────────────────────────────────────────────

    /// <summary>
    /// Runs the three-way match (PO → Receipt → Invoice).
    /// <para>
    /// <paramref name="receivedValue"/>   = total GRN value (sum of all ReceivedQty × UnitCost).<br/>
    /// <paramref name="previouslyInvoiced"/> = amount already invoiced on this PO before this invoice.<br/>
    /// <paramref name="tolerancePct"/>    = allowed price variance % (default 2 %).
    /// </para>
    /// </summary>
    public ThreeWayMatchResult RunThreeWayMatch(
        decimal receivedValue,
        decimal previouslyInvoiced,
        decimal tolerancePct = 2m)
    {
        if (InvoiceType == APInvoiceType.Prepayment)
            throw new InvalidOperationException("Three-way match does not apply to prepayment invoices.");

        var uninvoicedReceived = receivedValue - previouslyInvoiced;
        var variancePct = uninvoicedReceived > 0
            ? Math.Abs(SubTotal - uninvoicedReceived) / uninvoicedReceived * 100m
            : (SubTotal > 0 ? 100m : 0m);

        bool qtyException   = SubTotal > receivedValue + 0.01m;   // billing beyond total received
        bool priceException = variancePct > tolerancePct;

        MatchStatus = (qtyException, priceException) switch
        {
            (true,  true)  => ThreeWayMatchStatus.FullException,
            (true,  false) => ThreeWayMatchStatus.QtyException,
            (false, true)  => ThreeWayMatchStatus.PriceException,
            _              => ThreeWayMatchStatus.Matched
        };

        var notes = new
        {
            runAt              = DateTime.UtcNow,
            poReceivedValue    = receivedValue,
            previouslyInvoiced,
            uninvoicedReceived,
            invoiceSubTotal    = SubTotal,
            variancePct        = Math.Round(variancePct, 2),
            tolerancePct,
            qtyException,
            priceException,
            result             = MatchStatus.ToString()
        };
        MatchNotes  = JsonSerializer.Serialize(notes);
        BypassReason = null;   // clear any old bypass if re-matched
        SetUpdated();

        return new ThreeWayMatchResult(
            MatchStatus, receivedValue, previouslyInvoiced,
            uninvoicedReceived, SubTotal, Math.Round(variancePct, 2),
            tolerancePct, qtyException, priceException);
    }

    /// <summary>
    /// Manager override: clears a match exception and allows approval.
    /// </summary>
    public void BypassMatch(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new InvalidOperationException("A bypass reason is required.");
        if (MatchStatus == ThreeWayMatchStatus.Matched)
            throw new InvalidOperationException("Invoice is already matched — no bypass needed.");

        BypassReason = reason;
        MatchStatus  = ThreeWayMatchStatus.Bypassed;
        SetUpdated();
    }

    // ── Prepayment ────────────────────────────────────────────────────────────

    /// <summary>
    /// Links a prepayment invoice and deducts its amount from this invoice's outstanding balance.
    /// </summary>
    public void ApplyPrepayment(APInvoice prepaymentInvoice)
    {
        if (InvoiceType == APInvoiceType.Prepayment)
            throw new InvalidOperationException("Cannot apply a prepayment to another prepayment invoice.");
        if (prepaymentInvoice.InvoiceType != APInvoiceType.Prepayment)
            throw new InvalidOperationException("The linked invoice is not a prepayment invoice.");
        if (prepaymentInvoice.Status == APInvoiceStatus.Voided)
            throw new InvalidOperationException("Cannot apply a voided prepayment.");
        if (prepaymentInvoice.VendorId != VendorId)
            throw new InvalidOperationException("Prepayment must be from the same vendor.");

        var applyAmt = Math.Min(prepaymentInvoice.TotalAmount, OutstandingAmount);
        PrepaymentApplied          = applyAmt;
        LinkedPrepaymentInvoiceId  = prepaymentInvoice.Id;
        SetUpdated();
    }

    // ── Workflow ──────────────────────────────────────────────────────────────

    public void Approve(Guid? journalEntryId = null)
    {
        if (Status != APInvoiceStatus.Draft)
            throw new InvalidOperationException("Only a Draft invoice can be approved.");

        // Prepayment invoices skip 3WM — they're approved before goods arrive.
        if (InvoiceType != APInvoiceType.Prepayment && PurchaseOrderId.HasValue)
        {
            if (MatchStatus == ThreeWayMatchStatus.NotMatched)
                throw new InvalidOperationException(
                    "Three-way match has not been run. Run the match before approving.");
            if (MatchStatus is ThreeWayMatchStatus.QtyException
                           or ThreeWayMatchStatus.PriceException
                           or ThreeWayMatchStatus.FullException)
                throw new InvalidOperationException(
                    "Three-way match has exceptions. A manager must bypass the match before approving.");
        }

        Status         = APInvoiceStatus.Approved;
        JournalEntryId = journalEntryId;
        SetUpdated();
    }

    public void SchedulePayment()
    {
        if (Status != APInvoiceStatus.Approved)
            throw new InvalidOperationException("Invoice must be approved before scheduling payment.");
        Status = APInvoiceStatus.Scheduled;
        SetUpdated();
    }

    public void ApplyPayment(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Payment amount must be positive.");
        if (amount > OutstandingAmount)
            throw new InvalidOperationException("Payment exceeds outstanding balance.");

        PaidAmount += amount;
        if (PaidAmount + PrepaymentApplied >= TotalAmount)
            Status = APInvoiceStatus.Paid;
        SetUpdated();
    }

    public void MarkOverdue()
    {
        if (Status is APInvoiceStatus.Approved or APInvoiceStatus.Scheduled)
        {
            Status = APInvoiceStatus.Overdue;
            SetUpdated();
        }
    }

    public void Void()
    {
        if (Status == APInvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot void a fully paid invoice.");
        Status = APInvoiceStatus.Voided;
        SetUpdated();
    }
}

/// <summary>Returned from RunThreeWayMatch — not persisted, used by service layer.</summary>
public record ThreeWayMatchResult(
    ThreeWayMatchStatus Status,
    decimal ReceivedValue,
    decimal PreviouslyInvoiced,
    decimal UninvoicedReceived,
    decimal InvoiceSubTotal,
    decimal VariancePct,
    decimal TolerancePct,
    bool QtyException,
    bool PriceException);
