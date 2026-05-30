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
    Task ReceiveGoodsAsync(Guid poId, IEnumerable<ReceiveGoodsRequest> receipts, CancellationToken ct = default);
    Task CancelPurchaseOrderAsync(Guid id, CancellationToken ct = default);

    // AP Invoices
    Task<IEnumerable<APInvoiceDto>> GetInvoicesAsync(Guid? vendorId = null, CancellationToken ct = default);
    Task<APInvoiceDto> CreateInvoiceAsync(CreateAPInvoiceRequest req, CancellationToken ct = default);
    Task<APInvoiceDto> GenerateInvoiceFromPOAsync(Guid poId, string vendorInvoiceRef, CancellationToken ct = default);
    Task ApproveInvoiceAsync(Guid id, CancellationToken ct = default);
    Task VoidInvoiceAsync(Guid id, CancellationToken ct = default);

    // AP Payments
    Task<IEnumerable<APPaymentDto>> GetPaymentsAsync(Guid? vendorId = null, CancellationToken ct = default);
    Task<APPaymentDto> CreatePaymentAsync(CreateAPPaymentRequest req, CancellationToken ct = default);

    // Reports
    Task<IEnumerable<APAgingDto>> GetAgingReportAsync(CancellationToken ct = default);
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

    // ── Vendors ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<VendorDto>> GetVendorsAsync(CancellationToken ct = default)
    {
        var list = await _db.Vendors.Where(v => !v.IsDeleted).OrderBy(v => v.VendorNumber).ToListAsync(ct);
        return list.Select(ToVendorDto);
    }

    public async Task<VendorDto> CreateVendorAsync(CreateVendorRequest req, CancellationToken ct = default)
    {
        var count = await _db.Vendors.CountAsync(ct) + 1;
        var vendor = new Vendor(_org.OrganizationId, $"VEND-{count:D5}", req.Name, req.Email, req.Phone,
            req.Address, req.Currency, req.PaymentTermsDays, req.TaxId);
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync(ct);
        return ToVendorDto(vendor);
    }

    public async Task<VendorDto> UpdateVendorAsync(Guid id, UpdateVendorRequest req, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");
        vendor.Update(req.Name, req.Email, req.Phone, req.Address,
            req.PaymentTermsDays, req.BankAccountName, req.BankAccountNumber);
        await _db.SaveChangesAsync(ct);
        return ToVendorDto(vendor);
    }

    public async Task DeleteVendorAsync(Guid id, CancellationToken ct = default)
    {
        var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Vendor not found.");
        // Check if used in any open PO
        var hasOpenPO = await _db.PurchaseOrders
            .AnyAsync(p => p.VendorId == id && !p.IsDeleted &&
                p.Status != PurchaseOrderStatus.Closed && p.Status != PurchaseOrderStatus.Cancelled, ct);
        if (hasOpenPO)
            throw new InvalidOperationException("Cannot delete a vendor with open purchase orders.");
        vendor.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Purchase Orders ───────────────────────────────────────────────────────

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
            o.OrderDate, o.ExpectedDate, o.Status.ToString(),
            o.GrandTotal, o.Lines.Count, o.CreatedAt));
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

        // Validate the variant exists
        var variantExists = await _db.ProductVariants
            .IgnoreQueryFilters()
            .AnyAsync(v => v.Id == req.ProductVariantId && !v.IsDeleted, ct);
        if (!variantExists)
            throw new InvalidOperationException("Product variant not found.");

        var line = po.AddLine(req.ProductVariantId, req.ProductCode, req.Description,
            req.UnitOfMeasure, req.Quantity, req.UnitCost, req.TaxRate);
        _db.PurchaseOrderLines.Add(line);   // explicit tracking — avoids concurrency exception
        await _db.SaveChangesAsync(ct);
        return (await GetPurchaseOrderAsync(poId, ct))!;
    }

    public async Task RemovePOLineAsync(Guid poId, Guid lineId, CancellationToken ct = default)
    {
        // Load PO without Include, then load the specific line directly from DbSet.
        // This avoids EF collection-tracking issues caused by global query filters on PurchaseOrderLine.
        var po = await _db.PurchaseOrders
            .FirstOrDefaultAsync(o => o.Id == poId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");

        var line = await _db.PurchaseOrderLines
            .FirstOrDefaultAsync(l => l.Id == lineId && l.PurchaseOrderId == poId && !l.IsDeleted, ct)
            ?? throw new InvalidOperationException("Line not found.");

        // Validate via domain (status check)
        po.ValidateCanRemoveLine();
        line.SoftDelete();

        // Recalc PO totals from remaining active lines
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

    public async Task ReceiveGoodsAsync(Guid poId, IEnumerable<ReceiveGoodsRequest> receipts, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(poId, ct);
        foreach (var r in receipts)
        {
            var line = po.Lines.FirstOrDefault(l => l.Id == r.LineId)
                ?? throw new InvalidOperationException($"Line {r.LineId} not found.");
            line.Receive(r.ReceivedQty);
        }
        po.UpdateReceiptStatus();
        await _db.SaveChangesAsync(ct);
    }

    public async Task CancelPurchaseOrderAsync(Guid id, CancellationToken ct = default)
    {
        var po = await LoadPOWithLines(id, ct);
        po.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // ── AP Invoices ───────────────────────────────────────────────────────────

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

        if (po.Status != PurchaseOrderStatus.FullyReceived && po.Status != PurchaseOrderStatus.PartiallyReceived)
            throw new InvalidOperationException("Goods must be received before generating an AP invoice.");

        var vendor = po.Vendor!;
        var dueDate = DateTime.UtcNow.Date.AddDays(vendor.PaymentTermsDays);
        var count = await _db.APInvoices.CountAsync(ct) + 1;

        var inv = new APInvoice(_org.OrganizationId, $"APINV-{count:D6}", po.VendorId,
            DateTime.UtcNow.Date, dueDate,
            $"Invoice for {po.PONumber}", vendorInvoiceRef,
            po.SubTotal, po.TaxTotal, po.Id);

        _db.APInvoices.Add(inv);
        po.Invoice(inv.Id);
        await _db.SaveChangesAsync(ct);

        var created = await _db.APInvoices
            .Include(i => i.Vendor).Include(i => i.PurchaseOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToAPInvoiceDto(created);
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

    // ── AP Payments ───────────────────────────────────────────────────────────

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

    // ── AP Aging Report ───────────────────────────────────────────────────────

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

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<PurchaseOrder> LoadPOWithLines(Guid id, CancellationToken ct)
    {
        return await _db.PurchaseOrders.Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Purchase order not found.");
    }

    private static VendorDto ToVendorDto(Vendor v) => new(
        v.Id, v.VendorNumber, v.Name, v.Email, v.Phone, v.Address,
        v.Currency, v.PaymentTermsDays, v.TaxId,
        v.BankAccountName, v.BankAccountNumber, v.Status.ToString(), v.CreatedAt);

    private static PurchaseOrderDto ToPODto(PurchaseOrder o) => new(
        o.Id, o.PONumber, o.VendorId, o.Vendor?.Name ?? string.Empty,
        o.OrderDate, o.ExpectedDate, o.Description, o.Currency,
        o.Status.ToString(), o.SubTotal, o.TaxTotal, o.GrandTotal,
        o.APInvoiceId, o.CreatedAt,
        o.Lines.Select(l => new PurchaseOrderLineDto(
            l.Id, l.ProductVariantId, l.ProductCode, l.Description, l.UnitOfMeasure,
            l.OrderedQty, l.ReceivedQty, l.UnitCost, l.TaxRate,
            l.LineTotal, l.IsFullyReceived)).ToList());

    private static APInvoiceDto ToAPInvoiceDto(APInvoice i) => new(
        i.Id, i.InvoiceNumber, i.VendorId, i.Vendor?.Name ?? string.Empty,
        i.PurchaseOrderId, i.PurchaseOrder?.PONumber,
        i.InvoiceDate, i.DueDate, i.Description, i.VendorInvoiceRef,
        i.SubTotal, i.TaxAmount, i.TotalAmount, i.PaidAmount,
        i.OutstandingAmount, i.Status.ToString(), i.DaysOutstanding, i.CreatedAt);
}
