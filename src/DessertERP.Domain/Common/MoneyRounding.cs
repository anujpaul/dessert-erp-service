namespace DessertERP.Domain.Common;

public enum MoneyRoundingMethod
{
    HalfUp,
    Bankers
}

public enum MoneyRoundingLevel
{
    Line,
    Document
}

public static class MoneyRounding
{
    public static decimal Round(decimal value, int decimalPlaces, MoneyRoundingMethod method)
    {
        var mode = method == MoneyRoundingMethod.Bankers
            ? MidpointRounding.ToEven
            : MidpointRounding.AwayFromZero;
        return Math.Round(value, decimalPlaces, mode);
    }

    public static decimal LineValue(
        decimal value,
        int decimalPlaces,
        MoneyRoundingMethod method,
        MoneyRoundingLevel level) =>
        level == MoneyRoundingLevel.Line ? Round(value, decimalPlaces, method) : value;
}
