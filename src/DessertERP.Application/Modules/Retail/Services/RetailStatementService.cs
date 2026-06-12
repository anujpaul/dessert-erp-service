using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.CashBank;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.Retail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DessertERP.Application.Modules.Retail.Services;

public record RetailImportResult(Guid StagingId, string StagingStatus, Guid? TransactionId,
    Guid? StatementId, string TransactionNumber, bool Duplicate,
    int MatchedLines, int UnmatchedLines, string? ValidationMessage);

public record RetailStagingDto(Guid Id, string SourceFile, string SourceHash, string Status,
    string StoreCode, string TransactionNumber, DateTime BusinessDate, DateTime TransactionDate,
    string Currency, decimal GrandTotal, int LineCount, int TenderCount,
    int MatchedLines, int UnmatchedLines, string? ValidationMessage,
    Guid? PromotedTransactionId, Guid? RetailStatementId, DateTime CreatedAt, DateTime? PromotedAt);

public record RetailStatementDto(Guid Id, string StatementNumber, Guid StoreId, string StoreName,
    DateTime BusinessDate, string Currency, string Status, int TransactionCount,
    decimal NetSales, decimal DiscountTotal, decimal TaxTotal, decimal GrandTotal,
    decimal CostTotal, Guid? ARInvoiceId, Guid? ARCreditNoteId, Guid? JournalEntryId, DateTime? PostedAt,
    string? PostingError);

public record RetailSettlementDto(Guid Id, Guid RetailStatementId, string StatementNumber,
    string PaymentMethod, decimal Amount, string Currency, string Status,
    string? ProcessorReference, Guid? BankTransactionId, DateTime? SettledAt);

public interface IRetailStatementService
{
    Task<RetailImportResult> ImportPosLogAsync(Guid organizationId, Stream xml,
        string sourceFile, CancellationToken ct = default);
    Task<RetailStagingDto> StagePosLogAsync(Guid organizationId, Stream xml,
        string sourceFile, CancellationToken ct = default);
    Task<RetailImportResult> PromoteStagedAsync(Guid stagingId,
        CancellationToken ct = default);
    Task<List<RetailStagingDto>> GetStagedTransactionsAsync(Guid organizationId,
        int page = 1, int pageSize = 100, CancellationToken ct = default);
    Task<int> PostOpenStatementsAsync(Guid organizationId, CancellationToken ct = default);
    Task PostStatementAsync(Guid statementId, CancellationToken ct = default);
    Task<List<RetailStatementDto>> GetStatementsAsync(Guid organizationId,
        int page = 1, int pageSize = 100, CancellationToken ct = default);
    Task<List<RetailSettlementDto>> GetSettlementsAsync(Guid organizationId,
        string? status = null, CancellationToken ct = default);
    Task SettleCardAsync(Guid settlementId, Guid bankAccountId, decimal merchantFee,
        string? processorReference, string postedBy, CancellationToken ct = default);
}

public class RetailStatementService : IRetailStatementService
{
    private readonly IAppDbContext _db;
    private readonly ILogger<RetailStatementService> _logger;

    public RetailStatementService(IAppDbContext db, ILogger<RetailStatementService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<RetailImportResult> ImportPosLogAsync(Guid organizationId, Stream xml,
        string sourceFile, CancellationToken ct = default)
    {
        var staged = await StagePosLogAsync(organizationId, xml, sourceFile, ct);
        if (staged.Status is nameof(RetailStagingStatus.Invalid) or nameof(RetailStagingStatus.Failed))
            return new RetailImportResult(staged.Id, staged.Status, null, null,
                staged.TransactionNumber, false, staged.MatchedLines, staged.UnmatchedLines,
                staged.ValidationMessage);

        return await PromoteStagedAsync(staged.Id, ct);
    }

    public async Task<RetailStagingDto> StagePosLogAsync(Guid organizationId, Stream xml,
        string sourceFile, CancellationToken ct = default)
    {
        await using var source = new MemoryStream();
        await xml.CopyToAsync(source, ct);
        var bytes = source.ToArray();
        var sourceHash = Convert.ToHexString(SHA256.HashData(bytes));

        var existing = await _db.RetailTransactionStaging
            .Include(s => s.Lines)
            .Include(s => s.Tenders)
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId &&
                s.SourceHash == sourceHash, ct);
        if (existing is not null)
            return ToStagingDto(existing);

        source.Position = 0;
        string rawXml;
        using (var reader = new StreamReader(source, Encoding.UTF8, true, leaveOpen: true))
            rawXml = await reader.ReadToEndAsync(ct);
        source.Position = 0;
        var parsed = await ParseAsync(source, ct);

        var staging = new RetailTransactionStaging(organizationId, sourceFile, sourceHash,
            rawXml, parsed.StoreCode, parsed.TransactionId, parsed.OperatorId,
            parsed.BusinessDate, parsed.TransactionDate, parsed.Currency, parsed.IsReturn,
            parsed.SubTotal, parsed.DiscountTotal, parsed.TaxTotal, parsed.GrandTotal);

        var identifiers = parsed.Lines
            .SelectMany(l => new[] { l.Sku, l.PosItemId })
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var variants = await _db.ProductVariants
            .Where(v => v.OrganizationId == organizationId &&
                (identifiers.Contains(v.Sku) ||
                    (v.Barcode != null && identifiers.Contains(v.Barcode))))
            .ToListAsync(ct);

        ProductVariant? Match(ParsedLine line) => variants.FirstOrDefault(v =>
            string.Equals(v.Sku, line.Sku, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Barcode, line.Sku, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Sku, line.PosItemId, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Barcode, line.PosItemId, StringComparison.OrdinalIgnoreCase));

        var lineNumber = 0;
        foreach (var parsedLine in parsed.Lines)
        {
            var line = new RetailTransactionStagingLine(staging.Id, ++lineNumber,
                parsedLine.Sku, parsedLine.PosItemId, parsedLine.ProductName,
                parsedLine.Quantity, parsedLine.UnitPrice, parsedLine.DiscountAmount,
                parsedLine.TaxAmount, parsedLine.LineSubTotal, parsedLine.LineTotal,
                parsedLine.UnitOfMeasure, parsedLine.IsReturn);
            var variant = Match(parsedLine);
            line.SetProductMatch(variant?.Id, variant is null ? "No product variant matched this SKU." : null);
            staging.AddLine(line);
        }

        var tenderSequence = 0;
        foreach (var payment in parsed.Payments)
            staging.AddTender(new RetailTransactionStagingTender(staging.Id, ++tenderSequence,
                payment.Method, payment.Amount, payment.Reference));

        var validationErrors = new List<string>();
        var existingTransaction = await _db.POSTransactions.AnyAsync(t =>
            t.OrganizationId == organizationId && t.ExternalRef == parsed.TransactionId, ct);
        if (existingTransaction)
            validationErrors.Add($"Transaction {parsed.TransactionId} already exists in operational tables.");

        var lineTotal = parsed.Lines.Sum(l => l.LineTotal);
        if (Math.Abs(lineTotal - parsed.GrandTotal) > 0.02m)
            validationErrors.Add(
                $"Line total {lineTotal:0.00} does not match transaction total {parsed.GrandTotal:0.00}.");

        var tenderTotal = parsed.Payments.Sum(p => p.Amount);
        if (Math.Abs(tenderTotal - parsed.GrandTotal) > 0.02m)
            validationErrors.Add(
                $"Tender total {tenderTotal:0.00} does not match transaction total {parsed.GrandTotal:0.00}.");

        var unmatched = staging.Lines.Count(l => l.MatchedProductVariantId is null);
        if (validationErrors.Count > 0)
            staging.MarkInvalid(string.Join(" ", validationErrors));
        else
            staging.MarkValid(unmatched > 0
                ? $"{unmatched} line(s) have no matching product variant and will be promoted without an inventory match."
                : null);

        _db.RetailTransactionStaging.Add(staging);
        await _db.SaveChangesAsync(ct);
        return ToStagingDto(staging);
    }

    public async Task<RetailImportResult> PromoteStagedAsync(Guid stagingId,
        CancellationToken ct = default)
    {
        var staging = await _db.RetailTransactionStaging
            .Include(s => s.Lines)
            .Include(s => s.Tenders)
            .FirstOrDefaultAsync(s => s.Id == stagingId, ct)
            ?? throw new InvalidOperationException("Retail staging transaction not found.");

        if (staging.Status == RetailStagingStatus.Promoted)
            return new RetailImportResult(staging.Id, staging.Status.ToString(),
                staging.PromotedTransactionId, staging.RetailStatementId,
                staging.TransactionNumber, true,
                staging.Lines.Count(l => l.MatchedProductVariantId is not null),
                staging.Lines.Count(l => l.MatchedProductVariantId is null), null);
        if (staging.Status != RetailStagingStatus.Valid)
            throw new InvalidOperationException(
                $"Only valid staged transactions can be promoted. Current status: {staging.Status}. {staging.ValidationMessage}");

        var parsed = new ParsedTransaction(staging.StoreCode, staging.TransactionNumber,
            staging.OperatorId, staging.BusinessDate, staging.TransactionDate, staging.Currency,
            staging.IsReturn, staging.SubTotal, staging.DiscountTotal, staging.TaxTotal,
            staging.GrandTotal,
            staging.Lines.OrderBy(l => l.LineNumber).Select(l => new ParsedLine(
                l.Sku, l.PosItemId, l.ProductName, l.Quantity, l.UnitPrice,
                l.DiscountAmount, l.TaxAmount, l.LineSubTotal, l.LineTotal,
                l.UnitOfMeasure, l.IsReturn)).ToList(),
            staging.Tenders.OrderBy(t => t.Sequence).Select(t =>
                new ParsedPayment(t.PaymentMethod, t.Amount, t.Reference)).ToList());

        var result = await PromoteParsedAsync(staging.OrganizationId, parsed,
            staging.SourceFile, ct);
        if (result.TransactionId is null || result.StatementId is null)
            throw new InvalidOperationException(
                "The staged transaction could not be linked to an operational transaction and statement.");
        staging.MarkPromoted(result.TransactionId.Value, result.StatementId.Value);
        await _db.SaveChangesAsync(ct);
        return result with
        {
            StagingId = staging.Id,
            StagingStatus = staging.Status.ToString(),
            ValidationMessage = null
        };
    }

    public async Task<List<RetailStagingDto>> GetStagedTransactionsAsync(Guid organizationId,
        int page = 1, int pageSize = 100, CancellationToken ct = default)
    {
        var rows = await _db.RetailTransactionStaging
            .AsNoTracking()
            .Include(s => s.Lines)
            .Include(s => s.Tenders)
            .Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.CreatedAt)
            .Skip((Math.Max(1, page) - 1) * Math.Clamp(pageSize, 1, 500))
            .Take(Math.Clamp(pageSize, 1, 500))
            .ToListAsync(ct);
        return rows.Select(ToStagingDto).ToList();
    }

    private async Task<RetailImportResult> PromoteParsedAsync(Guid organizationId,
        ParsedTransaction parsed, string sourceFile, CancellationToken ct)
    {
        var duplicate = await _db.POSTransactions
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId &&
                t.ExternalRef == parsed.TransactionId, ct);
        if (duplicate is not null)
            return new RetailImportResult(Guid.Empty, RetailStagingStatus.Promoted.ToString(),
                duplicate.Id, duplicate.RetailStatementId, duplicate.TransactionNumber,
                true, 0, parsed.Lines.Count, null);

        var store = await _db.RetailStores
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId &&
                s.StoreCode == parsed.StoreCode, ct);
        if (store is null)
        {
            store = new RetailStore(organizationId, parsed.StoreCode,
                $"Retail Store {parsed.StoreCode}");
            _db.RetailStores.Add(store);
        }

        var statement = await _db.RetailStatements
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId &&
                s.StoreId == store.Id && s.BusinessDate == parsed.BusinessDate.Date &&
                s.Currency == parsed.Currency && s.Status == RetailStatementStatus.Open, ct);
        if (statement is null)
        {
            statement = new RetailStatement(organizationId, store.Id,
                $"RST-{parsed.StoreCode}-{parsed.BusinessDate:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}",
                parsed.BusinessDate, parsed.Currency);
            _db.RetailStatements.Add(statement);
        }

        var identifiers = parsed.Lines
            .SelectMany(l => new[] { l.Sku, l.PosItemId })
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var variants = await _db.ProductVariants
            .Include(v => v.Product)
            .Where(v => v.OrganizationId == organizationId &&
                (identifiers.Contains(v.Sku) || (v.Barcode != null && identifiers.Contains(v.Barcode))))
            .ToListAsync(ct);
        var variantIds = variants.Select(v => v.Id).ToList();
        var inventoryByVariant = await _db.InventoryRecords
            .Where(i => variantIds.Contains(i.ProductVariantId))
            .ToDictionaryAsync(i => i.ProductVariantId, ct);

        ProductVariant? Match(ParsedLine line) => variants.FirstOrDefault(v =>
            string.Equals(v.Sku, line.Sku, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Barcode, line.Sku, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Sku, line.PosItemId, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(v.Barcode, line.PosItemId, StringComparison.OrdinalIgnoreCase));

        var tx = new POSTransaction(organizationId, store.Id, parsed.TransactionId,
            parsed.TransactionDate, parsed.IsReturn ? POSTransactionType.Return : POSTransactionType.Sale,
            parsed.TransactionId, parsed.OperatorId, null, parsed.Currency, null, sourceFile);
        tx.AssignToStatement(statement.Id);
        tx.SetImportedTotals(parsed.SubTotal, parsed.DiscountTotal, parsed.TaxTotal,
            parsed.GrandTotal, parsed.Payments.Sum(p => p.Amount));
        _db.POSTransactions.Add(tx);

        var matched = 0;
        decimal costTotal = 0m;
        foreach (var sourceLine in parsed.Lines)
        {
            var variant = Match(sourceLine);
            if (variant is not null)
            {
                matched++;
                if (inventoryByVariant.TryGetValue(variant.Id, out var inventory))
                    costTotal += -sourceLine.Quantity * inventory.AverageCost;
            }

            _db.POSTransactionLines.Add(new POSTransactionLine(tx.Id,
                sourceLine.Sku, sourceLine.ProductName, sourceLine.Quantity,
                sourceLine.UnitPrice, sourceLine.DiscountAmount, sourceLine.TaxAmount,
                sourceLine.LineSubTotal, sourceLine.LineTotal, variant?.Id,
                sourceLine.UnitOfMeasure, sourceLine.IsReturn));
        }

        foreach (var payment in parsed.Payments)
            _db.POSPayments.Add(new POSPayment(tx.Id, payment.Method, payment.Amount, payment.Reference));

        statement.AddTransaction(parsed.SubTotal, parsed.DiscountTotal, parsed.TaxTotal,
            parsed.GrandTotal, costTotal);
        return new RetailImportResult(Guid.Empty, RetailStagingStatus.Valid.ToString(),
            tx.Id, statement.Id, tx.TransactionNumber, false,
            matched, parsed.Lines.Count - matched, null);
    }

    private static RetailStagingDto ToStagingDto(RetailTransactionStaging staging)
        => new(staging.Id, staging.SourceFile, staging.SourceHash, staging.Status.ToString(),
            staging.StoreCode, staging.TransactionNumber, staging.BusinessDate,
            staging.TransactionDate, staging.Currency, staging.GrandTotal,
            staging.Lines.Count, staging.Tenders.Count,
            staging.Lines.Count(l => l.MatchedProductVariantId is not null),
            staging.Lines.Count(l => l.MatchedProductVariantId is null),
            staging.ValidationMessage, staging.PromotedTransactionId,
            staging.RetailStatementId, staging.CreatedAt, staging.PromotedAt);

    public async Task<int> PostOpenStatementsAsync(Guid organizationId, CancellationToken ct = default)
    {
        var ids = await _db.RetailStatements
            .Where(s => s.OrganizationId == organizationId && s.Status == RetailStatementStatus.Open)
            .OrderBy(s => s.BusinessDate)
            .Select(s => s.Id)
            .ToListAsync(ct);
        foreach (var id in ids)
            await PostStatementAsync(id, ct);
        return ids.Count;
    }

    public async Task PostStatementAsync(Guid statementId, CancellationToken ct = default)
    {
        var statement = await _db.RetailStatements
            .FirstOrDefaultAsync(s => s.Id == statementId, ct)
            ?? throw new InvalidOperationException("Retail statement not found.");
        if (statement.Status == RetailStatementStatus.Posted) return;
        if (statement.GrandTotal == 0)
            throw new InvalidOperationException("A zero-value retail statement cannot be posted.");

        var transactions = await _db.POSTransactions
            .Include(t => t.Lines)
            .Include(t => t.Payments)
            .Where(t => t.RetailStatementId == statement.Id)
            .ToListAsync(ct);
        if (transactions.Count == 0)
            throw new InvalidOperationException("The retail statement has no transactions.");

        var period = await _db.FiscalPeriods.Include(p => p.FiscalYear)
                .ThenInclude(y => y!.FiscalCalendar)
            .FirstOrDefaultAsync(p => p.FiscalYear!.OrganizationId == statement.OrganizationId &&
                p.FiscalYear.FiscalCalendar!.IsDefault &&
                p.FiscalYear.Status == FiscalYearStatus.Open &&
                p.Status == FiscalPeriodStatus.Open &&
                p.StartDate.Date <= statement.BusinessDate &&
                p.EndDate.Date >= statement.BusinessDate, ct)
            ?? throw new InvalidOperationException(
                $"No open fiscal period exists for retail business date {statement.BusinessDate:d}.");

        var customer = await _db.Customers.FirstOrDefaultAsync(c =>
            c.OrganizationId == statement.OrganizationId &&
            c.CustomerNumber == "RETAIL-WALKIN", ct);
        if (customer is null)
        {
            customer = new Customer(statement.OrganizationId, "RETAIL-WALKIN",
                "Walk-in Customer", paymentTermsDays: 0);
            _db.Customers.Add(customer);
        }

        ARInvoice? invoice = null;
        CustomerCreditNote? creditNote = null;
        if (statement.GrandTotal > 0)
        {
            invoice = new ARInvoice(statement.OrganizationId,
                $"RET-{statement.StatementNumber}", customer.Id, statement.BusinessDate,
                statement.BusinessDate, $"Retail statement {statement.StatementNumber}",
                statement.NetSales, statement.TaxTotal, statement.DiscountTotal);
            _db.ARInvoices.Add(invoice);
        }
        else
        {
            creditNote = new CustomerCreditNote(statement.OrganizationId,
                $"RCN-{statement.StatementNumber}", customer.Id, statement.BusinessDate,
                $"Retail refund statement {statement.StatementNumber}",
                Math.Abs(statement.NetSales - statement.DiscountTotal),
                Math.Abs(statement.TaxTotal), ARCreditNoteReason.Return,
                customerRef: statement.StatementNumber);
            creditNote.Issue();
            _db.CustomerCreditNotes.Add(creditNote);
        }

        var journalCount = await _db.JournalEntries.CountAsync(ct) + 1;
        var journal = new JournalEntry(statement.OrganizationId, $"JE-{journalCount:D6}",
            statement.BusinessDate, period.Id, $"Retail statement {statement.StatementNumber}",
            statement.StatementNumber, "RetailStatement", statement.Currency);
        _db.JournalEntries.Add(journal);

        var accounts = await LoadOrCreateAccountsAsync(statement.OrganizationId, ct);
        AddSignedLine(journal, accounts["1210"].Id, "Retail receivable", statement.GrandTotal, true);
        AddSignedLine(journal, accounts["4100"].Id, "Retail sales revenue",
            statement.NetSales - statement.DiscountTotal, false);
        AddSignedLine(journal, accounts["2210"].Id, "Retail sales tax", statement.TaxTotal, false);

        var tenderGroups = transactions.SelectMany(t => t.Payments)
            .GroupBy(p => p.PaymentMethod)
            .Select(g => new { Method = g.Key, Amount = g.Sum(p => p.Amount),
                Reference = g.Select(p => p.Reference).FirstOrDefault(r => !string.IsNullOrWhiteSpace(r)) })
            .Where(g => g.Amount != 0)
            .ToList();

        foreach (var tender in tenderGroups)
        {
            var account = tender.Method switch
            {
                POSPaymentMethod.Cash => "1110",
                POSPaymentMethod.GiftCard or POSPaymentMethod.StoreCredit => "2310",
                _ => "1130"
            };
            AddSignedLine(journal, accounts[account].Id, $"{tender.Method} tender", tender.Amount, true);
            _db.RetailTenderSettlements.Add(new RetailTenderSettlement(statement.OrganizationId,
                statement.Id, tender.Method, tender.Amount, statement.Currency, tender.Reference));
        }

        AddSignedLine(journal, accounts["1210"].Id, "Settle retail receivable",
            statement.GrandTotal, false);
        if (invoice is not null)
        {
            var summaryPayment = new ARPayment(statement.OrganizationId,
                $"RCP-{statement.StatementNumber}", customer.Id, invoice.Id,
                statement.BusinessDate, statement.GrandTotal, PaymentMethod.Other,
                statement.StatementNumber);
            summaryPayment.Post(journal.Id);
            _db.ARPayments.Add(summaryPayment);
        }
        if (statement.CostTotal != 0)
        {
            AddSignedLine(journal, accounts["5100"].Id, "Retail cost of goods sold",
                statement.CostTotal, true);
            AddSignedLine(journal, accounts["1310"].Id, "Retail inventory issued",
                statement.CostTotal, false);
        }

        var postingVariantIds = transactions.SelectMany(t => t.Lines)
            .Where(l => l.ProductVariantId.HasValue)
            .Select(l => l.ProductVariantId!.Value)
            .Distinct()
            .ToList();
        var postingInventory = await _db.InventoryRecords
            .Where(i => postingVariantIds.Contains(i.ProductVariantId))
            .ToDictionaryAsync(i => i.ProductVariantId, ct);

        foreach (var tx in transactions)
        {
            foreach (var line in tx.Lines.Where(l => l.ProductVariantId.HasValue))
            {
                if (!postingInventory.TryGetValue(line.ProductVariantId!.Value, out var inventory))
                    continue;
                var delta = -line.Quantity;
                inventory.AdjustQuantity(delta, line.IsReturn ? "Retail return" : "Retail sale");
                _db.InventoryTransactions.Add(new InventoryTransaction(statement.OrganizationId,
                    inventory.ProductVariantId,
                    line.IsReturn ? InventoryTransactionType.SaleReturn : InventoryTransactionType.SaleShipment,
                    delta, inventory.AverageCost, inventory.QuantityOnHand,
                    tx.TransactionNumber, tx.Id, $"Retail statement {statement.StatementNumber}",
                    "retail-statement"));
            }
        }

        journal.Post();
        if (invoice is not null)
        {
            invoice.Issue(journal.Id);
            invoice.ApplyPayment(statement.GrandTotal);
        }
        foreach (var tx in transactions)
            tx.MarkProcessed(invoice?.Id, journal.Id);
        statement.MarkPosted(invoice?.Id, creditNote?.Id, journal.Id);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Posted retail statement {Statement} with {Count} transaction(s)",
            statement.StatementNumber, statement.TransactionCount);
    }

    public async Task<List<RetailStatementDto>> GetStatementsAsync(Guid organizationId,
        int page = 1, int pageSize = 100, CancellationToken ct = default)
    {
        var stores = await _db.RetailStores.Where(s => s.OrganizationId == organizationId)
            .ToDictionaryAsync(s => s.Id, s => s.Name, ct);
        var rows = await _db.RetailStatements.Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.BusinessDate).ThenByDescending(s => s.CreatedAt)
            .Skip((Math.Max(1, page) - 1) * Math.Clamp(pageSize, 1, 500))
            .Take(Math.Clamp(pageSize, 1, 500)).ToListAsync(ct);
        return rows.Select(s => new RetailStatementDto(s.Id, s.StatementNumber, s.StoreId,
            stores.GetValueOrDefault(s.StoreId, "Unknown"), s.BusinessDate, s.Currency,
            s.Status.ToString(), s.TransactionCount, s.NetSales, s.DiscountTotal, s.TaxTotal,
            s.GrandTotal, s.CostTotal, s.ARInvoiceId, s.ARCreditNoteId, s.JournalEntryId, s.PostedAt,
            s.PostingError)).ToList();
    }

    public async Task<List<RetailSettlementDto>> GetSettlementsAsync(Guid organizationId,
        string? status = null, CancellationToken ct = default)
    {
        var query = _db.RetailTenderSettlements.Where(s => s.OrganizationId == organizationId);
        if (Enum.TryParse<RetailSettlementStatus>(status, true, out var parsed))
            query = query.Where(s => s.Status == parsed);
        var rows = await query.OrderByDescending(s => s.CreatedAt).Take(500).ToListAsync(ct);
        var statementNumbers = await _db.RetailStatements
            .Where(s => rows.Select(r => r.RetailStatementId).Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.StatementNumber, ct);
        return rows.Select(s => new RetailSettlementDto(s.Id, s.RetailStatementId,
            statementNumbers.GetValueOrDefault(s.RetailStatementId, "Unknown"),
            s.PaymentMethod.ToString(), s.Amount, s.Currency, s.Status.ToString(),
            s.ProcessorReference, s.BankTransactionId, s.SettledAt)).ToList();
    }

    public async Task SettleCardAsync(Guid settlementId, Guid bankAccountId, decimal merchantFee,
        string? processorReference, string postedBy, CancellationToken ct = default)
    {
        if (merchantFee < 0) throw new InvalidOperationException("Merchant fee cannot be negative.");
        var settlement = await _db.RetailTenderSettlements
            .FirstOrDefaultAsync(s => s.Id == settlementId, ct)
            ?? throw new InvalidOperationException("Settlement not found.");
        var bankAccount = await _db.BankAccounts.FirstOrDefaultAsync(b => b.Id == bankAccountId, ct)
            ?? throw new InvalidOperationException("Bank account not found.");
        var deposit = settlement.Amount - merchantFee;
        if (deposit <= 0) throw new InvalidOperationException("Merchant fee must be less than settlement amount.");

        var bankTx = new BankTransaction(settlement.OrganizationId, bankAccountId,
            $"CARD-{DateTime.UtcNow:yyyyMMddHHmmss}", DateTime.UtcNow,
            BankTransactionType.Deposit, deposit, "Credit card processor settlement",
            processorReference ?? settlement.ProcessorReference, "Card Processor");
        bankTx.Post(postedBy);
        _db.BankTransactions.Add(bankTx);
        bankAccount.AdjustBalance(deposit);

        var period = await _db.FiscalPeriods.Include(p => p.FiscalYear)
                .ThenInclude(y => y!.FiscalCalendar)
            .FirstOrDefaultAsync(p => p.FiscalYear!.OrganizationId == settlement.OrganizationId &&
                p.FiscalYear.FiscalCalendar!.IsDefault &&
                p.FiscalYear.Status == FiscalYearStatus.Open &&
                p.Status == FiscalPeriodStatus.Open &&
                p.StartDate.Date <= DateTime.UtcNow.Date &&
                p.EndDate.Date >= DateTime.UtcNow.Date, ct)
            ?? throw new InvalidOperationException("No open fiscal period exists for the settlement date.");
        var accounts = await LoadOrCreateAccountsAsync(settlement.OrganizationId, ct);
        var bankGlAccountId = bankAccount.GLAccountId ?? accounts["1120"].Id;
        var journalCount = await _db.JournalEntries.CountAsync(ct) + 1;
        var journal = new JournalEntry(settlement.OrganizationId, $"JE-{journalCount:D6}",
            DateTime.UtcNow.Date, period.Id, "Credit card processor settlement",
            processorReference ?? settlement.ProcessorReference ?? settlement.Id.ToString(),
            "CardSettlement", settlement.Currency);
        journal.AddLine(bankGlAccountId, "Processor deposit", deposit, 0m);
        if (merchantFee > 0)
            journal.AddLine(accounts["6900"].Id, "Merchant processing fee", merchantFee, 0m);
        journal.AddLine(accounts["1130"].Id, "Clear card receivable", 0m, settlement.Amount);
        journal.Post();
        _db.JournalEntries.Add(journal);

        settlement.Settle(bankTx.Id);
        await _db.SaveChangesAsync(ct);
    }

    private async Task<Dictionary<string, Account>> LoadOrCreateAccountsAsync(
        Guid organizationId, CancellationToken ct)
    {
        var required = new[] { "1110", "1120", "1130", "1210", "1310", "2210", "2310", "4100", "5100", "6900" };
        var accounts = await _db.Accounts.Where(a => a.OrganizationId == organizationId &&
            required.Contains(a.AccountNumber) && !a.IsHeaderAccount)
            .ToDictionaryAsync(a => a.AccountNumber, ct);
        if (!accounts.ContainsKey("1130") || !accounts.ContainsKey("2310"))
        {
            var assetType = await _db.AccountTypes.FirstAsync(t => t.Code == "ASSET", ct);
            var liabilityType = await _db.AccountTypes.FirstAsync(t => t.Code == "LIABILITY", ct);
            if (!accounts.ContainsKey("1130"))
            {
                var account = new Account(organizationId, "1130", "Credit Card Clearing", assetType.Id, false);
                _db.Accounts.Add(account);
                accounts["1130"] = account;
            }
            if (!accounts.ContainsKey("2310"))
            {
                var account = new Account(organizationId, "2310", "Gift Card Liability", liabilityType.Id, false);
                _db.Accounts.Add(account);
                accounts["2310"] = account;
            }
        }
        var missing = required.Where(n => !accounts.ContainsKey(n)).ToList();
        if (missing.Count > 0)
            throw new InvalidOperationException($"Missing retail posting account(s): {string.Join(", ", missing)}.");
        return accounts;
    }

    private static void AddSignedLine(JournalEntry journal, Guid accountId,
        string description, decimal amount, bool debitWhenPositive)
    {
        if (amount == 0) return;
        var debit = (amount > 0) == debitWhenPositive ? Math.Abs(amount) : 0m;
        var credit = debit == 0 ? Math.Abs(amount) : 0m;
        journal.AddLine(accountId, description, debit, credit);
    }

    private static async Task<ParsedTransaction> ParseAsync(Stream stream, CancellationToken ct)
    {
        var doc = await XDocument.LoadAsync(stream, LoadOptions.None, ct);
        var root = doc.Root ?? throw new InvalidOperationException("POSLog XML has no root element.");
        string Value(string name) => root.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value.Trim() ?? "";
        decimal Money(XElement? e) => decimal.TryParse(
            e?.Elements().FirstOrDefault(x => x.Name.LocalName == "value")?.Value ?? e?.Value,
            NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 0m;

        var totals = root.Descendants().Where(e => e.Name.LocalName == "Total")
            .GroupBy(e => (string?)e.Attribute("TotalType") ?? "")
            .ToDictionary(g => g.Key, g => Money(g.Last()));
        decimal Total(string key) => totals.GetValueOrDefault(key);

        var businessUnit = root.Descendants().FirstOrDefault(e => e.Name.LocalName == "BusinessUnit");
        var storeCode = businessUnit?.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "UnitID")?.Value.Trim();
        if (string.IsNullOrWhiteSpace(storeCode))
            throw new InvalidOperationException("POSLog BusinessUnit/UnitID is required.");

        var transactionId = Value("TransactionID");
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new InvalidOperationException("POSLog TransactionID is required.");

        var businessDate = DateTime.Parse(Value("BusinessDayDate"), CultureInfo.InvariantCulture).Date;
        var transactionDateText = Value("BeginDateTime");
        var transactionDate = DateTime.TryParse(transactionDateText, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal, out var parsedDate) ? parsedDate : businessDate;
        var currency = root.Descendants().Attributes("Currency")
            .Select(a => a.Value).FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? "USD";

        var lines = new List<ParsedLine>();
        foreach (var item in root.Descendants().Where(e => e.Name.LocalName == "LineItem" &&
            !string.Equals((string?)e.Attribute("CancelFlag"), "true", StringComparison.OrdinalIgnoreCase)))
        {
            var sale = item.Elements().FirstOrDefault(e => e.Name.LocalName == "Sale");
            var orderReturn = item.Elements().FirstOrDefault(e => e.Name.LocalName == "OrderReturn");
            var merchandise = sale ?? orderReturn;
            if (merchandise is null) continue;
            var isReturn = orderReturn is not null ||
                string.Equals((string?)item.Attribute("Action"), "Return", StringComparison.OrdinalIgnoreCase);
            XElement? Desc(string name) => merchandise.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == name);
            var quantity = Math.Abs(Money(Desc("Quantity")));
            if (quantity == 0) quantity = 1m;
            if (isReturn) quantity = -quantity;
            var sku = Desc("ItemID")?.Value.Trim();
            var posItemId = Desc("POSItemID")?.Value.Trim();
            sku = string.IsNullOrWhiteSpace(sku) ? posItemId : sku;
            if (string.IsNullOrWhiteSpace(sku)) continue;
            var productName = Desc("Description")?.Value.Trim();
            if (string.IsNullOrWhiteSpace(productName))
                productName = (string?)Desc("ItemID")?.Attribute("Name") ?? sku;
            var unitPrice = Math.Abs(Money(Desc("RegularSalesUnitPrice")));
            if (unitPrice == 0) unitPrice = Math.Abs(Money(Desc("ActualSalesUnitPrice")));
            var gross = quantity * unitPrice;
            var amounts = merchandise.Descendants().FirstOrDefault(e => e.Name.LocalName == "Amounts");
            var netElement = amounts?.Elements().FirstOrDefault(e =>
                e.Name.LocalName == "Amount" &&
                string.Equals((string?)e.Attribute("AmountType"), "NetAmount",
                    StringComparison.OrdinalIgnoreCase));
            var netMagnitude = Math.Abs(Money(netElement));
            if (netMagnitude == 0)
                netMagnitude = Math.Abs(Money(Desc("ExtendedAmountWithoutTax")));
            var net = isReturn ? -netMagnitude : netMagnitude;
            var discount = Math.Max(0m, Math.Abs(gross) - netMagnitude);
            var totalTax = merchandise.Descendants().FirstOrDefault(e =>
                e.Name.LocalName == "TotalTaxAmount");
            var taxMagnitude = Math.Abs(Money(totalTax));
            var tax = isReturn ? -taxMagnitude : taxMagnitude;
            var uom = (string?)Desc("Quantity")?.Attribute("UnitOfMeasureCode") ?? "EA";
            lines.Add(new ParsedLine(sku, posItemId, productName, quantity, unitPrice,
                discount, tax, gross, net + tax, uom, isReturn));
        }
        if (lines.Count == 0)
            throw new InvalidOperationException("POSLog contains no sale or return lines.");

        var grandTotal = Total("GrandTotalAmount");
        var isReturnTransaction = grandTotal < 0 || lines.All(l => l.IsReturn);
        var payments = root.Descendants().Where(e => e.Name.LocalName == "Tender")
            .Select(t =>
            {
                var tenderType = (string?)t.Attribute("TenderType") ?? "";
                var typeCode = (string?)t.Attribute("TypeCode") ?? "";
                var amount = Math.Abs(Money(t.Elements().FirstOrDefault(e => e.Name.LocalName == "Amount")));
                if (string.Equals(typeCode, "Return", StringComparison.OrdinalIgnoreCase)) amount = -amount;
                var method = tenderType switch
                {
                    "Cash" => POSPaymentMethod.Cash,
                    "GiftCard" => POSPaymentMethod.GiftCard,
                    "Debit" => POSPaymentMethod.DebitCard,
                    "CreditDebit" => POSPaymentMethod.CreditCard,
                    _ => POSPaymentMethod.Mixed
                };
                var reference = t.Descendants().FirstOrDefault(e =>
                    e.Name.LocalName is "ReferenceNumber" or "CardNumber" or "PanNumber")?.Value.Trim();
                return new ParsedPayment(method, amount, reference);
            }).ToList();

        var netTotal = Total("TransactionNetAmount");
        var discountTotal = Math.Abs(Total("TransactionPromotionalSavingsAmount")) +
            Math.Abs(Total("TotalDiscountAmount")) +
            Math.Abs(Total("TotalEmployeeDiscountAmount"));
        var subTotal = netTotal >= 0 ? netTotal + discountTotal : netTotal - discountTotal;
        var taxTotal = Total("TotalTaxCollected");
        if (grandTotal == 0) grandTotal = subTotal - Math.Sign(subTotal) * discountTotal + taxTotal;

        return new ParsedTransaction(storeCode, transactionId, Value("OperatorID"),
            businessDate, transactionDate, currency.ToUpperInvariant(), isReturnTransaction,
            subTotal, discountTotal, taxTotal, grandTotal, lines, payments);
    }

    private sealed record ParsedTransaction(string StoreCode, string TransactionId,
        string OperatorId, DateTime BusinessDate, DateTime TransactionDate, string Currency,
        bool IsReturn, decimal SubTotal, decimal DiscountTotal, decimal TaxTotal,
        decimal GrandTotal, List<ParsedLine> Lines, List<ParsedPayment> Payments);
    private sealed record ParsedLine(string Sku, string? PosItemId, string ProductName,
        decimal Quantity, decimal UnitPrice, decimal DiscountAmount, decimal TaxAmount,
        decimal LineSubTotal, decimal LineTotal, string UnitOfMeasure, bool IsReturn);
    private sealed record ParsedPayment(POSPaymentMethod Method, decimal Amount, string? Reference);
}
