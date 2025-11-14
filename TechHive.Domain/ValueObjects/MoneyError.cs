

namespace TechHive.Domain.ValueObjects;

public static class MonyErrors
{
    public static readonly Error Invalid = Error.Validation(
        "Money.Invalid",
        "Amount without currency or currency without amount is not valid.."
    );

    public static readonly Error AmountNegative = Error.Validation(
        "Money.AmountNegative",
        "Money amount cannot be negative."
    );

    public static readonly Error CurrencyNotFound = Error.Validation(
        "Money.CurrencyNotFound",
        "Money currency not found."
    );

    public static readonly Error CurrencyMismatch = Error.Validation(
        "Money.CurrencyMismatch",
        "Mony currency mismatched."
    );
}
