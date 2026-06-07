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
    Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequest req, CancellationToken ct = default);
    Task<CustomerLedgerDto> GetCustomerLedgerAsync(Guid customerId, CancellationToken ct = default);

    // Sales Orders
    Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersAsync(string? status = null, Guid? customerId = null, CancellationToken ct = default);
    Task<SalesOrderDto?> GetSalesOrderAsync(Guid id, CancellationToken ct = default);
    Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest req, CancellationToken ct = default);
    Task<SalesOrderDto> AddSalesOrderLineAsync(Guid orderId, AddSalesOrderLineRequest req, CancellationToken ct = default);
    Task RemoveSalesOrderLineAsync(Guid orderId, Guid lineId, CancellationToken ct = default);
    Task<SalesOrderDto> ApplyDiscountToOrderAsync(Guid orderId, decimal discountPct, CancellationToken ct = default);
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
            o.IsExported, o.ExportedAt));
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

        foreach (var line in lines)
            line.Update(line.Quantity, line.UnitPrice, discountPct);

        // Recalculate order-level totals from the updated lines.
        order.RecalcTotalsFromLines(lines);

        await _db.SaveChangesAsync(ct);

        return (await GetSalesOrderAsync(orderId, ct))!;
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

    private async Task<SalesOrder> LoadOrderWithLines(Guid id, CancellationToken ct)
    {
        return await _db.SalesOrders
            .Include(o => o.Lines)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct)
            ?? throw new InvalidOperationException("Sales order not found.");
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
            i.DaysOutstanding, i.CreatedAt);

    private static SalesOrderDto ToSalesOrderDto(SalesOrder o) =>
        new(o.Id, o.OrderNumber, o.CustomerId, o.Customer?.Name ?? string.Empty,
            o.OrderDate, o.RequestedShipDate, o.ActualShipDate,
            o.Description, o.CustomerRef, o.Currency, o.Status.ToString(),
            o.SubTotal, o.TaxTotal, o.DiscountTotal, o.GrandTotal,
            o.ARInvoiceId, o.CreatedAt,
            o.Lines.Select(l => new SalesOrderLineDto(
                l.Id, l.ProductVariantId, l.Sku, l.ProductName, l.VariantDescription,
                l.UnitOfMeasure, l.Quantity, l.UnitPrice, l.DiscountPct, l.TaxRate,
                l.LineSubTotal, l.DiscountAmount, l.TaxAmount, l.LineTotal)).ToList(),
            o.IsExported, o.ExportedAt);
}
