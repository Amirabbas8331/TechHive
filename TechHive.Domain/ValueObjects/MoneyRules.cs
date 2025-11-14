
using TechHive.Domain.Abstraction;
using TechHive.Domain.Enums;
using TechHive.Domain.Guards;
using TechHive.Domain.Guardss;
using TechHive.Domain.Results;

namespace TechHive.Domain.ValueObjects;

public sealed class MoneyRules : IValueObjectRules<(decimal? amount, string? currencyCode)>
{
    private MoneyRules() { }

    public static Result<(decimal? amount, string? currencyCode)> Validate(
        (decimal? amount, string? currencyCode) value
    ) =>
        ValidationChain
            .For(value)
            .Ensure(
                v =>
                    (v.amount is null && v.currencyCode is null)
                    || (v.amount is not null && v.currencyCode is not null),
                MonyErrors.Invalid
            )
            .Ensure(v => Guard.Against.Positive(v.amount), MonyErrors.AmountNegative)
            .Ensure(v => Currency.FromCode(v.currencyCode!).IsSuccess, MonyErrors.CurrencyNotFound);
}
