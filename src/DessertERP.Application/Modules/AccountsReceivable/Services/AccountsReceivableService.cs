using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.AccountsReceivable.DTOs;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.AccountsReceivable.Services;

public interface IAccountsReceivableService
{
    // Customers
    Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken ct = default);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest req, CancellationToken ct = default);

    // Sales Orders
    Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersAsync(string? status = null, Guid? customerId = null, CancellationToken ct = default);
    Task<SalesOrderDto?> GetSalesOrderAsync(Guid id, CancellationToken ct = default);
    Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest req, CancellationToken ct = default);
    Task<SalesOrderDto> AddSalesOrderLineAsync(Guid orderId, AddSalesOrderLineRequest req, CancellationToken ct = default);
    Task RemoveSalesOrderLineAsync(Guid orderId, Guid lineId, CancellationToken ct = default);
    Task ConfirmSalesOrderAsync(Guid id, CancellationToken ct = default);
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

    // Reports
    Task<IEnumerable<ARAgingDto>> GetAgingReportAsync(CancellationToken ct = default);
}

public class AccountsReceivableService : IAccountsReceivableService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public AccountsReceivableService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db = db;
        _org = org;
    }

    // ── Customers ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken ct = default)
    {
        var list = await _db.Customers.Where(c => !c.IsDeleted).OrderBy(c => c.CustomerNumber).ToListAsync(ct);
        return list.Select(ToCustomerDto);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest req, CancellationToken ct = default)
    {
        var count = await _db.Customers.CountAsync(ct) + 1;
        var customer = new Customer(_org.OrganizationId, $"CUST-{count:D5}", req.Name, req.Email, req.Phone,
            req.Address, req.Currency, req.PaymentTermsDays, req.CreditLimit);
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);
        return ToCustomerDto(customer);
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
            o.Status.ToString(), o.GrandTotal, o.Lines.Count, o.CreatedAt));
    }

    public async Task<SalesOrderDto?> GetSalesOrderAsync(Guid id, CancellationToken ct = default)
    {
        var o = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct);
        return o is null ? null : ToSalesOrderDto(o);
    }

    public async Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest req, CancellationToken ct = default)
    {
        var count = await _db.SalesOrders.CountAsync(ct) + 1;
        var order = new SalesOrder(_org.OrganizationId, $"SO-{req.OrderDate:yyyy}-{count:D5}",
            req.CustomerId, req.OrderDate, req.Description,
            req.CustomerRef, req.Currency, req.RequestedShipDate);
        _db.SalesOrders.Add(order);
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
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == req.ProductVariantId && !v.IsDeleted, ct)
            ?? throw new InvalidOperationException("Product variant not found.");

        var product = variant.Product
            ?? throw new InvalidOperationException("Product not found for variant.");
        var unitPrice = req.OverrideUnitPrice ?? variant.EffectivePrice(product.BasePrice);
        var variantDesc = string.Join(", ", new[] { variant.Size, variant.Color, variant.Material }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

        // Domain method validates status, updates order totals, and returns the new line.
        // Explicitly Add() the line to the DbSet so EF tracks it in Added state —
        // avoids concurrency exceptions from collection-change-tracking with query filters.
        var line = order.AddLine(variant.Id, variant.Sku, product.Name,
            string.IsNullOrEmpty(variantDesc) ? null : variantDesc,
            product.UnitOfMeasure, req.Quantity, unitPrice, product.TaxRate, req.DiscountPct);
        _db.SalesOrderLines.Add(line);
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
        order.RecalcTotalsFromLines(remaining);

        await _db.SaveChangesAsync(ct);
    }

    public async Task ConfirmSalesOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        order.Confirm();
        await _db.SaveChangesAsync(ct);
    }

    public async Task StartPickingAsync(Guid id, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        order.StartPicking();
        await _db.SaveChangesAsync(ct);
    }

    public async Task ShipSalesOrderAsync(Guid id, ShipOrderRequest req, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        order.Ship(req.ShipDate);
        await _db.SaveChangesAsync(ct);
    }

    public async Task CancelSalesOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await LoadOrderWithLines(id, ct);
        order.Cancel();
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
        var inv = await _db.ARInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
        inv.Issue();
        await _db.SaveChangesAsync(ct);
    }

    public async Task VoidInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _db.ARInvoices.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");
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
        var invoice = await _db.ARInvoices.Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == req.ARInvoiceId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Invoice not found.");

        var count = await _db.ARPayments.CountAsync(ct) + 1;
        if (!Enum.TryParse<PaymentMethod>(req.PaymentMethod, out var method))
            method = PaymentMethod.BankTransfer;

        var payment = new ARPayment(_org.OrganizationId, $"RCPT-{count:D6}", req.CustomerId, req.ARInvoiceId,
            req.PaymentDate, req.Amount, method, req.Reference);

        invoice.ApplyPayment(req.Amount);
        _db.ARPayments.Add(payment);
        payment.Post();
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

    // ── Aging Report ──────────────────────────────────────────────────────────

    public async Task<IEnumerable<ARAgingDto>> GetAgingReportAsync(CancellationToken ct = default)
    {
        var invoices = await _db.ARInvoices
            .Include(i => i.Customer)
            .Where(i => !i.IsDeleted && i.Status != ARInvoiceStatus.FullyPaid && i.Status != ARInvoiceStatus.Voided)
            .ToListAsync(ct);

        return invoices
            .GroupBy(i => new { i.CustomerId, i.Customer!.CustomerNumber, i.Customer.Name })
            .Select(g =>
            {
                var today = DateTime.UtcNow.Date;
                return new ARAgingDto(
                    g.Key.CustomerNumber, g.Key.Name,
                    g.Where(i => i.DaysOutstanding == 0).Sum(i => i.OutstandingAmount),
                    g.Where(i => i.DaysOutstanding is > 0 and <= 30).Sum(i => i.OutstandingAmount),
                    g.Where(i => i.DaysOutstanding is > 30 and <= 60).Sum(i => i.OutstandingAmount),
                    g.Where(i => i.DaysOutstanding is > 60 and <= 90).Sum(i => i.OutstandingAmount),
                    g.Where(i => i.DaysOutstanding > 90).Sum(i => i.OutstandingAmount),
                    g.Sum(i => i.OutstandingAmount));
            })
            .OrderByDescending(r => r.Total);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private Task<SalesOrder> LoadOrderWithLines(Guid id, CancellationToken ct) =>
        _db.SalesOrders.Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
        is Task<SalesOrder?> t
            ? t.ContinueWith(r => r.Result ?? throw new InvalidOperationException("Sales order not found."), ct)
            : throw new InvalidOperationException("Sales order not found.");

    private static CustomerDto ToCustomerDto(Customer c) => new(
        c.Id, c.CustomerNumber, c.Name, c.Email, c.Phone, c.Address,
        c.Currency, c.PaymentTermsDays, c.CreditLimit, c.Status.ToString(), c.CreatedAt);

    private static SalesOrderDto ToSalesOrderDto(SalesOrder o)
    {
        // Compute totals live from lines — avoids stale persisted column values
        var lineDtos = o.Lines
            .Where(l => !l.IsDeleted)
            .Select(l => new SalesOrderLineDto(
                l.Id, l.ProductVariantId, l.Sku, l.ProductName, l.VariantDescription,
                l.UnitOfMeasure, l.Quantity, l.UnitPrice, l.DiscountPct, l.TaxRate,
                l.LineSubTotal, l.DiscountAmount, l.TaxAmount, l.LineTotal))
            .ToList();

        var subTotal      = lineDtos.Sum(l => l.LineSubTotal);
        var discountTotal = lineDtos.Sum(l => l.DiscountAmount);
        var taxTotal      = lineDtos.Sum(l => l.TaxAmount);
        var grandTotal    = lineDtos.Sum(l => l.LineTotal);

        return new SalesOrderDto(
            o.Id, o.OrderNumber, o.CustomerId, o.Customer?.Name ?? string.Empty,
            o.OrderDate, o.RequestedShipDate, o.ActualShipDate,
            o.Description, o.CustomerRef, o.Currency, o.Status.ToString(),
            subTotal, taxTotal, discountTotal, grandTotal,
            o.ARInvoiceId, o.CreatedAt, lineDtos);
    }

    private static ARInvoiceDto ToARInvoiceDto(ARInvoice i) => new(
        i.Id, i.InvoiceNumber, i.CustomerId, i.Customer?.Name ?? string.Empty,
        i.SalesOrderId, i.SalesOrder?.OrderNumber,
        i.InvoiceDate, i.DueDate, i.Description,
        i.SubTotal, i.TaxAmount, i.DiscountAmount, i.TotalAmount,
        i.PaidAmount, i.OutstandingAmount, i.Status.ToString(),
        i.DaysOutstanding, i.CreatedAt);
}
