using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Services;
using DessertERP.Domain.Common;
using DessertERP.Application.Modules.AccountsReceivable.DTOs;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.AccountsReceivable.Services;

public interface IAccountsReceivableService
{
    // Customers
    Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken ct = default);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest req, CancellationToken ct = default);
    Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequest req, CancellationToken ct = default);
    Task<CustomerLedgerDto> GetCustomerLedgerAsync(Guid customerId, CancellationToken ct = default);

    // Sales Orders
    Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersAsync(string? status = null, Guid? customerId = null, CancellationToken ct = default);
    Task<SalesOrderDto?> GetSalesOrderAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentAuditDto>> GetSalesOrderHistoryAsync(Guid id, CancellationToken ct = default);
    Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest req, CancellationToken ct = default);
    Task<SalesOrderDto> AddSalesOrderLineAsync(Guid orderId, AddSalesOrderLineRequest req, CancellationToken ct = default);
    Task<SalesOrderDto> UpdateSalesOrderLineAsync(Guid orderId, Guid lineId, UpdateSalesOrderLineRequest req, CancellationToken ct = default);
    Task RemoveSalesOrderLineAsync(Guid orderId, Guid lineId, CancellationToken ct = default);
    Task<SalesOrderDto> ApplyDiscountToOrderAsync(Guid orderId, decimal discountPct, CancellationToken ct = default);
    Task ConfirmSalesOrderAsync(Guid id, ConfirmSalesOrderRequest req, CancellationToken ct = default);
    Task StartPickingAsync(Guid id, CancellationToken ct = default);
    Task ShipSalesOrderAsync(Guid id, ShipOrderRequest req, CancellationToken ct = default);
    Task CancelSalesOrderAsync(Guid id, CancellationToken ct = default);

    // AR Invoices
    Task<IEnumerable<ARInvoiceDto>> GetInvoicesAsync(Guid? customerId = null, CancellationToken ct = default);
    Task<ARInvoiceDto> CreateInvoiceAsync(CreateARInvoiceRequest req, CancellationToken ct = default);
    Task<ARInvoiceDto> GenerateInvoiceFromOrderAsync(Guid salesOrderId, CancellationToken ct = default);
    Task IssueInvoiceAsync(Guid id, CancellationToken ct = default);
    Task VoidInvoiceAsync(Guid id, CancellationToken ct = default);

    // AR Payments
    Task<IEnumerable<ARPaymentDto>> GetPaymentsAsync(Guid? customerId = null, CancellationToken ct = default);
    Task<ARPaymentDto> CreatePaymentAsync(CreateARPaymentRequest req, CancellationToken ct = default);

    // Customer Addresses
    Task<IEnumerable<CustomerAddressDto>> GetCustomerAddressesAsync(Guid customerId, CancellationToken ct = default);
    Task<CustomerAddressDto> SaveCustomerAddressAsync(Guid customerId, Guid? addressId, SaveCustomerAddressRequest req, CancellationToken ct = default);
    Task DeleteCustomerAddressAsync(Guid customerId, Guid addressId, CancellationToken ct = default);
    Task SetPrimaryCustomerAddressAsync(Guid customerId, Guid addressId, CancellationToken ct = default);

    // Customer Contacts
    Task<IEnumerable<CustomerContactDto>> GetCustomerContactsAsync(Guid customerId, CancellationToken ct = default);
    Task<CustomerContactDto> SaveCustomerContactAsync(Guid customerId, Guid? contactId, SaveCustomerContactRequest req, CancellationToken ct = default);
    Task DeleteCustomerContactAsync(Guid customerId, Guid contactId, CancellationToken ct = default);
    Task SetPrimaryCustomerContactAsync(Guid customerId, Guid contactId, CancellationToken ct = default);

    // Reports
    Task<IEnumerable<ARAgingDto>> GetAgingReportAsync(CancellationToken ct = default);

    // ── Sales Quotations ──────────────────────────────────────────────────────
    Task<IEnumerable<QuotationSummaryDto>> GetQuotationsAsync(string? status = null, Guid? customerId = null, CancellationToken ct = default);
    Task<QuotationDto?> GetQuotationAsync(Guid id, CancellationToken ct = default);
    Task<QuotationDto> CreateQuotationAsync(CreateQuotationRequest req, CancellationToken ct = default);
    Task<QuotationDto> AddQuotationLineAsync(Guid quotationId, AddQuotationLineRequest req, CancellationToken ct = default);
    Task RemoveQuotationLineAsync(Guid quotationId, Guid lineId, CancellationToken ct = default);
    Task<QuotationDto> SubmitQuotationForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default);
    Task<QuotationDto> ApproveQuotationAsync(Guid id, CancellationToken ct = default);
    Task<QuotationDto> RejectQuotationAsync(Guid id, string reason, CancellationToken ct = default);
    Task<QuotationDto> SendQuotationAsync(Guid id, CancellationToken ct = default);
    Task<QuotationDto> AcceptQuotationAsync(Guid id, CancellationToken ct = default);
    Task<QuotationDto> RejectByCustomerAsync(Guid id, string? reason, CancellationToken ct = default);
    Task<SalesOrderDto> ConvertQuotationToSOAsync(Guid quotationId, ConvertQuotationToSORequest req, CancellationToken ct = default);
    Task CancelQuotationAsync(Guid id, CancellationToken ct = default);

    // ── SO Workflow ───────────────────────────────────────────────────────────
    Task<SalesOrderDto> SubmitSOForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default);
    Task<SalesOrderDto> ApproveSOAsync(Guid id, CancellationToken ct = default);
    Task<SalesOrderDto> RejectSOAsync(Guid id, string reason, CancellationToken ct = default);
    Task<SalesOrderDto> ConfirmDeliveryAsync(Guid id, ConfirmDeliveryRequest req, CancellationToken ct = default);

    // ── AR Invoice Workflow ───────────────────────────────────────────────────
    Task<ARInvoiceDto> SubmitARInvoiceForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default);
    Task<ARInvoiceDto> ApproveARInvoiceAsync(Guid id, CancellationToken ct = default);
    Task<ARInvoiceDto> RejectARInvoiceAsync(Guid id, CancellationToken ct = default);

    // ── AR Credit Notes ───────────────────────────────────────────────────────
    Task<IEnumerable<ARCreditNoteSummaryDto>> GetARCreditNotesAsync(Guid? customerId = null, CancellationToken ct = default);
    Task<ARCreditNoteDto?> GetARCreditNoteAsync(Guid id, CancellationToken ct = default);
    Task<ARCreditNoteDto> CreateARCreditNoteAsync(CreateARCreditNoteRequest req, CancellationToken ct = default);
    Task<ARCreditNoteDto> SubmitCreditNoteForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default);
    Task<ARCreditNoteDto> ApproveCreditNoteAsync(Guid id, CancellationToken ct = default);
    Task<ARCreditNoteDto> RejectCreditNoteAsync(Guid id, CancellationToken ct = default);
    Task<ARCreditNoteDto> IssueCreditNoteAsync(Guid id, CancellationToken ct = default);
    Task<ARCreditNoteDto> ApplyCreditToInvoiceAsync(Guid id, ApplyCreditNoteRequest req, CancellationToken ct = default);
    Task<ARCreditNoteDto> VoidCreditNoteAsync(Guid id, CancellationToken ct = default);

    // ── Dunning / Collections ─────────────────────────────────────────────────
    Task<IEnumerable<DunningRecordDto>> GetDunningRecordsAsync(Guid? customerId = null, CancellationToken ct = default);
    Task<DunningRecordDto> CreateDunningAsync(CreateDunningRequest req, CancellationToken ct = default);
    Task<DunningRecordDto> ResolveDunningAsync(Guid id, string? notes, CancellationToken ct = default);
    Task<DunningRecordDto> EscalateDunningAsync(Guid id, CancellationToken ct = default);
}

public class AccountsReceivableService : IAccountsReceivableService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;
    private readonly IDocumentAuditService _audit;

    public AccountsReceivableService(
        IAppDbContext db,
        ICurrentOrganizationService org,
        IDocumentAuditService audit)
    {
        _db = db;
        _org = org;
        _audit = audit;
    }

    // ── Customers ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken ct = default)
    {
        var customers = await _db.Customers.Where(c => !c.IsDeleted).OrderBy(c => c.CustomerNumber).ToListAsync(ct);
        // load outstanding balances in one query
        var customerIds = customers.Select(c => c.Id).ToList();
        var balances = await _db.ARInvoices
            .Where(i => customerIds.Contains(i.CustomerId) && !i.IsDeleted &&
                        i.Status != ARInvoiceStatus.FullyPaid && i.Status != ARInvoiceStatus.Voided)
            .GroupBy(i => i.CustomerId)
            .Select(g => new { CustomerId = g.Key, Outstanding = g.Sum(i => i.TotalAmount - i.PaidAmount) })
            .ToDictionaryAsync(x => x.CustomerId, x => x.Outstanding, ct);
        return customers.Select(c => ToCustomerDto(c, balances.GetValueOrDefault(c.Id, 0)));
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest req, CancellationToken ct = default)
    {
        var count = await _db.Customers.CountAsync(ct) + 1;
        var customer = new Customer(_org.OrganizationId, $"CUST-{count:D5}", req.Name, req.Email, req.Phone,
            req.BillingAddress, req.Currency, req.PaymentTermsDays, req.CreditLimit,
            req.BillingAddress, req.ShippingAddress, req.Website, req.Notes);
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);
        return ToCustomerDto(customer, 0);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequest req, CancellationToken ct = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Customer not found.");
        customer.Update(req.Name, req.Email, req.Phone, req.BillingAddress, req.ShippingAddress,
            req.PaymentTermsDays, req.CreditLimit, req.Website, req.Notes);
        await _db.SaveChangesAsync(ct);
        var outstanding = await _db.ARInvoices
            .Where(i => i.CustomerId == id && !i.IsDeleted &&
                        i.Status != ARInvoiceStatus.FullyPaid && i.Status != ARInvoiceStatus.Voided)
            .SumAsync(i => i.TotalAmount - i.PaidAmount, ct);
        return ToCustomerDto(customer, outstanding);
    }

    public async Task<CustomerLedgerDto> GetCustomerLedgerAsync(Guid customerId, CancellationToken ct = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Customer not found.");

        var invoices = await _db.ARInvoices
            .Include(i => i.SalesOrder)
            .Where(i => i.CustomerId == customerId && !i.IsDeleted)
            .OrderBy(i => i.InvoiceDate)
            .ToListAsync(ct);

        var payments = await _db.ARPayments
            .Include(p => p.ARInvoice)
            .Where(p => p.CustomerId == customerId && !p.IsDeleted)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync(ct);

        // Build chronological ledger entries
        var entries = new List<(DateTime Date, CustomerLedgerEntryDto Dto)>();

        foreach (var inv in invoices)
        {
            entries.Add((inv.InvoiceDate, new CustomerLedgerEntryDto(
                "Invoice", inv.InvoiceNumber, inv.InvoiceDate,
                inv.TotalAmount, 0, 0,   // RunningBalance filled below
                inv.Status.ToString(), inv.SalesOrder?.OrderNumber)));
        }
        foreach (var pay in payments)
        {
            entries.Add((pay.PaymentDate, new CustomerLedgerEntryDto(
                "Payment", pay.PaymentNumber, pay.PaymentDate,
                0, pay.Amount, 0,
                pay.Status.ToString(), null)));
        }

        entries.Sort((a, b) => a.Date.CompareTo(b.Date));

        decimal running = 0;
        var finalEntries = entries.Select(e =>
        {
            running += e.Dto.Debit - e.Dto.Credit;
            return e.Dto with { RunningBalance = running };
        }).ToList();

        var totalInvoiced = invoices.Sum(i => i.TotalAmount);
        var totalPaid     = payments.Sum(p => p.Amount);

        return new CustomerLedgerDto(
            customerId, customer.Name, customer.CustomerNumber,
            totalInvoiced, totalPaid, totalInvoiced - totalPaid,
            finalEntries);
    }

    // ── Sales Orders ──────────────────────────────────────────────────────────

    public async Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersAsync(
        string? status = null, Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .Where(o => !o.IsDeleted);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<SalesOrderStatus>(status, out var s))
            query = query.Where(o => o.Status == s);
        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync(ct);
        return orders.Select(o => new SalesOrderSummaryDto(
            o.Id, o.OrderNumber, o.CustomerId, o.Customer?.Name ?? string.Empty,
            o.OrderDate, o.RequestedShipDate, o.CustomerRef,
            o.Status.ToString(), o.GrandTotal, o.Lines.Count, o.CreatedAt,
            o.IsExported, o.ExportedAt, o.WorkflowInstanceId, o.RejectionReason));
    }

    public async Task<SalesOrderDto?> GetSalesOrderAsync(Guid id, CancellationToken ct = default)
    {
        var o = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct);
        return o is null ? null : ToSalesOrderDto(o);
    }

    public Task<IReadOnlyList<DocumentAuditDto>> GetSalesOrderHistoryAsync(
        Guid id, CancellationToken ct = default)
        => _audit.GetAsync("SalesOrder", id, ct);

    public async Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest req, CancellationToken ct = default)
    {
        var count = await _db.SalesOrders.CountAsync(ct) + 1;
        var order = new SalesOrder(_org.OrganizationId, $"SO-{req.OrderDate:yyyy}-{count:D5}",
            req.CustomerId, req.OrderDate, req.Description,
            req.CustomerRef, req.Currency, req.RequestedShipDate);
        _db.SalesOrders.Add(order);
        _audit.Add("AR", "Created", order.Id, "SalesOrder", null, new
        {
            order.OrderNumber,
            order.CustomerId,
            order.OrderDate,
            order.RequestedShipDate,
            order.Currency
        });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(order.Id, ct))!;
    }

    public async Task<SalesOrderDto> AddSalesOrderLineAsync(Guid orderId, AddSalesOrderLineRequest req, CancellationToken ct = default)
    {
        // Load with tracking so totals are updated on the order.
        // Load WITHOUT Include to avoid query-filter interference on the lines collection.
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        var variant = await _db.ProductVariants
            .IgnoreQueryFilters()
            .Include(v => v.Product).ThenInclude(p => p!.Category)
            .FirstOrDefaultAsync(v => v.Id == req.ProductVariantId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Product variant not found.");

        var product = variant.Product
            ?? throw new InvalidOperationException("Product not found for variant.");
        var unitPrice = req.OverrideUnitPrice ?? variant.EffectivePrice(product.BasePrice);
        var effectiveTaxRate = product.EffectiveTaxRate(product.Category?.TaxRate ?? 0m);
        var variantDesc = string.Join(", ", new[] { variant.Size, variant.Color, variant.Material }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

        // Domain method validates status, updates order totals, and returns the new line.
        // Explicitly Add() the line to the DbSet so EF tracks it in Added state —
        // avoids concurrency exceptions from collection-change-tracking with query filters.
        var line = order.AddLine(variant.Id, variant.Sku, product.Name,
            string.IsNullOrEmpty(variantDesc) ? null : variantDesc,
            product.UnitOfMeasure, req.Quantity, unitPrice, effectiveTaxRate, req.DiscountPct);
        _db.SalesOrderLines.Add(line);
        var activeLines = await _db.SalesOrderLines
            .Where(l => l.SalesOrderId == orderId && !l.IsDeleted)
            .ToListAsync(ct);
        activeLines.Add(line);
        var rounding = await GetMoneyRoundingAsync(ct);
        order.RecalcTotalsFromLines(activeLines, rounding.DecimalPlaces, rounding.Method, rounding.Level);
        _audit.Add("AR", "Line Added", order.Id, "SalesOrder", null, new
        {
            LineId = line.Id,
            line.Sku,
            line.ProductName,
            line.Quantity,
            line.UnitPrice,
            line.DiscountPct
        });
        await _db.SaveChangesAsync(ct);

        return (await GetSalesOrderAsync(orderId, ct))!;
    }

    public async Task<SalesOrderDto> UpdateSalesOrderLineAsync(
        Guid orderId, Guid lineId, UpdateSalesOrderLineRequest req, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        if (order.Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be edited on a Draft order.");

        var line = await _db.SalesOrderLines
            .FirstOrDefaultAsync(l => l.Id == lineId && l.SalesOrderId == orderId && !l.IsDeleted, ct)
            ?? throw new InvalidOperationException("Line not found.");

        var oldLine = new { line.Quantity, line.UnitPrice, line.DiscountPct };
        line.Update(req.Quantity, req.UnitPrice ?? line.UnitPrice, req.DiscountPct ?? line.DiscountPct);

        var lines = await _db.SalesOrderLines
            .Where(l => l.SalesOrderId == orderId && !l.IsDeleted)
            .ToListAsync(ct);
        var rounding = await GetMoneyRoundingAsync(ct);
        order.RecalcTotalsFromLines(lines, rounding.DecimalPlaces, rounding.Method, rounding.Level);

        _audit.Add("AR", "Line Updated", order.Id, "SalesOrder", oldLine, new
        {
            LineId = line.Id,
            line.Sku,
            line.Quantity,
            line.UnitPrice,
            line.DiscountPct
        });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(orderId, ct))!;
    }

    public async Task RemoveSalesOrderLineAsync(Guid orderId, Guid lineId, CancellationToken ct = default)
    {
        // Load tracked (no Include) so we can update order totals.
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        order.ValidateCanRemoveLine();

        // Soft-delete the line directly from its DbSet — avoids collection-tracking issues.
        var line = await _db.SalesOrderLines
            .FirstOrDefaultAsync(l => l.Id == lineId && l.SalesOrderId == orderId && !l.IsDeleted, ct)
            ?? throw new InvalidOperationException("Line not found.");
        line.SoftDelete();

        // Recalc order totals from remaining active lines.
        var remaining = await _db.SalesOrderLines
            .Where(l => l.SalesOrderId == orderId && !l.IsDeleted && l.Id != lineId)
            .ToListAsync(ct);
        var rounding = await GetMoneyRoundingAsync(ct);
        order.RecalcTotalsFromLines(remaining, rounding.DecimalPlaces, rounding.Method, rounding.Level);

        _audit.Add("AR", "Line Removed", order.Id, "SalesOrder", new
        {
            LineId = line.Id,
            line.Sku,
            line.ProductName,
            line.Quantity,
            line.UnitPrice
        });
        await _db.SaveChangesAsync(ct);
    }

    public async Task<SalesOrderDto> ApplyDiscountToOrderAsync(Guid orderId, decimal discountPct, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        if (order.Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Discount can only be applied to Draft orders.");

        if (discountPct < 0 || discountPct > 100)
            throw new InvalidOperationException("Discount percentage must be between 0 and 100.");

        // Apply the discount to every active line using the existing Update() method.
        var lines = await _db.SalesOrderLines
            .Where(l => l.SalesOrderId == orderId && !l.IsDeleted)
            .ToListAsync(ct);
        var oldDiscounts = lines.Select(l => new { LineId = l.Id, l.DiscountPct }).ToList();

        foreach (var line in lines)
            line.Update(line.Quantity, line.UnitPrice, discountPct);

        // Recalculate order-level totals from the updated lines.
        var rounding = await GetMoneyRoundingAsync(ct);
        order.RecalcTotalsFromLines(lines, rounding.DecimalPlaces, rounding.Method, rounding.Level);

        _audit.Add("AR", "Discount Applied", order.Id, "SalesOrder", oldDiscounts, new { DiscountPct = discountPct });
        await _db.SaveChangesAsync(ct);

        return (await GetSalesOrderAsync(orderId, ct))!;
    }

    public async Task ConfirmSalesOrderAsync(Guid id, ConfirmSalesOrderRequest req, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        var oldStatus = order.Status.ToString();
        order.Confirm();
        await ReserveSalesOrderInventoryAsync(order, req.BackorderLimit, ct);
        _audit.Add("AR", "Confirmed", order.Id, "SalesOrder",
            new { Status = oldStatus },
            new { Status = order.Status.ToString(), req.BackorderLimit });
        await _db.SaveChangesAsync(ct);
    }

    public async Task StartPickingAsync(Guid id, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        var oldStatus = order.Status.ToString();
        order.StartPicking();
        _audit.Add("AR", "Picking Started", order.Id, "SalesOrder",
            new { Status = oldStatus }, new { Status = order.Status.ToString() });
        await _db.SaveChangesAsync(ct);
    }

    public async Task ShipSalesOrderAsync(Guid id, ShipOrderRequest req, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        var oldStatus = order.Status.ToString();
        var fullyShipped = await ShipSalesOrderInventoryAsync(order, req, ct);
        order.Ship(req.ShipDate ?? DateTime.UtcNow, fullyShipped);
        _audit.Add("AR", fullyShipped ? "Shipped" : "Partially Shipped", order.Id, "SalesOrder",
            new { Status = oldStatus },
            new
            {
                Status = order.Status.ToString(),
                ShipDate = req.ShipDate ?? DateTime.UtcNow,
                Lines = (req.Lines ?? []).Select(l => new { l.LineId, l.Quantity }).ToList()
            });
        await _db.SaveChangesAsync(ct);
    }

    public async Task CancelSalesOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        var oldStatus = order.Status.ToString();
        var shouldReleaseInventory = order.Status is SalesOrderStatus.Confirmed or SalesOrderStatus.Picking;
        order.Cancel();
        if (shouldReleaseInventory)
            await ReleaseSalesOrderInventoryAsync(order, ct);
        _audit.Add("AR", "Cancelled", order.Id, "SalesOrder",
            new { Status = oldStatus }, new { Status = order.Status.ToString() });
        await _db.SaveChangesAsync(ct);
    }

    // ── AR Invoices ───────────────────────────────────────────────────────────

    public async Task<IEnumerable<ARInvoiceDto>> GetInvoicesAsync(Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.ARInvoices
            .Include(i => i.Customer)
            .Include(i => i.SalesOrder)
            .Where(i => !i.IsDeleted);
        if (customerId.HasValue) query = query.Where(i => i.CustomerId == customerId.Value);
        var list = await query.OrderByDescending(i => i.InvoiceDate).ToListAsync(ct);
        return list.Select(ToARInvoiceDto);
    }

    public async Task<ARInvoiceDto> CreateInvoiceAsync(CreateARInvoiceRequest req, CancellationToken ct = default)
    {
        var count = await _db.ARInvoices.CountAsync(ct) + 1;
        var inv = new ARInvoice(_org.OrganizationId, $"INV-{count:D6}", req.CustomerId,
            req.InvoiceDate, req.DueDate, req.Description,
            req.SubTotal, req.TaxAmount, req.DiscountAmount, req.SalesOrderId);
        _db.ARInvoices.Add(inv);
        await _db.SaveChangesAsync(ct);

        var created = await _db.ARInvoices
            .Include(i => i.Customer).Include(i => i.SalesOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToARInvoiceDto(created);
    }

    public async Task<ARInvoiceDto> GenerateInvoiceFromOrderAsync(Guid salesOrderId, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == salesOrderId && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        if (order.Status != SalesOrderStatus.Shipped)
            throw new InvalidOperationException("Invoice can only be generated for a Shipped order.");

        var customer = order.Customer!;
        var dueDate = DateTime.UtcNow.Date.AddDays(customer.PaymentTermsDays);
        var count = await _db.ARInvoices.CountAsync(ct) + 1;

        var inv = new ARInvoice(_org.OrganizationId, $"INV-{count:D6}", order.CustomerId,
            DateTime.UtcNow.Date, dueDate,
            $"Invoice for {order.OrderNumber}",
            order.SubTotal, order.TaxTotal, order.DiscountTotal, order.Id);

        _db.ARInvoices.Add(inv);
        order.Invoice(inv.Id);
        await _db.SaveChangesAsync(ct);

        var created = await _db.ARInvoices
            .Include(i => i.Customer).Include(i => i.SalesOrder)
            .FirstAsync(i => i.Id == inv.Id, ct);
        return ToARInvoiceDto(created);
    }

    public async Task IssueInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices
            .Include(i => i.SalesOrder)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        var journal = await CreateInvoiceJournalAsync(inv, ct);
        inv.Issue(journal.Id);
        await _db.SaveChangesAsync(ct);
    }

    public async Task VoidInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        if (inv.JournalEntryId.HasValue)
        {
            var journal = await _db.JournalEntries
                .Include(e => e.Lines)
                .FirstOrDefaultAsync(e => e.Id == inv.JournalEntryId.Value && !e.IsDeleted, ct)
                ?? throw new InvalidOperationException("Invoice journal entry not found.");
            journal.Void();
        }
        inv.Void();
        await _db.SaveChangesAsync(ct);
    }

    // ── AR Payments ───────────────────────────────────────────────────────────

    public async Task<IEnumerable<ARPaymentDto>> GetPaymentsAsync(Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.ARPayments
            .Include(p => p.Customer)
            .Include(p => p.ARInvoice)
            .Where(p => !p.IsDeleted);
        if (customerId.HasValue) query = query.Where(p => p.CustomerId == customerId.Value);
        var list = await query.OrderByDescending(p => p.PaymentDate).ToListAsync(ct);
        return list.Select(p => new ARPaymentDto(
            p.Id, p.PaymentNumber, p.CustomerId, p.Customer?.Name ?? string.Empty,
            p.ARInvoiceId, p.ARInvoice?.InvoiceNumber ?? string.Empty,
            p.PaymentDate, p.Amount, p.PaymentMethod.ToString(),
            p.Reference, p.Status.ToString(), p.CreatedAt));
    }

    public async Task<ARPaymentDto> CreatePaymentAsync(CreateARPaymentRequest req, CancellationToken ct = default)
    {
        var invoice = await _db.ARInvoices
            .Include(i => i.Customer)
            .Include(i => i.SalesOrder)
            .FirstOrDefaultAsync(i => i.Id == req.ARInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var count = await _db.ARPayments.CountAsync(ct) + 1;
        if (!Enum.TryParse<PaymentMethod>(req.PaymentMethod, out var method))
            method = PaymentMethod.BankTransfer;

        var payment = new ARPayment(_org.OrganizationId, $"RCPT-{count:D6}", req.CustomerId, req.ARInvoiceId,
            req.PaymentDate, req.Amount, method, req.Reference);

        var journal = await CreatePaymentJournalAsync(payment, invoice, ct);
        invoice.ApplyPayment(req.Amount);
        _db.ARPayments.Add(payment);
        payment.Post(journal.Id);
        await _db.SaveChangesAsync(ct);

        var created = await _db.ARPayments
            .Include(p => p.Customer).Include(p => p.ARInvoice)
            .FirstAsync(p => p.Id == payment.Id, ct);
        return new ARPaymentDto(
            created.Id, created.PaymentNumber, created.CustomerId,
            created.Customer?.Name ?? string.Empty,
            created.ARInvoiceId, created.ARInvoice?.InvoiceNumber ?? string.Empty,
            created.PaymentDate, created.Amount, created.PaymentMethod.ToString(),
            created.Reference, created.Status.ToString(), created.CreatedAt);
    }

    // ── Customer Addresses ────────────────────────────────────────────────────

    public async Task<IEnumerable<CustomerAddressDto>> GetCustomerAddressesAsync(Guid customerId, CancellationToken ct = default)
    {
        var addresses = await _db.CustomerAddresses
            .Where(a => a.CustomerId == customerId && !a.IsDeleted)
            .OrderByDescending(a => a.IsPrimary).ThenBy(a => a.Label)
            .ToListAsync(ct);
        return addresses.Select(ToCustomerAddressDto);
    }

    public async Task<CustomerAddressDto> SaveCustomerAddressAsync(Guid customerId, Guid? addressId, SaveCustomerAddressRequest req, CancellationToken ct = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Customer not found.");

        if (!Enum.TryParse<AddressType>(req.AddressType, out var addrType))
            addrType = AddressType.Billing;

        CustomerAddress address;
        if (addressId.HasValue)
        {
            address = await _db.CustomerAddresses
                .FirstOrDefaultAsync(a => a.Id == addressId.Value && a.CustomerId == customerId && !a.IsDeleted, ct)
                ?? throw new InvalidOperationException("Address not found.");
            address.Update(req.Label, addrType, req.Line1, req.Line2, req.City, req.State, req.PostalCode, req.Country ?? "US");
        }
        else
        {
            // First address is automatically primary
            var isFirst = !await _db.CustomerAddresses.AnyAsync(a => a.CustomerId == customerId && !a.IsDeleted, ct);
            address = new CustomerAddress(_org.OrganizationId, customerId, req.Label, addrType,
                req.Line1, req.Line2, req.City, req.State, req.PostalCode, req.Country ?? "US", isFirst);
            _db.CustomerAddresses.Add(address);
        }
        await _db.SaveChangesAsync(ct);
        return ToCustomerAddressDto(address);
    }

    public async Task DeleteCustomerAddressAsync(Guid customerId, Guid addressId, CancellationToken ct = default)
    {
        var address = await _db.CustomerAddresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.CustomerId == customerId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Address not found.");
        address.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetPrimaryCustomerAddressAsync(Guid customerId, Guid addressId, CancellationToken ct = default)
    {
        var all = await _db.CustomerAddresses
            .Where(a => a.CustomerId == customerId && !a.IsDeleted)
            .ToListAsync(ct);
        foreach (var a in all) a.ClearPrimary();
        var target = all.FirstOrDefault(a => a.Id == addressId)
            ?? throw new InvalidOperationException("Address not found.");
        target.SetPrimary();
        await _db.SaveChangesAsync(ct);
    }

    // ── Customer Contacts ─────────────────────────────────────────────────────

    public async Task<IEnumerable<CustomerContactDto>> GetCustomerContactsAsync(Guid customerId, CancellationToken ct = default)
    {
        var contacts = await _db.CustomerContacts
            .Where(c => c.CustomerId == customerId && !c.IsDeleted)
            .OrderByDescending(c => c.IsPrimary).ThenBy(c => c.Name)
            .ToListAsync(ct);
        return contacts.Select(ToCustomerContactDto);
    }

    public async Task<CustomerContactDto> SaveCustomerContactAsync(Guid customerId, Guid? contactId, SaveCustomerContactRequest req, CancellationToken ct = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Customer not found.");

        CustomerContact contact;
        if (contactId.HasValue)
        {
            contact = await _db.CustomerContacts
                .FirstOrDefaultAsync(c => c.Id == contactId.Value && c.CustomerId == customerId && !c.IsDeleted, ct)
                ?? throw new InvalidOperationException("Contact not found.");
            contact.Update(req.Name, req.Title, req.Email, req.Phone, req.Mobile, req.Notes);
        }
        else
        {
            var isFirst = !await _db.CustomerContacts.AnyAsync(c => c.CustomerId == customerId && !c.IsDeleted, ct);
            contact = new CustomerContact(_org.OrganizationId, customerId, req.Name, req.Title, req.Email, req.Phone, req.Mobile, req.Notes, isFirst);
            _db.CustomerContacts.Add(contact);
        }
        await _db.SaveChangesAsync(ct);
        return ToCustomerContactDto(contact);
    }

    public async Task DeleteCustomerContactAsync(Guid customerId, Guid contactId, CancellationToken ct = default)
    {
        var contact = await _db.CustomerContacts
            .FirstOrDefaultAsync(c => c.Id == contactId && c.CustomerId == customerId && !c.IsDeleted, ct)
            ?? throw new InvalidOperationException("Contact not found.");
        contact.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetPrimaryCustomerContactAsync(Guid customerId, Guid contactId, CancellationToken ct = default)
    {
        var all = await _db.CustomerContacts
            .Where(c => c.CustomerId == customerId && !c.IsDeleted)
            .ToListAsync(ct);
        foreach (var c in all) c.ClearPrimary();
        var target = all.FirstOrDefault(c => c.Id == contactId)
            ?? throw new InvalidOperationException("Contact not found.");
        target.SetPrimary();
        await _db.SaveChangesAsync(ct);
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ARAgingDto>> GetAgingReportAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var invoices = await _db.ARInvoices
            .Include(i => i.Customer)
            .Where(i => !i.IsDeleted && i.Status != ARInvoiceStatus.FullyPaid && i.Status != ARInvoiceStatus.Voided)
            .ToListAsync(ct);

        return invoices
            .GroupBy(i => i.CustomerId)
            .Select(g =>
            {
                var customer = g.First().Customer!;
                decimal current = 0, d1_30 = 0, d31_60 = 0, d61_90 = 0, over90 = 0;
                foreach (var inv in g)
                {
                    var outstanding = inv.TotalAmount - inv.PaidAmount;
                    var days = (today - inv.DueDate.Date).Days;
                    if (days <= 0)       current += outstanding;
                    else if (days <= 30) d1_30   += outstanding;
                    else if (days <= 60) d31_60  += outstanding;
                    else if (days <= 90) d61_90  += outstanding;
                    else                 over90  += outstanding;
                }
                return new ARAgingDto(customer.CustomerNumber, customer.Name,
                    current, d1_30, d31_60, d61_90, over90,
                    current + d1_30 + d31_60 + d61_90 + over90);
            })
            .OrderBy(a => a.CustomerName);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<(int DecimalPlaces, MoneyRoundingMethod Method, MoneyRoundingLevel Level)>
        GetMoneyRoundingAsync(CancellationToken ct)
    {
        var settings = await _db.Organizations
            .AsNoTracking()
            .Where(o => o.Id == _org.OrganizationId)
            .Select(o => new { o.MoneyDecimalPlaces, o.MoneyRoundingMethod, o.MoneyRoundingLevel })
            .FirstAsync(ct);
        return (settings.MoneyDecimalPlaces, settings.MoneyRoundingMethod, settings.MoneyRoundingLevel);
    }

    private async Task<SalesOrder> LoadOrderWithLines(Guid id, CancellationToken ct)
    {
        return await _db.SalesOrders
            .Include(o => o.Lines)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");
    }

    private async Task ReserveSalesOrderInventoryAsync(
        SalesOrder order, decimal backorderLimit, CancellationToken ct)
    {
        var records = await LoadInventoryRecordsForOrderAsync(order, ct);

        foreach (var lineGroup in ActiveOrderLineQuantities(order))
        {
            var record = records[lineGroup.ProductVariantId];
            record.Reserve(lineGroup.Quantity, backorderLimit);
            AddInventoryTransaction(order, record, InventoryTransactionType.SaleCommit,
                lineGroup.Quantity, $"Reserved for sales order {order.OrderNumber}.");
        }
    }

    private async Task ReleaseSalesOrderInventoryAsync(SalesOrder order, CancellationToken ct)
    {
        var records = await LoadInventoryRecordsForOrderAsync(order, ct);

        foreach (var lineGroup in ActiveOrderLineQuantities(order))
        {
            var record = records[lineGroup.ProductVariantId];
            record.Unreserve(lineGroup.Quantity);
            AddInventoryTransaction(order, record, InventoryTransactionType.SaleUncommit,
                -lineGroup.Quantity, $"Released reservation for sales order {order.OrderNumber}.");
        }
    }

    private async Task<bool> ShipSalesOrderInventoryAsync(
        SalesOrder order, ShipOrderRequest req, CancellationToken ct)
    {
        var records = await LoadInventoryRecordsForOrderAsync(order, ct);
        var activeLines = order.Lines.Where(l => !l.IsDeleted).ToDictionary(l => l.Id);
        decimal shipmentCost = 0m;
        var requestedLines = req.Lines?.ToList() ??
            activeLines.Values
                .Where(l => l.QuantityShipped < l.Quantity)
                .Select(l => new ShipOrderLineRequest(l.Id, l.Quantity - l.QuantityShipped))
                .ToList();

        if (requestedLines.Count == 0)
            throw new InvalidOperationException("At least one shipment line is required.");
        if (requestedLines.GroupBy(l => l.LineId).Any(g => g.Count() > 1))
            throw new InvalidOperationException("A sales order line can only appear once in a shipment.");

        foreach (var requestedLine in requestedLines)
        {
            if (!activeLines.TryGetValue(requestedLine.LineId, out var orderLine))
                throw new InvalidOperationException("Shipment contains a line that is not on this sales order.");

            orderLine.Ship(requestedLine.Quantity);
            var record = records[orderLine.ProductVariantId];
            shipmentCost += requestedLine.Quantity * record.AverageCost;
            record.IssueStock(requestedLine.Quantity);
            record.Unreserve(requestedLine.Quantity);
            AddInventoryTransaction(order, record, InventoryTransactionType.SaleShipment,
                -requestedLine.Quantity, $"Shipped sales order {order.OrderNumber}, line {orderLine.Sku}.");
        }

        if (shipmentCost > 0)
            await CreateShipmentJournalAsync(order, req.ShipDate ?? DateTime.UtcNow, shipmentCost, ct);

        return activeLines.Values.All(l => l.QuantityShipped >= l.Quantity);
    }

    private async Task<Dictionary<Guid, InventoryRecord>> LoadInventoryRecordsForOrderAsync(
        SalesOrder order, CancellationToken ct)
    {
        var lineQuantities = ActiveOrderLineQuantities(order).ToList();
        var variantIds = lineQuantities.Select(l => l.ProductVariantId).ToList();

        var records = await _db.InventoryRecords
            .Where(r => variantIds.Contains(r.ProductVariantId))
            .ToDictionaryAsync(r => r.ProductVariantId, ct);

        var missingSku = order.Lines
            .Where(l => !l.IsDeleted && !records.ContainsKey(l.ProductVariantId))
            .Select(l => l.Sku)
            .FirstOrDefault();
        if (missingSku is not null)
            throw new InvalidOperationException($"Inventory record not found for SKU {missingSku}.");

        return records;
    }

    private static IEnumerable<(Guid ProductVariantId, decimal Quantity)> ActiveOrderLineQuantities(SalesOrder order) =>
        order.Lines
            .Where(l => !l.IsDeleted)
            .GroupBy(l => l.ProductVariantId)
            .Select(g => (ProductVariantId: g.Key, Quantity: g.Sum(l => l.Quantity)));

    private void AddInventoryTransaction(
        SalesOrder order,
        InventoryRecord record,
        InventoryTransactionType transactionType,
        decimal quantity,
        string notes)
    {
        _db.InventoryTransactions.Add(new InventoryTransaction(
            record.OrganizationId,
            record.ProductVariantId,
            transactionType,
            quantity,
            record.AverageCost,
            record.QuantityOnHand,
            referenceNumber: order.OrderNumber,
            referenceDocumentId: order.Id,
            notes: notes));
    }

    private async Task<JournalEntry> CreateInvoiceJournalAsync(ARInvoice invoice, CancellationToken ct)
    {
        if (invoice.JournalEntryId.HasValue)
            throw new InvalidOperationException("This invoice has already been posted to the general ledger.");

        var accounts = await LoadPostingAccountsAsync(new[] { "1210", "4100", "2210" }, ct);
        var journal = await CreateJournalAsync(
            invoice.InvoiceDate,
            $"Customer invoice {invoice.InvoiceNumber}",
            invoice.InvoiceNumber,
            "Sales",
            invoice.OrganizationId,
            invoice.SalesOrder?.Currency ?? "USD",
            ct);

        journal.AddLine(accounts["1210"].Id, $"Trade receivable - {invoice.InvoiceNumber}",
            invoice.TotalAmount, 0m);
        journal.AddLine(accounts["4100"].Id, $"Sales revenue - {invoice.InvoiceNumber}",
            0m, invoice.SubTotal - invoice.DiscountAmount);
        if (invoice.TaxAmount > 0)
            journal.AddLine(accounts["2210"].Id, $"Sales tax payable - {invoice.InvoiceNumber}",
                0m, invoice.TaxAmount);
        journal.Post();
        return journal;
    }

    private async Task<JournalEntry> CreatePaymentJournalAsync(
        ARPayment payment, ARInvoice invoice, CancellationToken ct)
    {
        var cashAccountNumber = payment.PaymentMethod == PaymentMethod.Cash ? "1110" : "1120";
        var accounts = await LoadPostingAccountsAsync(new[] { cashAccountNumber, "1210" }, ct);
        var journal = await CreateJournalAsync(
            payment.PaymentDate,
            $"Customer receipt {payment.PaymentNumber}",
            payment.Reference ?? payment.PaymentNumber,
            "CashReceipt",
            payment.OrganizationId,
            invoice.SalesOrder?.Currency ?? "USD",
            ct);

        journal.AddLine(accounts[cashAccountNumber].Id, $"Receipt for {invoice.InvoiceNumber}",
            payment.Amount, 0m);
        journal.AddLine(accounts["1210"].Id, $"Settle receivable - {invoice.InvoiceNumber}",
            0m, payment.Amount);
        journal.Post();
        return journal;
    }

    private async Task CreateShipmentJournalAsync(
        SalesOrder order, DateTime shipDate, decimal shipmentCost, CancellationToken ct)
    {
        var accounts = await LoadPostingAccountsAsync(new[] { "5100", "1310" }, ct);
        var journal = await CreateJournalAsync(
            shipDate,
            $"Cost of goods sold for {order.OrderNumber}",
            order.OrderNumber,
            "Inventory",
            order.OrganizationId,
            order.Currency,
            ct);

        journal.AddLine(accounts["5100"].Id, $"COGS - {order.OrderNumber}", shipmentCost, 0m);
        journal.AddLine(accounts["1310"].Id, $"Inventory issued - {order.OrderNumber}", 0m, shipmentCost);
        journal.Post();
    }

    private async Task<JournalEntry> CreateJournalAsync(
        DateTime entryDate,
        string description,
        string reference,
        string journalType,
        Guid organizationId,
        string currency,
        CancellationToken ct)
    {
        var date = entryDate.Date;
        var period = await _db.FiscalPeriods
            .Include(p => p.FiscalYear)
                .ThenInclude(y => y!.FiscalCalendar)
            .FirstOrDefaultAsync(p =>
                p.FiscalYear!.OrganizationId == organizationId &&
                p.FiscalYear.FiscalCalendar!.IsDefault &&
                p.FiscalYear.Status == FiscalYearStatus.Open &&
                p.Status == FiscalPeriodStatus.Open &&
                p.StartDate.Date <= date &&
                p.EndDate.Date >= date, ct)
            ?? throw new InvalidOperationException($"No open fiscal period exists for {date:d}.");

        var count = await _db.JournalEntries.CountAsync(ct) + 1;
        var journal = new JournalEntry(
            organizationId,
            $"JE-{count:D6}",
            date,
            period.Id,
            description,
            reference,
            journalType,
            currency);
        _db.JournalEntries.Add(journal);
        return journal;
    }

    private async Task<Dictionary<string, Account>> LoadPostingAccountsAsync(
        IEnumerable<string> accountNumbers, CancellationToken ct)
    {
        var numbers = accountNumbers.Distinct().ToList();
        var accounts = await _db.Accounts
            .Where(a =>
                a.OrganizationId == _org.OrganizationId &&
                numbers.Contains(a.AccountNumber) &&
                !a.IsHeaderAccount &&
                a.Status == AccountStatus.Active &&
                !a.IsDeleted)
            .ToDictionaryAsync(a => a.AccountNumber, ct);

        var missing = numbers.Where(n => !accounts.ContainsKey(n)).ToList();
        if (missing.Count > 0)
            throw new InvalidOperationException(
                $"Required posting account(s) not found or inactive: {string.Join(", ", missing)}.");

        return accounts;
    }

    private static CustomerDto ToCustomerDto(Customer c, decimal outstanding) =>
        new(c.Id, c.CustomerNumber, c.Name, c.Email, c.Phone,
            c.BillingAddress, c.ShippingAddress, c.Website, c.Notes,
            c.Currency, c.PaymentTermsDays, c.CreditLimit,
            outstanding, outstanding, c.CreditLimit - outstanding,
            c.Status.ToString(), c.CreatedAt);

    private static CustomerAddressDto ToCustomerAddressDto(CustomerAddress a) =>
        new(a.Id, a.Label, a.AddressType.ToString(), a.IsPrimary,
            a.Line1, a.Line2, a.City, a.State, a.PostalCode, a.Country, a.SingleLine);

    private static CustomerContactDto ToCustomerContactDto(CustomerContact c) =>
        new(c.Id, c.Name, c.Title, c.Email, c.Phone, c.Mobile, c.IsPrimary, c.Notes);

    private static ARInvoiceDto ToARInvoiceDto(ARInvoice i) =>
        new(i.Id, i.InvoiceNumber, i.CustomerId, i.Customer?.Name ?? string.Empty,
            i.SalesOrderId, i.SalesOrder?.OrderNumber,
            i.InvoiceDate, i.DueDate, i.Description,
            i.SubTotal, i.TaxAmount, i.DiscountAmount, i.TotalAmount,
            i.PaidAmount, i.OutstandingAmount, i.Status.ToString(),
            i.DaysOutstanding, i.CreatedAt,
            i.WorkflowInstanceId, i.IsSubmittedForApproval);

    private static SalesOrderDto ToSalesOrderDto(SalesOrder o) =>
        new(o.Id, o.OrderNumber, o.CustomerId, o.Customer?.Name ?? string.Empty,
            o.OrderDate, o.RequestedShipDate, o.ActualShipDate,
            o.Description, o.CustomerRef, o.Currency, o.Status.ToString(),
            o.SubTotal, o.TaxTotal, o.DiscountTotal, o.GrandTotal,
            o.ARInvoiceId, o.CreatedAt,
            o.Lines.Select(l => new SalesOrderLineDto(
                l.Id, l.ProductVariantId, l.Sku, l.ProductName, l.VariantDescription,
                l.UnitOfMeasure, l.Quantity, l.QuantityShipped, l.UnitPrice, l.DiscountPct, l.TaxRate,
                l.LineSubTotal, l.DiscountAmount, l.TaxAmount, l.LineTotal)).ToList(),
            o.IsExported, o.ExportedAt,
            o.WorkflowInstanceId, o.RejectionReason, o.DeliveredAt, o.DeliveryReference);

    // ── Sales Quotations ──────────────────────────────────────────────────────

    public async Task<IEnumerable<QuotationSummaryDto>> GetQuotationsAsync(
        string? status = null, Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.SalesQuotations
            .Include(q => q.Customer)
            .Include(q => q.Lines)
            .Where(q => !q.IsDeleted);
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<QuotationStatus>(status, out var s))
            query = query.Where(q => q.Status == s);
        if (customerId.HasValue)
            query = query.Where(q => q.CustomerId == customerId.Value);
        var list = await query.OrderByDescending(q => q.QuotationDate).ToListAsync(ct);
        return list.Select(ToQuotationSummaryDto);
    }

    public async Task<QuotationDto?> GetQuotationAsync(Guid id, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .Include(q => q.Customer)
            .Include(q => q.Lines)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, ct);
        return q is null ? null : ToQuotationDto(q);
    }

    public async Task<QuotationDto> CreateQuotationAsync(CreateQuotationRequest req, CancellationToken ct = default)
    {
        var count = await _db.SalesQuotations.CountAsync(ct) + 1;
        var q = new SalesQuotation(_org.OrganizationId, $"QUO-{req.QuotationDate:yyyy}-{count:D5}",
            req.CustomerId, req.QuotationDate, req.ValidUntil, req.Description,
            req.CustomerRef, req.Currency, req.Notes);
        _db.SalesQuotations.Add(q);
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(q.Id, ct))!;
    }

    public async Task<QuotationDto> AddQuotationLineAsync(Guid quotationId, AddQuotationLineRequest req, CancellationToken ct = default)
    {
        var quotation = await _db.SalesQuotations
            .FirstOrDefaultAsync(q => q.Id == quotationId && !q.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");

        var variant = await _db.ProductVariants
            .IgnoreQueryFilters()
            .Include(v => v.Product).ThenInclude(p => p!.Category)
            .FirstOrDefaultAsync(v => v.Id == req.ProductVariantId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Product variant not found.");

        var product = variant.Product ?? throw new InvalidOperationException("Product not found.");
        var unitPrice = req.OverrideUnitPrice ?? variant.EffectivePrice(product.BasePrice);
        var taxRate = product.EffectiveTaxRate(product.Category?.TaxRate ?? 0m);
        var variantDesc = string.Join(", ", new[] { variant.Size, variant.Color, variant.Material }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

        var line = quotation.AddLine(variant.Id, variant.Sku, product.Name,
            string.IsNullOrEmpty(variantDesc) ? null : variantDesc,
            product.UnitOfMeasure, req.Quantity, unitPrice, taxRate, req.DiscountPct);
        _db.SalesQuotationLines.Add(line);
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(quotationId, ct))!;
    }

    public async Task RemoveQuotationLineAsync(Guid quotationId, Guid lineId, CancellationToken ct = default)
    {
        var quotation = await _db.SalesQuotations
            .FirstOrDefaultAsync(q => q.Id == quotationId && !q.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        if (quotation.Status != QuotationStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft quotation.");
        var line = await _db.SalesQuotationLines
            .FirstOrDefaultAsync(l => l.Id == lineId && l.QuotationId == quotationId && !l.IsDeleted, ct)
            ?? throw new InvalidOperationException("Line not found.");
        line.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    public async Task<QuotationDto> SubmitQuotationForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");

        var wf = new WorkflowInstance(_org.OrganizationId, WorkflowDocumentType.SalesQuotation,
            q.Id, q.QuotationNumber, q.GrandTotal, submittedBy);
        wf.AddApprovalStep(1, "Quotation Approval", "SalesManager", null);
        _db.WorkflowInstances.Add(wf);
        await _db.SaveChangesAsync(ct);

        q.SubmitForApproval(wf.Id);
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<QuotationDto> ApproveQuotationAsync(Guid id, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.WorkflowApproved();
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<QuotationDto> RejectQuotationAsync(Guid id, string reason, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.WorkflowRejected(reason);
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<QuotationDto> SendQuotationAsync(Guid id, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.Send();
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<QuotationDto> AcceptQuotationAsync(Guid id, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.Accept();
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<QuotationDto> RejectByCustomerAsync(Guid id, string? reason, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.Reject(reason);
        await _db.SaveChangesAsync(ct);
        return (await GetQuotationAsync(id, ct))!;
    }

    public async Task<SalesOrderDto> ConvertQuotationToSOAsync(Guid quotationId, ConvertQuotationToSORequest req, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == quotationId && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");

        if (q.Status != QuotationStatus.Accepted)
            throw new InvalidOperationException("Only Accepted quotations can be converted to a Sales Order.");

        var orderDate = req.OrderDate ?? DateTime.UtcNow.Date;
        var count = await _db.SalesOrders.CountAsync(ct) + 1;
        var so = new SalesOrder(_org.OrganizationId, $"SO-{orderDate:yyyy}-{count:D5}",
            q.CustomerId, orderDate,
            req.Description ?? q.Description,
            q.CustomerRef, q.Currency);
        _db.SalesOrders.Add(so);
        await _db.SaveChangesAsync(ct);

        foreach (var ql in q.Lines)
        {
            var line = so.AddLine(ql.ProductVariantId, ql.Sku, ql.ProductName,
                ql.VariantDescription, ql.UnitOfMeasure,
                ql.Quantity, ql.UnitPrice, ql.TaxRate, ql.DiscountPct);
            _db.SalesOrderLines.Add(line);
        }

        q.MarkConverted(so.Id);
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(so.Id, ct))!;
    }

    public async Task CancelQuotationAsync(Guid id, CancellationToken ct = default)
    {
        var q = await _db.SalesQuotations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Quotation not found.");
        q.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // ── SO Workflow ───────────────────────────────────────────────────────────

    public async Task<SalesOrderDto> SubmitSOForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");

        var wf = new WorkflowInstance(_org.OrganizationId, WorkflowDocumentType.SalesOrder,
            order.Id, order.OrderNumber, order.GrandTotal, submittedBy);
        wf.AddApprovalStep(1, "Sales Order Approval", "SalesManager", null);
        _db.WorkflowInstances.Add(wf);
        await _db.SaveChangesAsync(ct);

        order.SubmitForApproval(wf.Id);
        _audit.Add("AR", "Submitted for Approval", order.Id, "SalesOrder", null, new
        {
            WorkflowInstanceId = wf.Id,
            SubmittedBy = submittedBy,
            Status = order.Status.ToString()
        });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(id, ct))!;
    }

    public async Task<SalesOrderDto> ApproveSOAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");
        order.WorkflowApproved();
        await ReserveSalesOrderInventoryAsync(order, 0m, ct);
        _audit.Add("AR", "Approved", order.Id, "SalesOrder", null,
            new { Status = order.Status.ToString() });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(id, ct))!;
    }

    public async Task<SalesOrderDto> RejectSOAsync(Guid id, string reason, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");
        order.WorkflowRejected(reason);
        _audit.Add("AR", "Rejected", order.Id, "SalesOrder", null,
            new { Status = order.Status.ToString(), Reason = reason });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(id, ct))!;
    }

    public async Task<SalesOrderDto> ConfirmDeliveryAsync(Guid id, ConfirmDeliveryRequest req, CancellationToken ct = default)
    {
        var order = await _db.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");
        order.ConfirmDelivery(req.DeliveredAt ?? DateTime.UtcNow, req.Reference);
        _audit.Add("AR", "Delivery Confirmed", order.Id, "SalesOrder", null, new
        {
            DeliveredAt = req.DeliveredAt ?? DateTime.UtcNow,
            req.Reference
        });
        await _db.SaveChangesAsync(ct);
        return (await GetSalesOrderAsync(id, ct))!;
    }

    // ── AR Invoice Workflow ───────────────────────────────────────────────────

    public async Task<ARInvoiceDto> SubmitARInvoiceForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices
            .Include(i => i.Customer).Include(i => i.SalesOrder)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var wf = new WorkflowInstance(_org.OrganizationId, WorkflowDocumentType.ARInvoice,
            inv.Id, inv.InvoiceNumber, inv.TotalAmount, submittedBy);
        wf.AddApprovalStep(1, "Invoice Approval", "FinanceManager", null);
        _db.WorkflowInstances.Add(wf);
        await _db.SaveChangesAsync(ct);

        inv.SubmitForApproval(wf.Id);
        await _db.SaveChangesAsync(ct);
        return ToARInvoiceDto(inv);
    }

    public async Task<ARInvoiceDto> ApproveARInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices
            .Include(i => i.Customer).Include(i => i.SalesOrder)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        var journal = await CreateInvoiceJournalAsync(inv, ct);
        inv.WorkflowApproved(journal.Id);
        await _db.SaveChangesAsync(ct);
        return ToARInvoiceDto(inv);
    }

    public async Task<ARInvoiceDto> RejectARInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices
            .Include(i => i.Customer).Include(i => i.SalesOrder)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        inv.WorkflowRejected();
        await _db.SaveChangesAsync(ct);
        return ToARInvoiceDto(inv);
    }

    // ── AR Credit Notes ───────────────────────────────────────────────────────

    public async Task<IEnumerable<ARCreditNoteSummaryDto>> GetARCreditNotesAsync(
        Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.CustomerCreditNotes
            .Include(cn => cn.Customer)
            .Where(cn => !cn.IsDeleted);
        if (customerId.HasValue)
            query = query.Where(cn => cn.CustomerId == customerId.Value);
        var list = await query.OrderByDescending(cn => cn.CreditDate).ToListAsync(ct);
        return list.Select(cn => new ARCreditNoteSummaryDto(
            cn.Id, cn.CreditNoteNumber, cn.CustomerId, cn.Customer?.Name ?? string.Empty,
            cn.ARInvoiceId, cn.CreditDate, cn.Reason.ToString(),
            cn.TotalAmount, cn.AvailableCredit, cn.Status.ToString(), cn.CreatedAt));
    }

    public async Task<ARCreditNoteDto?> GetARCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .Include(x => x.Customer)
            .Include(x => x.ARInvoice)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        return cn is null ? null : ToARCreditNoteDto(cn);
    }

    public async Task<ARCreditNoteDto> CreateARCreditNoteAsync(CreateARCreditNoteRequest req, CancellationToken ct = default)
    {
        if (!Enum.TryParse<ARCreditNoteReason>(req.Reason, out var reason))
            reason = ARCreditNoteReason.Other;

        var count = await _db.CustomerCreditNotes.CountAsync(ct) + 1;
        var cn = new CustomerCreditNote(_org.OrganizationId, $"CN-{count:D6}",
            req.CustomerId, req.CreditDate, req.Description,
            req.SubTotal, req.TaxAmount, reason,
            req.ARInvoiceId, req.SalesOrderId, req.CustomerRef, req.Notes);
        _db.CustomerCreditNotes.Add(cn);
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(cn.Id, ct))!;
    }

    public async Task<ARCreditNoteDto> SubmitCreditNoteForApprovalAsync(Guid id, string submittedBy, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");

        var wf = new WorkflowInstance(_org.OrganizationId, WorkflowDocumentType.ARCreditNote,
            cn.Id, cn.CreditNoteNumber, cn.TotalAmount, submittedBy);
        wf.AddApprovalStep(1, "Credit Note Approval", "FinanceManager", null);
        _db.WorkflowInstances.Add(wf);
        await _db.SaveChangesAsync(ct);

        cn.SubmitForApproval(wf.Id);
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    public async Task<ARCreditNoteDto> ApproveCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.WorkflowApproved();   // → internally calls Issue()
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    public async Task<ARCreditNoteDto> RejectCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.WorkflowRejected();
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    public async Task<ARCreditNoteDto> IssueCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.Issue();
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    public async Task<ARCreditNoteDto> ApplyCreditToInvoiceAsync(Guid id, ApplyCreditNoteRequest req, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");

        var inv = await _db.ARInvoices
            .FirstOrDefaultAsync(i => i.Id == req.ARInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        cn.ApplyCredit(req.Amount);
        inv.ApplyPayment(req.Amount);
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    public async Task<ARCreditNoteDto> VoidCreditNoteAsync(Guid id, CancellationToken ct = default)
    {
        var cn = await _db.CustomerCreditNotes
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Credit note not found.");
        cn.Void();
        await _db.SaveChangesAsync(ct);
        return (await GetARCreditNoteAsync(id, ct))!;
    }

    // ── Dunning / Collections ─────────────────────────────────────────────────

    public async Task<IEnumerable<DunningRecordDto>> GetDunningRecordsAsync(
        Guid? customerId = null, CancellationToken ct = default)
    {
        var query = _db.DunningRecords
            .Include(d => d.Customer)
            .Include(d => d.ARInvoice)
            .Where(d => !d.IsDeleted);
        if (customerId.HasValue)
            query = query.Where(d => d.CustomerId == customerId.Value);
        var list = await query.OrderByDescending(d => d.SentDate).ToListAsync(ct);
        return list.Select(ToDunningDto);
    }

    public async Task<DunningRecordDto> CreateDunningAsync(CreateDunningRequest req, CancellationToken ct = default)
    {
        if (!Enum.TryParse<DunningLevel>(req.Level, out var level))
            level = DunningLevel.Reminder;

        var inv = await _db.ARInvoices
            .FirstOrDefaultAsync(i => i.Id == req.ARInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var count = await _db.DunningRecords.CountAsync(ct) + 1;
        var followUpDate = req.FollowUpDate ?? DateTime.UtcNow.Date.AddDays(7);
        var d = new DunningRecord(_org.OrganizationId, $"DUN-{count:D5}",
            req.CustomerId, req.ARInvoiceId, level,
            DateTime.UtcNow.Date, followUpDate,
            inv.OutstandingAmount, req.AssignedTo, req.Notes);
        _db.DunningRecords.Add(d);
        await _db.SaveChangesAsync(ct);

        var created = await _db.DunningRecords
            .Include(x => x.Customer).Include(x => x.ARInvoice)
            .FirstAsync(x => x.Id == d.Id, ct);
        return ToDunningDto(created);
    }

    public async Task<DunningRecordDto> ResolveDunningAsync(Guid id, string? notes, CancellationToken ct = default)
    {
        var d = await _db.DunningRecords
            .Include(x => x.Customer).Include(x => x.ARInvoice)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Dunning record not found.");
        d.Resolve(notes);
        await _db.SaveChangesAsync(ct);
        return ToDunningDto(d);
    }

    public async Task<DunningRecordDto> EscalateDunningAsync(Guid id, CancellationToken ct = default)
    {
        var d = await _db.DunningRecords
            .Include(x => x.Customer).Include(x => x.ARInvoice)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Dunning record not found.");
        d.Escalate();
        await _db.SaveChangesAsync(ct);
        return ToDunningDto(d);
    }

    // ── Mapping helpers (S2C additions) ───────────────────────────────────────

    private static QuotationSummaryDto ToQuotationSummaryDto(SalesQuotation q) =>
        new(q.Id, q.QuotationNumber, q.CustomerId, q.Customer?.Name ?? string.Empty,
            q.QuotationDate, q.ValidUntil, q.CustomerRef, q.Status.ToString(),
            q.GrandTotal, q.Lines.Count, q.WorkflowInstanceId, q.ConvertedToSOId, q.CreatedAt);

    private static QuotationDto ToQuotationDto(SalesQuotation q) =>
        new(q.Id, q.QuotationNumber, q.CustomerId, q.Customer?.Name ?? string.Empty,
            q.QuotationDate, q.ValidUntil, q.Description, q.CustomerRef, q.Currency,
            q.Status.ToString(), q.SubTotal, q.TaxTotal, q.DiscountTotal, q.GrandTotal,
            q.WorkflowInstanceId, q.RejectionReason, q.ConvertedToSOId, q.ConvertedAt,
            q.Notes, q.CreatedAt,
            q.Lines.Select((l, i) => new QuotationLineDto(
                l.Id, i + 1, l.ProductVariantId, l.Sku, l.ProductName,
                l.VariantDescription, l.UnitOfMeasure, l.Quantity, l.UnitPrice,
                l.DiscountPct, l.TaxRate, l.LineSubTotal, l.DiscountAmount,
                l.TaxAmount, l.LineTotal)).ToList());

    private static ARCreditNoteDto ToARCreditNoteDto(CustomerCreditNote cn) =>
        new(cn.Id, cn.CreditNoteNumber, cn.CustomerId, cn.Customer?.Name ?? string.Empty,
            cn.ARInvoiceId, cn.ARInvoice?.InvoiceNumber, cn.SalesOrderId, null,
            cn.CreditDate, cn.Description, cn.CustomerRef,
            cn.SubTotal, cn.TaxAmount, cn.TotalAmount,
            cn.AppliedAmount, cn.AvailableCredit,
            cn.Status.ToString(), cn.Reason.ToString(), cn.Notes,
            cn.WorkflowInstanceId, cn.CreatedAt);

    private static DunningRecordDto ToDunningDto(DunningRecord d) =>
        new(d.Id, d.DunningNumber, d.CustomerId, d.Customer?.Name ?? string.Empty,
            d.ARInvoiceId, d.ARInvoice?.InvoiceNumber ?? string.Empty,
            d.Level.ToString(), d.Status.ToString(),
            d.SentDate, d.FollowUpDate, d.OutstandingAmount,
            d.AssignedTo, d.Notes,
            d.ResolvedAt, d.ResolutionNotes, d.CreatedAt);
}
