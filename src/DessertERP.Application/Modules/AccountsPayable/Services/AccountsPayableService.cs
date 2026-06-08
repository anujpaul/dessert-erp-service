using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.AccountsPayable.DTOs;
using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.AccountsPayable.Services;

public interface IAccountsPayableService
{
    // Vendors
    Task<IEnumerable<VendorDto>> GetVendorsAsync(CancellationToken ct = default);
    Task<VendorDto> CreateVendorAsync(CreateVendorRequest req, CancellationToken ct = default);
    Task<VendorDto> UpdateVendorAsync(Guid id, UpdateVendorRequest req, CancellationToken ct = default);
    Task DeleteVendorAsync(Guid id, CancellationToken ct = default);

    // Purchase Orders
    Task<IEnumerable<PurchaseOrderSummaryDto>> GetPurchaseOrdersAsync(string? status = null, Guid? vendorId = null, CancellationToken ct = default);
    Task<PurchaseOrderDto?> GetPurchaseOrderAsync(Guid id, CancellationToken ct = default);
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest req, CancellationToken ct = default);
    Task<PurchaseOrderDto> AddPOLineAsync(Guid poId, AddPOLineRequest req, CancellationToken ct = default);
    Task RemovePOLineAsync(Guid poId, Guid lineId, CancellationToken ct = default);
    Task SendPurchaseOrderAsync(Guid id, CancellationToken ct = default);
    Task<ReceiptDto> RecordReceiptAsync(Guid poId, RecordReceiptRequest req, CancellationToken ct = default);
    Task<IEnumerable<ReceiptDto>> GetReceiptsAsync(Guid poId, CancellationToken ct = default);
    Task ClosePurchaseOrderAsync(Guid id, CancellationToken ct = default);
    Task CancelPurchaseOrderAsync(Guid id, CancellationToken ct = default);

    // AP Invoices
    Task<IEnumerable<APInvoiceDto>> GetInvoicesAsync(Guid? vendorId = null, CancellationToken ct = default);
    Task<APInvoiceDto> CreateInvoiceAsync(CreateAPInvoiceRequest req, CancellationToken ct = default);
    Task<APInvoiceDto> GenerateInvoiceFromPOAsync(Guid poId, string vendorInvoiceRef, CancellationToken ct = default);
    Task<APInvoiceDto> CreatePrepaymentInvoiceAsync(CreatePrepaymentInvoiceRequest req, CancellationToken ct = default);
    Task<ThreeWayMatchDto> RunThreeWayMatchAsync(Guid invoiceId, CancellationToken ct = default);
    Task<APInvoiceDto> BypassMatchAsync(Guid invoiceId, string reason, CancellationToken ct = default);
    Task<APInvoiceDto> ApplyPrepaymentAsync(Guid invoiceId, Guid prepaymentInvoiceId, CancellationToken ct = default);
    Task ApproveInvoiceAsync(Guid id, CancellationToken ct = default);
    Task VoidInvoiceAsync(Guid id, CancellationToken ct = default);

    // AP Payments
    Task<IEnumerable<APPaymentDto>> GetPaymentsAsync(Guid? vendorId = null, CancellationToken ct = default);
    Task<APPaymentDto> CreatePaymentAsync(CreateAPPaymentRequest req, CancellationToken ct = default);

    // Vendor Addresses
    Task<IEnumerable<VendorAddressDto>> GetVendorAddressesAsync(Guid vendorId, CancellationToken ct = default);
    Task<VendorAddressDto> SaveVendorAddressAsync(Guid vendorId, Guid? addressId, SaveVendorAddressRequest req, CancellationToken ct = default);
    Task DeleteVendorAddressAsync(Guid vendorId, Guid addressId, CancellationToken ct = default);
    Task SetPrimaryVendorAddressAsync(Guid vendorId, Guid addressId, CancellationToken ct = default);

    // Vendor Contacts
    Task<IEnumerable<VendorContactDto>> GetVendorContactsAsync(Guid vendorId, CancellationToken ct = default);
    Task<VendorContactDto> SaveVendorContactAsync(Guid vendorId, Guid? contactId, SaveVendorContactRequest req, CancellationToken ct = default);
    Task DeleteVendorContactAsync(Guid vendorId, Guid contactId, CancellationToken ct = default);
    Task SetPrimaryVendorContactAsync(Guid vendorId, Guid contactId, CancellationToken ct = default);

    // Reports
    Task<IEnumerable<APAgingDto>> GetAgingReportAsync(CancellationToken ct = default);
    Task<VendorLedgerDto> GetVendorLedgerAsync(Guid vendorId, CancellationToken ct = default);

    // Purchase Requisitions
    Task<IEnumerable<PRSummaryDto>> GetRequisitionsAsync(string? status = null, CancellationToken ct = default);
    Task<PRDto?> GetRequisitionAsync(Guid id, CancellationToken ct = default);
    Task<PRDto> CreateRequisitionAsync(CreatePRRequest req, CancellationToken ct = default);
    Task<PRDto> AddPRLineAsync(Guid prId, AddPRLineRequest req, CancellationToken ct = default);
    Task RemovePRLineAsync(Guid prId, Guid lineId, CancellationToken ct = default);
    Task<PRDto> SubmitRequisitionAsync(Guid id, CancellationToken ct = default);
    Task<PRDto> ApproveRequisitionAsync(Guid id, string approvedBy, CancellationToken ct = default);
    Task<PRDto> RejectRequisitionAsync(Guid id, string rejectedBy, string reason, CancellationToken ct = default);
    Task<PurchaseOrderDto> ConvertRequisitionToPOAsync(Guid prId, ConvertPRToPORequest req, CancellationToken ct = default);
    Task CancelRequisitionAsync(Guid id, CancellationToken ct = default);

    // PO Workflow
    Task SubmitPOForApprovalAsync(Guid poId, string submittedBy, CancellationToken ct = default);
    Task POWorkflowApprovedAsync(Guid workflowInstanceId, CancellationToken ct = default);
    Task POWorkflowRejectedAsync(Guid workflowInstanceId, string reason, CancellationToken ct = default);

    // Invoice Workflow
    Task SubmitInvoiceForApprovalAsync(Guid invoiceId, string submittedBy, CancellationToken ct = default);
    Task InvoiceWorkflowApprovedAsync(Guid workflowInstanceId, CancellationToken ct = default);
    Task InvoiceWorkflowRejectedAsync(Guid workflowInstanceId, CancellationToken ct = default);

    // Payment Proposals
    Task<IEnumerable<PaymentProposalSummaryDto>> GetPaymentProposalsAsync(CancellationToken ct = default);
    Task<PaymentProposalDto?> GetPaymentProposalAsync(Guid id, CancellationToken ct = default);
    Task<PaymentProposalDto> CreatePaymentProposalAsync(CreatePaymentProposalRequest req, CancellationToken ct = default);
    Task<PaymentProposalDto> AddProposalLineAsync(Guid proposalId, Guid invoiceId, CancellationToken ct = default);
    Task RemoveProposalLineAsync(Guid proposalId, Guid lineId, CancellationToken ct = default);
    Task<PaymentProposalDto> ApprovePaymentProposalAsync(Guid id, CancellationToken ct = default);
    Task<PaymentProposalDto> ProcessPaymentProposalAsync(Guid id, string processedBy, CancellationToken ct = default);
    Task CancelPaymentProposalAsync(Guid id, CancellationToken ct = default);

    // Vendor Credit Notes
    Task<IEnumerable<CreditNoteDto>> GetCreditNotesAsync(Guid? vendorId = null, CancellationToken ct = default);
    Task<CreditNoteDto> CreateCreditNoteAsync(CreateCreditNoteRequest req, CancellationToken ct = default);
    Task<CreditNoteDto> PostCreditNoteAsync(Guid id, CancellationToken ct = default);
    Task<CreditNoteDto> ApplyCreditNoteAsync(Guid id, Guid invoiceId, decimal amount, CancellationToken ct = default);
    Task VoidCreditNoteAsync(Guid id, CancellationToken ct = default);
}

public class AccountsPayableService : IAccountsPayableService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public AccountsPayableService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db = db;
        _org = org;
    }

    // Vendors

    public async Task<IEnumerable<VendorDto>> GetVendorsAsync(CancellationToken ct = default)
    {
        var vendors = await _db.Vendors.Where(v => !v.IsDeleted).OrderBy(v => v.VendorNumber).ToListAsync(ct);
        var vendorIds = vendors.Select(v => v.Id).ToList();
        var balances = await _db.APInvoices
            .Where(i => vendorIds.Contains(i.VendorId) && !i.IsDeleted &&
                        i.Status != APInvoiceStatus.Paid && i.Status != APInvoiceStatus.Voided)
            .GroupBy(i => i.VendorId)
            .Select(g => new { VendorId = g.Key, Outstanding = g.Sum(i => i.TotalAmount - i.PaidAmount - i.PrepaymentApplied) })
            .ToDictionaryAsync(x => x.VendorId, x => x.Outstanding, ct);
        return vendors.Select(v => ToVendorDto(v, balances.GetValueOrDefault(v.Id, 0)));
    }

    public async Task<VendorDto> CreateVendorAsync(CreateVendorRequest req, CancellationToken ct = default)
    {
        var count = await _db.Vendors.CountAsync(ct) + 1;
        var vendor = new Vendor(_org.OrganizationId, $"VEND-{count:D5}", req.Name, req.Email, req.Phone,
            req.BillingAddress, req.Currency, req.PaymentTermsDays, req.TaxId,
            req.BillingAddress, req.ShippingAddress, req.Website, req.Notes);
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync(ct);
        return ToVendorDto(vendor, 0);
    }

    public async Task<VendorDto> UpdateVendorAsync(Guid id, UpdateVendorRequest req, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");
        vendor.Update(req.Name, req.Email, req.Phone, req.BillingAddress, req.ShippingAddress,
            req.PaymentTermsDays, req.BankAccountName, req.BankAccountNumber,
            req.BankRoutingNumber, req.Website, req.Notes);
        await _db.SaveChangesAsync(ct);
        var outstanding = await _db.APInvoices
            .Where(i => i.VendorId == id && !i.IsDeleted &&
                        i.Status != APInvoiceStatus.Paid && i.Status != APInvoiceStatus.Voided)
            .SumAsync(i => i.TotalAmount - i.PaidAmount - i.PrepaymentApplied, ct);
        return ToVendorDto(vendor, outstanding);
    }

    public async Task DeleteVendorAsync(Guid id, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");
        var hasOpenPO = await _db.PurchaseOrders
            .AnyAsync(p => p.VendorId == id && !p.IsDeleted &&
                p.Status != PurchaseOrderStatus.Closed && p.Status != PurchaseOrderStatus.Cancelled, ct);
        if (hasOpenPO)
            throw new InvalidOperationException("Cannot delete a vendor with open purchase orders.");
        vendor.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // Purchase Orders

    public async Task<IEnumerable<PurchaseOrderSummaryDto>> GetPurchaseOrdersAsync(
        string? status = null, Guid? vendorId = null, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Lines)
            .Where(o => !o.IsDeleted);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<PurchaseOrderStatus>(status, out var s))
            query = query.Where(o => o.Status == s);
        if (vendorId.HasValue)
            query = query.Where(o => o.VendorId == vendorId.Value);

        var list = await query.OrderByDescending(o => o.OrderDate).ToListAsync(ct);
        return list.Select(o => new PurchaseOrderSummaryDto(
            o.Id, o.PONumber, o.VendorId, o.Vendor?.Name ?? string.Empty,
            o.OrderDate, o.ExpectedDate, o.Status.ToString(), o.InvoiceStatus.ToString(),
            o.GrandTotal, o.InvoicedAmount, o.Lines.Count, o.CreatedAt));
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderAsync(Guid id, CancellationToken ct = default)
    {
        var o = await _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct);
        return o is null ? null : ToPODto(o);
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest req, CancellationToken ct = default)
    {
        var count = await _db.PurchaseOrders.CountAsync(ct) + 1;
        var po = new PurchaseOrder(_org.OrganizationId, $"PO-{req.OrderDate:yyyy}-{count:D5}",
            req.VendorId, req.OrderDate, req.Description, req.Currency, req.ExpectedDate);
        _db.PurchaseOrders.Add(po);
        await _db.SaveChangesAsync(ct);
        return (await GetPurchaseOrderAsync(po.Id, ct))!;
    }

    public async Task<PurchaseOrderDto> AddPOLineAsync(Guid poId, AddPOLineRequest req, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders
            .FirstOrDefaultAsync(o => o.Id == poId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        var variantExists = await _db.ProductVariants
            .IgnoreQueryFilters()
            .AnyAsync(v => v.Id == req.ProductVariantId && !v.IsDeleted, ct);
        if (!variantExists)
            throw new InvalidOperationException("Product variant not found.");

        var line = po.AddLine(req.ProductVariantId, req.ProductCode, req.Description,
            req.UnitOfMeasure, req.Quantity, req.UnitCost, req.TaxRate);
        _db.PurchaseOrderLines.Add(line);
        await _db.SaveChangesAsync(ct);
        return (await GetPurchaseOrderAsync(poId, ct))!;
    }

    public async Task RemovePOLineAsync(Guid poId, Guid lineId, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders
            .FirstOrDefaultAsync(o => o.Id == poId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        var line = await _db.PurchaseOrderLines
            .FirstOrDefaultAsync(l => l.Id == lineId && l.PurchaseOrderId == poId && !l.IsDeleted, ct)
            ?? throw new InvalidOperationException("Line not found.");

        po.ValidateCanRemoveLine();
        line.SoftDelete();

        var remaining = await _db.PurchaseOrderLines
            .Where(l => l.PurchaseOrderId == poId && !l.IsDeleted && l.Id != lineId)
            .ToListAsync(ct);
        po.RecalcTotalsFromLines(remaining);

        await _db.SaveChangesAsync(ct);
    }

    public async Task SendPurchaseOrderAsync(Guid id, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(id, ct);
        po.Send();
        await _db.SaveChangesAsync(ct);
    }

    public async Task<ReceiptDto> RecordReceiptAsync(Guid poId, RecordReceiptRequest req, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(poId, ct);

        if (!po.CanReceive)
            throw new InvalidOperationException("This PO cannot receive more goods.");

        var count = await _db.PurchaseOrderReceipts.CountAsync(ct) + 1;
        var receiptDate = req.ReceivedDate?.Date ?? DateTime.UtcNow.Date;
        var receipt = new PurchaseOrderReceipt(
            po.OrganizationId, po.Id,
            $"GRN-{count:D6}", receiptDate, req.Notes);

        foreach (var lineReq in req.Lines)
        {
            if (lineReq.Qty <= 0) continue;
            var poLine = po.Lines.FirstOrDefault(l => l.Id == lineReq.LineId)
                ?? throw new InvalidOperationException($"PO line {lineReq.LineId} not found.");
            poLine.Receive(lineReq.Qty);
            receipt.AddLine(poLine.Id, lineReq.Qty);
        }

        po.UpdateReceiptStatus();
        _db.PurchaseOrderReceipts.Add(receipt);
        await _db.SaveChangesAsync(ct);

        return ToReceiptDto(receipt);
    }

    public async Task<IEnumerable<ReceiptDto>> GetReceiptsAsync(Guid poId, CancellationToken ct = default)
    {
        var receipts = await _db.PurchaseOrderReceipts
            .Include(r => r.Lines)
                .ThenInclude(l => l.PurchaseOrderLine)
            .Where(r => r.PurchaseOrderId == poId && !r.IsDeleted)
            .OrderByDescending(r => r.ReceivedDate)
            .ToListAsync(ct);

        return receipts.Select(r => ToReceiptDto(r));
    }

    public async Task ClosePurchaseOrderAsync(Guid id, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(id, ct);
        po.Close();
        await _db.SaveChangesAsync(ct);
    }

    public async Task CancelPurchaseOrderAsync(Guid id, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(id, ct);
        po.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // AP Invoices

    public async Task<IEnumerable<APInvoiceDto>> GetInvoicesAsync(Guid? vendorId = null, CancellationToken ct = default)
    {
        var query = _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .Where(i => !i.IsDeleted);
        if (vendorId.HasValue) query = query.Where(i => i.VendorId == vendorId.Value);
        var list = await query.OrderByDescending(i => i.InvoiceDate).ToListAsync(ct);
        return list.Select(ToAPInvoiceDto);
    }

    public async Task<APInvoiceDto> CreateInvoiceAsync(CreateAPInvoiceRequest req, CancellationToken ct = default)
    {
        var count = await _db.APInvoices.CountAsync(ct) + 1;
        var inv = new APInvoice(_org.OrganizationId, $"APINV-{count:D6}", req.VendorId,
            req.InvoiceDate, req.DueDate, req.Description, req.VendorInvoiceRef,
            req.SubTotal, req.TaxAmount, req.PurchaseOrderId);
        _db.APInvoices.Add(inv);
        await _db.SaveChangesAsync(ct);

        var created = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToAPInvoiceDto(created);
    }

    public async Task<APInvoiceDto> GenerateInvoiceFromPOAsync(Guid poId, string vendorInvoiceRef, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == poId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        if (po.Status == PurchaseOrderStatus.Draft || po.Status == PurchaseOrderStatus.Sent)
            throw new InvalidOperationException("Goods must be received before generating an AP invoice.");
        if (po.Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot invoice a cancelled PO.");
        if (po.InvoiceStatus == POInvoiceStatus.FullyInvoiced)
            throw new InvalidOperationException("All received goods have already been invoiced.");

        var receivedValue      = po.Lines.Sum(l => Math.Round(l.ReceivedQty * l.UnitCost, 4));
        var receivedTax        = po.Lines.Sum(l => Math.Round(l.ReceivedQty * l.UnitCost * l.TaxRate / 100, 4));
        var uninvoicedSubTotal = receivedValue - po.InvoicedAmount;
        var invoiceSubTotal    = Math.Round(uninvoicedSubTotal / (1 + (receivedTax > 0 && receivedValue > 0 ? receivedTax / receivedValue : 0)), 4);
        var invoiceTax         = Math.Round(uninvoicedSubTotal - invoiceSubTotal, 4);

        if (uninvoicedSubTotal <= 0)
            throw new InvalidOperationException("No uninvoiced received value to invoice.");

        var vendor  = po.Vendor!;
        var dueDate = DateTime.UtcNow.Date.AddDays(vendor.PaymentTermsDays);
        var count   = await _db.APInvoices.CountAsync(ct) + 1;

        var previouslyInvoiced = po.InvoicedAmount; // before recording this invoice

        var inv = new APInvoice(_org.OrganizationId, $"APINV-{count:D6}", po.VendorId,
            DateTime.UtcNow.Date, dueDate,
            $"Invoice for {po.PONumber} (received goods)", vendorInvoiceRef,
            invoiceSubTotal, invoiceTax, po.Id);

        _db.APInvoices.Add(inv);
        po.RecordInvoice(uninvoicedSubTotal);

        // Auto-run three-way match
        inv.RunThreeWayMatch(receivedValue, previouslyInvoiced, tolerancePct: 2m);

        await _db.SaveChangesAsync(ct);

        var created = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToAPInvoiceDto(created);
    }

    public async Task<APInvoiceDto> CreatePrepaymentInvoiceAsync(
        CreatePrepaymentInvoiceRequest req, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders
            .Include(o => o.Vendor)
            .FirstOrDefaultAsync(o => o.Id == req.PurchaseOrderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        if (po.Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot raise a prepayment against a cancelled PO.");

        var count = await _db.APInvoices.CountAsync(ct) + 1;
        var inv = new APInvoice(
            _org.OrganizationId, $"PREPAY-{count:D6}", po.VendorId,
            req.InvoiceDate, req.DueDate,
            req.Description ?? $"Prepayment for {po.PONumber}",
            req.VendorInvoiceRef,
            req.Amount, req.TaxAmount, po.Id,
            APInvoiceType.Prepayment);

        _db.APInvoices.Add(inv);
        await _db.SaveChangesAsync(ct);

        var created = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToAPInvoiceDto(created);
    }

    public async Task<ThreeWayMatchDto> RunThreeWayMatchAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices
            .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        if (!inv.PurchaseOrderId.HasValue)
            throw new InvalidOperationException("Three-way match requires a PO-linked invoice.");

        var po = await _db.PurchaseOrders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == inv.PurchaseOrderId.Value && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        var receivedValue      = po.Lines.Sum(l => Math.Round(l.ReceivedQty * l.UnitCost, 4));
        var previouslyInvoiced = po.InvoicedAmount - inv.SubTotal; // exclude this invoice

        var result = inv.RunThreeWayMatch(receivedValue, previouslyInvoiced, tolerancePct: 2m);
        await _db.SaveChangesAsync(ct);

        return new ThreeWayMatchDto(
            inv.Id, result.MatchStatus.ToString(),
            result.ReceivedValue, result.PreviouslyInvoiced,
            result.UninvoicedReceived, result.InvoiceSubTotal,
            result.VariancePct, result.TolerancePct,
            result.QtyException, result.PriceException);
    }

    public async Task<APInvoiceDto> BypassMatchAsync(Guid invoiceId, string reason, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        inv.BypassMatch(reason);
        await _db.SaveChangesAsync(ct);
        return ToAPInvoiceDto(inv);
    }

    public async Task<APInvoiceDto> ApplyPrepaymentAsync(
        Guid invoiceId, Guid prepaymentInvoiceId, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var prepay = await _db.APInvoices
            .FirstOrDefaultAsync(i => i.Id == prepaymentInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Prepayment invoice not found.");

        inv.ApplyPrepayment(prepay);
        await _db.SaveChangesAsync(ct);
        return ToAPInvoiceDto(inv);
    }

    public async Task ApproveInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        inv.Approve();
        await _db.SaveChangesAsync(ct);
    }

    public async Task VoidInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        inv.Void();
        await _db.SaveChangesAsync(ct);
    }

    // AP Payments

    public async Task<IEnumerable<APPaymentDto>> GetPaymentsAsync(Guid? vendorId = null, CancellationToken ct = default)
    {
        var query = _db.APPayments
            .Include(p => p.Vendor).Include(p => p.APInvoice)
            .Where(p => !p.IsDeleted);
        if (vendorId.HasValue) query = query.Where(p => p.VendorId == vendorId.Value);
        var list = await query.OrderByDescending(p => p.PaymentDate).ToListAsync(ct);
        return list.Select(p => new APPaymentDto(
            p.Id, p.PaymentNumber, p.VendorId, p.Vendor?.Name ?? string.Empty,
            p.APInvoiceId, p.APInvoice?.InvoiceNumber ?? string.Empty,
            p.PaymentDate, p.Amount, p.PaymentMethod,
            p.Reference, p.Status.ToString(), p.CreatedAt));
    }

    public async Task<APPaymentDto> CreatePaymentAsync(CreateAPPaymentRequest req, CancellationToken ct = default)
    {
        var invoice = await _db.APInvoices.FirstOrDefaultAsync(i => i.Id == req.APInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var count = await _db.APPayments.CountAsync(ct) + 1;
        var payment = new APPayment(_org.OrganizationId, $"PAY-{count:D6}", req.VendorId, req.APInvoiceId,
            req.PaymentDate, req.Amount, req.PaymentMethod, req.Reference);

        invoice.ApplyPayment(req.Amount);
        _db.APPayments.Add(payment);
        payment.Post();
        await _db.SaveChangesAsync(ct);

        var created = await _db.APPayments
            .Include(p => p.Vendor).Include(p => p.APInvoice)
            .FirstAsync(p => p.Id == payment.Id, ct);
        return new APPaymentDto(
            created.Id, created.PaymentNumber, created.VendorId,
            created.Vendor?.Name ?? string.Empty,
            created.APInvoiceId, created.APInvoice?.InvoiceNumber ?? string.Empty,
            created.PaymentDate, created.Amount, created.PaymentMethod,
            created.Reference, created.Status.ToString(), created.CreatedAt);
    }

    // AP Aging Report

    public async Task<IEnumerable<APAgingDto>> GetAgingReportAsync(CancellationToken ct = default)
    {
        var invoices = await _db.APInvoices
            .Include(i => i.Vendor)
            .Where(i => !i.IsDeleted && i.Status != APInvoiceStatus.Paid && i.Status != APInvoiceStatus.Voided)
            .ToListAsync(ct);

        return invoices
            .GroupBy(i => new { i.VendorId, i.Vendor!.VendorNumber, i.Vendor.Name })
            .Select(g => new APAgingDto(
                g.Key.VendorNumber, g.Key.Name,
                g.Where(i => i.DaysOutstanding == 0).Sum(i => i.OutstandingAmount),
                g.Where(i => i.DaysOutstanding is > 0 and <= 30).Sum(i => i.OutstandingAmount),
                g.Where(i => i.DaysOutstanding is > 30 and <= 60).Sum(i => i.OutstandingAmount),
                g.Where(i => i.DaysOutstanding is > 60 and <= 90).Sum(i => i.OutstandingAmount),
                g.Where(i => i.DaysOutstanding > 90).Sum(i => i.OutstandingAmount),
                g.Sum(i => i.OutstandingAmount)))
            .OrderByDescending(r => r.Total);
    }

    // Helpers

    private async Task<PurchaseOrder> LoadPOWithLines(Guid id, CancellationToken ct)
    {
        return await _db.PurchaseOrders.Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");
    }

    public async Task<VendorLedgerDto> GetVendorLedgerAsync(Guid vendorId, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == vendorId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");

        var invoices = await _db.APInvoices
            .Include(i => i.PurchaseOrder)
            .Where(i => i.VendorId == vendorId && !i.IsDeleted)
            .OrderBy(i => i.InvoiceDate)
            .ToListAsync(ct);

        var payments = await _db.APPayments
            .Include(p => p.APInvoice)
            .Where(p => p.VendorId == vendorId && !p.IsDeleted)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync(ct);

        var entries = new List<(DateTime Date, VendorLedgerEntryDto Dto)>();

        foreach (var inv in invoices)
        {
            entries.Add((inv.InvoiceDate, new VendorLedgerEntryDto(
                "Invoice", inv.InvoiceNumber, inv.InvoiceDate,
                0, inv.TotalAmount, 0,
                inv.Status.ToString(), inv.PurchaseOrder?.PONumber)));
        }
        foreach (var pay in payments)
        {
            entries.Add((pay.PaymentDate, new VendorLedgerEntryDto(
                "Payment", pay.PaymentNumber, pay.PaymentDate,
                pay.Amount, 0, 0,
                pay.Status.ToString(), null)));
        }

        entries.Sort((a, b) => a.Date.CompareTo(b.Date));

        decimal running = 0;
        var finalEntries = entries.Select(e =>
        {
            running += e.Dto.Credit - e.Dto.Debit;
            return e.Dto with { RunningBalance = running };
        }).ToList();

        var totalInvoiced = invoices.Sum(i => i.TotalAmount);
        var totalPaid     = payments.Sum(p => p.Amount);

        return new VendorLedgerDto(
            vendorId, vendor.Name, vendor.VendorNumber,
            totalInvoiced, totalPaid, totalInvoiced - totalPaid,
            finalEntries);
    }

    // ── Vendor Addresses ──────────────────────────────────────────────────────

    public async Task<IEnumerable<VendorAddressDto>> GetVendorAddressesAsync(Guid vendorId, CancellationToken ct = default)
    {
        var addresses = await _db.VendorAddresses
            .Where(a => a.VendorId == vendorId && !a.IsDeleted)
            .OrderByDescending(a => a.IsPrimary).ThenBy(a => a.Label)
            .ToListAsync(ct);
        return addresses.Select(ToVendorAddressDto);
    }

    public async Task<VendorAddressDto> SaveVendorAddressAsync(Guid vendorId, Guid? addressId, SaveVendorAddressRequest req, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == vendorId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");

        if (!Enum.TryParse<VendorAddressType>(req.AddressType, out var addrType))
            addrType = VendorAddressType.Billing;

        VendorAddress address;
        if (addressId.HasValue)
        {
            address = await _db.VendorAddresses
                .FirstOrDefaultAsync(a => a.Id == addressId.Value && a.VendorId == vendorId && !a.IsDeleted, ct)
                ?? throw new InvalidOperationException("Address not found.");
            address.Update(req.Label, addrType, req.Line1, req.Line2, req.City, req.State, req.PostalCode, req.Country ?? "US");
        }
        else
        {
            var isFirst = !await _db.VendorAddresses.AnyAsync(a => a.VendorId == vendorId && !a.IsDeleted, ct);
            address = new VendorAddress(_org.OrganizationId, vendorId, req.Label, addrType,
                req.Line1, req.Line2, req.City, req.State, req.PostalCode, req.Country ?? "US", isFirst);
            _db.VendorAddresses.Add(address);
        }
        await _db.SaveChangesAsync(ct);
        return ToVendorAddressDto(address);
    }

    public async Task DeleteVendorAddressAsync(Guid vendorId, Guid addressId, CancellationToken ct = default)
    {
        var address = await _db.VendorAddresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.VendorId == vendorId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Address not found.");
        address.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetPrimaryVendorAddressAsync(Guid vendorId, Guid addressId, CancellationToken ct = default)
    {
        var all = await _db.VendorAddresses
            .Where(a => a.VendorId == vendorId && !a.IsDeleted)
            .ToListAsync(ct);
        foreach (var a in all) a.ClearPrimary();
        var target = all.FirstOrDefault(a => a.Id == addressId)
            ?? throw new InvalidOperationException("Address not found.");
        target.SetPrimary();
        await _db.SaveChangesAsync(ct);
    }

    // ── Vendor Contacts ───────────────────────────────────────────────────────

    public async Task<IEnumerable<VendorContactDto>> GetVendorContactsAsync(Guid vendorId, CancellationToken ct = default)
    {
        var contacts = await _db.VendorContacts
            .Where(c => c.VendorId == vendorId && !c.IsDeleted)
            .OrderByDescending(c => c.IsPrimary).ThenBy(c => c.Name)
            .ToListAsync(ct);
        return contacts.Select(ToVendorContactDto);
    }

    public async Task<VendorContactDto> SaveVendorContactAsync(Guid vendorId, Guid? contactId, SaveVendorContactRequest req, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == vendorId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");

        VendorContact contact;
        if (contactId.HasValue)
        {
            contact = await _db.VendorContacts
                .FirstOrDefaultAsync(c => c.Id == contactId.Value && c.VendorId == vendorId && !c.IsDeleted, ct)
                ?? throw new InvalidOperationException("Contact not found.");
            contact.Update(req.Name, req.Title, req.Email, req.Phone, req.Mobile, req.Notes);
        }
        else
        {
            var isFirst = !await _db.VendorContacts.AnyAsync(c => c.VendorId == vendorId && !c.IsDeleted, ct);
            contact = new VendorContact(_org.OrganizationId, vendorId, req.Name, req.Title, req.Email, req.Phone, req.Mobile, req.Notes, isFirst);
            _db.VendorContacts.Add(contact);
        }
        await _db.SaveChangesAsync(ct);
        return ToVendorContactDto(contact);
    }

    public async Task DeleteVendorContactAsync(Guid vendorId, Guid contactId, CancellationToken ct = default)
    {
        var contact = await _db.VendorContacts
            .FirstOrDefaultAsync(c => c.Id == contactId && c.VendorId == vendorId && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Contact not found.");
        contact.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetPrimaryVendorContactAsync(Guid vendorId, Guid contactId, CancellationToken ct = default)
    {
        var all = await _db.VendorContacts
            .Where(c => c.VendorId == vendorId && !c.IsDeleted)
            .ToListAsync(ct);
        foreach (var c in all) c.ClearPrimary();
        var target = all.FirstOrDefault(c => c.Id == contactId)
            ?? throw new InvalidOperationException("Contact not found.");
        target.SetPrimary();
        await _db.SaveChangesAsync(ct);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static VendorAddressDto ToVendorAddressDto(VendorAddress a) =>
        new(a.Id, a.Label, a.AddressType.ToString(), a.IsPrimary,
            a.Line1, a.Line2, a.City, a.State, a.PostalCode, a.Country, a.SingleLine);

    private static VendorContactDto ToVendorContactDto(VendorContact c) =>
        new(c.Id, c.Name, c.Title, c.Email, c.Phone, c.Mobile, c.IsPrimary, c.Notes);
    private static VendorDto ToVendorDto(Vendor v, decimal outstanding) =>
        new(v.Id, v.VendorNumber, v.Name,
            v.Email, v.Phone, v.BillingAddress, v.ShippingAddress,
            v.Website, v.Notes, v.Currency, v.PaymentTermsDays, v.TaxId,
            v.BankAccountName, v.BankAccountNumber, v.BankRoutingNumber,
            outstanding, v.Status.ToString(), v.CreatedAt,
            v.Addresses?.Select(ToVendorAddressDto).ToList(),
            v.Contacts?.Select(ToVendorContactDto).ToList());

    private static PurchaseOrderDto ToPODto(PurchaseOrder o) =>
        new(o.Id, o.PONumber, o.VendorId, o.Vendor?.Name ?? string.Empty,
            o.OrderDate, o.ExpectedDate, o.Description, o.Currency,
            o.Status.ToString(), o.InvoiceStatus.ToString(),
            o.SubTotal, o.TaxTotal, o.GrandTotal, o.InvoicedAmount,
            o.CanReceive, o.CreatedAt,
            o.Lines.Select(l => new PurchaseOrderLineDto(
                l.Id, l.ProductVariantId, l.ProductCode, l.Description,
                l.UnitOfMeasure, l.OrderedQty, l.ReceivedQty,
                l.UnitCost, l.TaxRate, l.LineTotal,
                l.IsFullyReceived, l.OutstandingQty)).ToList());

    private static ReceiptDto ToReceiptDto(PurchaseOrderReceipt r) =>
        new(r.Id, r.ReceiptNumber, r.ReceivedDate, r.Notes, r.CreatedAt,
            r.Lines.Select(l => new ReceiptLineDto(
                l.Id, l.PurchaseOrderLineId,
                l.PurchaseOrderLine?.ProductCode ?? string.Empty,
                l.PurchaseOrderLine?.Description ?? string.Empty,
                l.Qty)).ToList());

    private static APInvoiceDto ToAPInvoiceDto(APInvoice i) =>
        new(i.Id, i.InvoiceNumber, i.VendorId, i.Vendor?.Name ?? string.Empty,
            i.PurchaseOrderId, i.PurchaseOrder?.PONumber,
            i.InvoiceDate, i.DueDate, i.Description, i.VendorInvoiceRef,
            i.SubTotal, i.TaxAmount, i.TotalAmount,
            i.PaidAmount, i.PrepaymentApplied,
            i.TotalAmount - i.PaidAmount - i.PrepaymentApplied,
            i.Status.ToString(), i.InvoiceType.ToString(), i.MatchStatus.ToString(),
            i.MatchNotes, i.BypassReason,
            i.LinkedPrepaymentInvoiceId, i.LinkedPrepaymentInvoice?.InvoiceNumber,
            (int)(DateTime.UtcNow - i.InvoiceDate).TotalDays, i.CreatedAt);

    // ── Purchase Requisitions ─────────────────────────────────────────────────

    public async Task<IEnumerable<PRSummaryDto>> GetRequisitionsAsync(
        string? status = null, CancellationToken ct = default)
    {
        var query = _db.PurchaseRequisitions.Include(r => r.Lines).AsQueryable();
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<PRStatus>(status, out var s))
            query = query.Where(r => r.Status == s);
        var list = await query.OrderByDescending(r => r.CreatedAt).ToListAsync(ct);
        return list.Select(r => new PRSummaryDto(
            r.Id, r.RequisitionNumber, r.RequestedBy, r.DepartmentCode,
            r.NeededByDate, r.Status.ToString(), r.TotalEstimatedCost,
            r.Lines.Count, r.WorkflowInstanceId, r.ConvertedToPOId, r.CreatedAt));
    }

    public async Task<PRDto?> GetRequisitionAsync(Guid id, CancellationToken ct = default)
    {
        var r = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return r is null ? null : ToPRDto(r);
    }

    public async Task<PRDto> CreateRequisitionAsync(CreatePRRequest req, CancellationToken ct = default)
    {
        var count = await _db.PurchaseRequisitions.CountAsync(ct) + 1;
        var pr = new PurchaseRequisition(_org.OrganizationId,
            $"PR-{req.NeededByDate:yyyy}-{count:D5}", req.RequestedBy, req.NeededByDate,
            req.DepartmentCode, req.CostCenterCode, req.Notes);

        if (req.Lines?.Any() == true)
        {
            foreach (var l in req.Lines)
                pr.AddLine(l.ProductId, l.Description, l.Quantity, l.UnitOfMeasure,
                    l.EstimatedUnitCost, l.SuggestedVendorId, l.GlAccountCode);
        }

        _db.PurchaseRequisitions.Add(pr);
        await _db.SaveChangesAsync(ct);
        return ToPRDto(pr);
    }

    public async Task<PRDto> AddPRLineAsync(Guid prId, AddPRLineRequest req, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == prId, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        pr.AddLine(req.ProductId, req.Description, req.Quantity, req.UnitOfMeasure,
            req.EstimatedUnitCost, req.SuggestedVendorId, req.GlAccountCode);
        await _db.SaveChangesAsync(ct);
        return ToPRDto(pr);
    }

    public async Task RemovePRLineAsync(Guid prId, Guid lineId, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == prId, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        pr.RemoveLine(lineId);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<PRDto> SubmitRequisitionAsync(Guid id, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        pr.Submit();
        await _db.SaveChangesAsync(ct);
        return ToPRDto(pr);
    }

    public async Task<PRDto> ApproveRequisitionAsync(Guid id, string approvedBy, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        // Create a simple workflow instance for tracking
        var wi = new DessertERP.Domain.Modules.Workflow.WorkflowInstance(
            _org.OrganizationId, DessertERP.Domain.Modules.Workflow.WorkflowDocumentType.PurchaseOrder,
            id, pr.RequisitionNumber, pr.TotalEstimatedCost, approvedBy);
        _db.WorkflowInstances.Add(wi);
        await _db.SaveChangesAsync(ct);
        pr.WorkflowApprove(wi.Id);
        await _db.SaveChangesAsync(ct);
        return ToPRDto(pr);
    }

    public async Task<PRDto> RejectRequisitionAsync(Guid id, string rejectedBy, string reason, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        var wi = new DessertERP.Domain.Modules.Workflow.WorkflowInstance(
            _org.OrganizationId, DessertERP.Domain.Modules.Workflow.WorkflowDocumentType.PurchaseOrder,
            id, pr.RequisitionNumber, pr.TotalEstimatedCost, rejectedBy);
        _db.WorkflowInstances.Add(wi);
        await _db.SaveChangesAsync(ct);
        pr.WorkflowReject(wi.Id, reason);
        await _db.SaveChangesAsync(ct);
        return ToPRDto(pr);
    }

    public async Task<PurchaseOrderDto> ConvertRequisitionToPOAsync(Guid prId, ConvertPRToPORequest req, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == prId, ct)
            ?? throw new InvalidOperationException("Requisition not found.");

        if (pr.Status != PRStatus.Approved)
            throw new InvalidOperationException("Only Approved requisitions can be converted to a PO.");

        var count = await _db.PurchaseOrders.CountAsync(ct) + 1;
        var po = new PurchaseOrder(_org.OrganizationId, $"PO-{req.OrderDate:yyyy}-{count:D5}",
            req.VendorId, req.OrderDate,
            $"Converted from {pr.RequisitionNumber}", req.Currency ?? "USD", req.ExpectedDate);

        // Add lines from the PR
        foreach (var prl in pr.Lines)
        {
            if (req.ProductVariantIds?.TryGetValue(prl.Id, out var variantId) == true && variantId != Guid.Empty)
            {
                var line = po.AddLine(variantId, prl.GlAccountCode ?? prl.Description,
                    prl.Description, prl.UnitOfMeasure, prl.Quantity, prl.EstimatedUnitCost);
                _db.PurchaseOrderLines.Add(line);
            }
        }

        _db.PurchaseOrders.Add(po);
        pr.MarkConverted(po.Id);
        await _db.SaveChangesAsync(ct);
        return (await GetPurchaseOrderAsync(po.Id, ct))!;
    }

    public async Task CancelRequisitionAsync(Guid id, CancellationToken ct = default)
    {
        var pr = await _db.PurchaseRequisitions.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new InvalidOperationException("Requisition not found.");
        pr.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // ── PO Workflow ───────────────────────────────────────────────────────────

    public async Task SubmitPOForApprovalAsync(Guid poId, string submittedBy, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(poId, ct);
        var wi = new DessertERP.Domain.Modules.Workflow.WorkflowInstance(
            _org.OrganizationId, DessertERP.Domain.Modules.Workflow.WorkflowDocumentType.PurchaseOrder,
            poId, po.PONumber, po.GrandTotal, submittedBy);
        _db.WorkflowInstances.Add(wi);
        await _db.SaveChangesAsync(ct);
        po.SubmitForApproval(wi.Id);
        await _db.SaveChangesAsync(ct);
    }

    public async Task POWorkflowApprovedAsync(Guid workflowInstanceId, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders.IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.WorkflowInstanceId == workflowInstanceId, ct)
            ?? throw new InvalidOperationException("No PO linked to this workflow instance.");
        po.WorkflowApproved();
        await _db.SaveChangesAsync(ct);
    }

    public async Task POWorkflowRejectedAsync(Guid workflowInstanceId, string reason, CancellationToken ct = default)
    {
        var po = await _db.PurchaseOrders.IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.WorkflowInstanceId == workflowInstanceId, ct)
            ?? throw new InvalidOperationException("No PO linked to this workflow instance.");
        po.WorkflowRejected(reason);
        await _db.SaveChangesAsync(ct);
    }

    // ── Invoice Workflow ──────────────────────────────────────────────────────

    public async Task SubmitInvoiceForApprovalAsync(Guid invoiceId, string submittedBy, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices
            .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        var wi = new DessertERP.Domain.Modules.Workflow.WorkflowInstance(
            _org.OrganizationId, DessertERP.Domain.Modules.Workflow.WorkflowDocumentType.APInvoice,
            invoiceId, inv.InvoiceNumber, inv.TotalAmount, submittedBy);
        _db.WorkflowInstances.Add(wi);
        await _db.SaveChangesAsync(ct);
        inv.SubmitForApproval(wi.Id);
        await _db.SaveChangesAsync(ct);
    }

    public async Task InvoiceWorkflowApprovedAsync(Guid workflowInstanceId, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.WorkflowInstanceId == workflowInstanceId, ct)
            ?? throw new InvalidOperationException("No invoice linked to this workflow instance.");
        inv.WorkflowApproved();
        await _db.SaveChangesAsync(ct);
    }

    public async Task InvoiceWorkflowRejectedAsync(Guid workflowInstanceId, CancellationToken ct = default)
    {
        var inv = await _db.APInvoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.WorkflowInstanceId == workflowInstanceId, ct)
            ?? throw new InvalidOperationException("No invoice linked to this workflow instance.");
        inv.WorkflowRejected();
        await _db.SaveChangesAsync(ct);
    }

    // ── Payment Proposals ─────────────────────────────────────────────────────

    public async Task<IEnumerable<PaymentProposalSummaryDto>> GetPaymentProposalsAsync(CancellationToken ct = default)
    {
        var list = await _db.PaymentProposals.Include(p => p.Lines)
            .OrderByDescending(p => p.ProposalDate).ToListAsync(ct);
        return list.Select(p => new PaymentProposalSummaryDto(
            p.Id, p.ProposalNumber, p.ProposalDate, p.PaymentDate,
            p.PaymentMethod, p.Status.ToString(), p.TotalAmount,
            p.Lines.Count, p.ProcessedAt, p.ProcessedBy, p.CreatedAt));
    }

    public async Task<PaymentProposalDto?> GetPaymentProposalAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _db.PaymentProposals.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return p is null ? null : ToProposalDto(p);
    }

    public async Task<PaymentProposalDto> CreatePaymentProposalAsync(CreatePaymentProposalRequest req, CancellationToken ct = default)
    {
        var count = await _db.PaymentProposals.CountAsync(ct) + 1;
        var proposal = new PaymentProposal(_org.OrganizationId,
            $"PAY-{req.ProposalDate:yyyy}-{count:D5}",
            req.ProposalDate, req.PaymentDate, req.PaymentMethod, req.BankAccount, req.Notes);
        _db.PaymentProposals.Add(proposal);
        await _db.SaveChangesAsync(ct);
        return ToProposalDto(proposal);
    }

    public async Task<PaymentProposalDto> AddProposalLineAsync(Guid proposalId, Guid invoiceId, CancellationToken ct = default)
    {
        var proposal = await _db.PaymentProposals.Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == proposalId, ct)
            ?? throw new InvalidOperationException("Proposal not found.");
        var inv = await _db.APInvoices.Include(i => i.Vendor)
            .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var outstanding = inv.TotalAmount - inv.PaidAmount - inv.PrepaymentApplied;
        var line = proposal.AddLine(inv.Id, inv.InvoiceNumber, inv.VendorId,
            inv.Vendor?.Name ?? string.Empty, outstanding, inv.DueDate);
        _db.PaymentProposalLines.Add(line);
        await _db.SaveChangesAsync(ct);
        return ToProposalDto(proposal);
    }

    public async Task RemoveProposalLineAsync(Guid proposalId, Guid lineId, CancellationToken ct = default)
    {
        var proposal = await _db.PaymentProposals.Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == proposalId, ct)
            ?? throw new InvalidOperationException("Proposal not found.");
        proposal.RemoveLine(lineId);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<PaymentProposalDto> ApprovePaymentProposalAsync(Guid id, CancellationToken ct = default)
    {
        var proposal = await _db.PaymentProposals.Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidOperationException("Proposal not found.");
        proposal.Approve();
        await _db.SaveChangesAsync(ct);
        return ToProposalDto(proposal);
    }

    public async Task<PaymentProposalDto> ProcessPaymentProposalAsync(Guid id, string processedBy, CancellationToken ct = default)
    {
        var proposal = await _db.PaymentProposals.Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidOperationException("Proposal not found.");

        var payCount = await _db.APPayments.CountAsync(ct);
        foreach (var line in proposal.Lines.Where(l => l.APPaymentId is null))
        {
            payCount++;
            var inv = await _db.APInvoices.FindAsync(new object[] { line.APInvoiceId }, ct)
                ?? throw new InvalidOperationException($"Invoice {line.APInvoiceId} not found.");

            var payment = new APPayment(_org.OrganizationId, $"PMT-{DateTime.UtcNow:yyyy}-{payCount:D6}",
                line.VendorId, line.APInvoiceId, proposal.PaymentDate, line.ProposedAmount,
                proposal.PaymentMethod, proposal.BankAccount);
            _db.APPayments.Add(payment);
            await _db.SaveChangesAsync(ct);

            inv.ApplyPayment(line.ProposedAmount);
            line.SetPayment(payment.Id);
        }

        proposal.MarkProcessed(processedBy);
        await _db.SaveChangesAsync(ct);
        return ToProposalDto(proposal);
    }

    public async Task CancelPaymentProposalAsync(Guid id, CancellationToken ct = default)
    {
        var proposal = await _db.PaymentProposals.Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidOperationException("Proposal not found.");
        proposal.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // ── Vendor Credit Notes ───────────────────────────────────────────────────

    public async Task<IEnumerable<CreditNoteDto>> GetCreditNotesAsync(
        Guid? vendorId = null, CancellationToken ct = default)
    {
        var query = _db.VendorCreditNotes.Include(c => c.Vendor).AsQueryable();
        if (vendorId.HasValue) query = query.Where(c => c.VendorId == vendorId.Value);
        var list = await query.OrderByDescending(c => c.CreditDate).ToListAsync(ct);
        return list.Select(ToCreditNoteDto);
    }

    public async Task<CreditNoteDto> CreateCreditNoteAsync(CreateCreditNoteRequest req, CancellationToken ct = default)
    {
        var count = await _db.VendorCreditNotes.CountAsync(ct) + 1;
        if (!Enum.TryParse<CreditNoteReason>(req.Reason, out var reason))
            reason = CreditNoteReason.Other;
        var cn = new VendorCreditNote(_org.OrganizationId, $"CN-{count:D6}",
            req.VendorId, req.CreditDate, req.Description,
            req.SubTotal, req.TaxAmount, reason,
            req.APInvoiceId, req.PurchaseOrderId, req.VendorCNRef, req.Notes);
        _db.VendorCreditNotes.Add(cn);
        await _db.SaveChangesAsync(ct);
        // Reload with vendor navigation populated
        var saved = await _db.VendorCreditNotes.Include(c => c.Vendor)
            .FirstAsync(c => c.Id == cn.Id, ct);
        return ToCreditNoteDto(saved);
    }

    public async Task<CreditNoteDto> PostCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.VendorCreditNotes.Include(c => c.Vendor)
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.Post();
        await _db.SaveChangesAsync(ct);
        return ToCreditNoteDto(cn);
    }

    public async Task<CreditNoteDto> ApplyCreditNoteAsync(Guid id, Guid invoiceId, decimal amount, CancellationToken ct = default)
    {
        var cn = await _db.VendorCreditNotes.Include(c => c.Vendor)
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        var inv = await _db.APInvoices.FindAsync(new object[] { invoiceId }, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        cn.ApplyCredit(amount);
        inv.ApplyPayment(amount);  // credits reduce the invoice balance
        await _db.SaveChangesAsync(ct);
        return ToCreditNoteDto(cn);
    }

    public async Task VoidCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.VendorCreditNotes.Include(c => c.Vendor)
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.Void();
        await _db.SaveChangesAsync(ct);
    }

    // ── P2P Mapping helpers ───────────────────────────────────────────────────

    private static PRDto ToPRDto(PurchaseRequisition r) =>
        new(r.Id, r.RequisitionNumber, r.RequestedBy, r.DepartmentCode, r.CostCenterCode,
            r.NeededByDate, r.Status.ToString(), r.TotalEstimatedCost,
            r.WorkflowInstanceId, r.ConvertedToPOId, r.RejectionReason, r.Notes, r.CreatedAt,
            r.Lines.Select(l => new PRLineDto(
                l.Id, l.LineNumber, l.ProductId, l.Description, l.Quantity,
                l.UnitOfMeasure, l.EstimatedUnitCost, l.EstimatedTotalCost,
                l.SuggestedVendorId, l.GlAccountCode, l.Notes)).ToList());

    private static PaymentProposalDto ToProposalDto(PaymentProposal p) =>
        new(p.Id, p.ProposalNumber, p.ProposalDate, p.PaymentDate,
            p.PaymentMethod, p.BankAccount, p.Status.ToString(), p.TotalAmount,
            p.ProcessedAt, p.ProcessedBy, p.Notes, p.CreatedAt,
            p.Lines.Select(l => new PaymentProposalLineDto(
                l.Id, l.APInvoiceId, l.InvoiceNumber,
                l.VendorId, l.VendorName, l.ProposedAmount, l.InvoiceDueDate,
                l.APPaymentId)).ToList());

    private static CreditNoteDto ToCreditNoteDto(VendorCreditNote c) =>
        new(c.Id, c.CreditNoteNumber, c.VendorId, c.Vendor?.Name ?? string.Empty,
            c.APInvoiceId, c.PurchaseOrderId, c.CreditDate, c.Description,
            c.VendorCNRef, c.SubTotal, c.TaxAmount, c.TotalAmount,
            c.AppliedAmount, c.AvailableCredit,
            c.Status.ToString(), c.Reason.ToString(), c.Notes, c.CreatedAt);
}
