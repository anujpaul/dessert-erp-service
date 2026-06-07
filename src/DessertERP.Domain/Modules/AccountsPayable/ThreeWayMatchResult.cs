namespace DessertERP.Domain.Modules.AccountsPayable;

/// <summary>
/// Value object returned by <see cref="APInvoice.RunThreeWayMatch"/>.
/// Carries the match decision and the figures used to reach it — suitable for
/// logging, DTOs, and controller responses without exposing domain internals.
/// </summary>
public sealed record ThreeWayMatchResult(
    ThreeWayMatchStatus MatchStatus,
    decimal ReceivedValue,
    decimal PreviouslyInvoiced,
    decimal UninvoicedReceived,
    decimal InvoiceSubTotal,
    decimal VariancePct,
    decimal TolerancePct,
    bool QtyException,
    bool PriceException)
{
    public bool Passed => MatchStatus == ThreeWayMatchStatus.Matched;
}
