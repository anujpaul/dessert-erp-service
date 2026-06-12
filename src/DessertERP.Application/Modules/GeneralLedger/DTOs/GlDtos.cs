namespace DessertERP.Application.Modules.GeneralLedger.DTOs;

// ── Fiscal Year ───────────────────────────────────────────────────────────────
public record FiscalCalendarDto(
    Guid Id,
    string Name,
    string Description,
    string CalendarType,
    bool IsDefault,
    int FiscalYearCount,
    DateTime CreatedAt);

public record CreateFiscalCalendarRequest(
    string Name,
    string Description,
    string CalendarType,
    bool IsDefault = false);

public record FiscalYearDto(Guid Id, string Name, string Description,
    Guid FiscalCalendarId, string FiscalCalendarName,
    DateTime StartDate, DateTime EndDate, string CalendarType,
    string Status, int PeriodCount, DateTime CreatedAt);

public record FiscalPeriodDto(Guid Id, Guid FiscalYearId, int PeriodNumber,
    string Name, DateTime StartDate, DateTime EndDate, string Status);

public record CreateFiscalYearRequest(
    Guid FiscalCalendarId,
    string Name, string Description,
    DateTime StartDate, DateTime EndDate,
    bool AutoGeneratePeriods = true);

/// <summary>Manually add a single custom period.</summary>
public record CreatePeriodRequest(string Name, DateTime StartDate, DateTime EndDate);

/// <summary>Bulk-generate periods using a preset calendar pattern.</summary>
/// <param name="Type">Monthly | Quarterly | 4-4-5</param>
public record GeneratePeriodsRequest(string? Type = null);

/// <summary>Rename / re-date an existing Open period.</summary>
public record UpdatePeriodRequest(string Name, DateTime StartDate, DateTime EndDate);

// ── Chart of Accounts ─────────────────────────────────────────────────────────
public record AccountTypeDto(Guid Id, string Code, string Name, string Nature, int DisplayOrder);

public record AccountDto(Guid Id, string AccountNumber, string Name,
    string? Description, Guid AccountTypeId, string AccountTypeName,
    Guid? ParentAccountId, string? ParentAccountName,
    bool IsHeaderAccount, bool AllowManualEntry,
    string Status, string Currency, int Level);

public record CreateAccountRequest(
    string AccountNumber, string Name, Guid AccountTypeId,
    bool IsHeaderAccount, Guid? ParentAccountId = null,
    string? Description = null, string Currency = "USD");

// ── Journal Entry ─────────────────────────────────────────────────────────────
public record JournalLineDto(Guid Id, Guid AccountId, string AccountNumber,
    string AccountName, string Description, decimal Debit, decimal Credit, int LineOrder);

public record JournalEntryDto(Guid Id, string EntryNumber, DateTime EntryDate,
    Guid FiscalPeriodId, string FiscalPeriodName, string Description,
    string Reference, string JournalType, string Status, string Currency,
    decimal TotalDebit, decimal TotalCredit, DateTime CreatedAt,
    IReadOnlyList<JournalLineDto> Lines);

public record CreateJournalLineRequest(Guid AccountId, string Description,
    decimal Debit, decimal Credit);

public record CreateJournalEntryRequest(
    DateTime EntryDate, Guid FiscalPeriodId,
    string Description, string Reference,
    string JournalType = "General", string Currency = "USD",
    IReadOnlyList<CreateJournalLineRequest>? Lines = null);

// ── Currency ──────────────────────────────────────────────────────────────────
public record CurrencyDto(
    Guid     Id,
    string   Code,
    string   Name,
    string   Symbol,
    int      DecimalPlaces,
    decimal  ExchangeRate,
    bool     IsBase,
    bool     IsActive,
    int?     NumericCode,
    string?  Country,
    DateTime? RateUpdatedAt,
    DateTime CreatedAt);

public record CreateCurrencyRequest(
    string  Code,
    string  Name,
    string  Symbol,
    int     DecimalPlaces  = 2,
    decimal ExchangeRate   = 1m,
    bool    IsBase         = false,
    int?    NumericCode    = null,
    string? Country        = null);

public record UpdateCurrencyRequest(
    string  Name,
    string  Symbol,
    int     DecimalPlaces,
    string? Country,
    int?    NumericCode);

public record UpdateExchangeRateRequest(decimal ExchangeRate);

// ── Reports ───────────────────────────────────────────────────────────────────
public record TrialBalanceLineDto(string AccountNumber, string AccountName,
    string AccountType, decimal TotalDebit, decimal TotalCredit, decimal Balance);
