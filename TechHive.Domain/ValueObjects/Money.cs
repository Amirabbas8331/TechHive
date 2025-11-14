
using TechHive.Domain.Enums;
using TechHive.Domain.Guards;
using TechHive.Domain.Guardss;
using TechHive.Domain.Results;

namespace TechHive.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, Currency currency) =>
        ValidationChain
            .For(amount)
            .Ensure(a => Guard.Against.Positive(amount), MonyErrors.AmountNegative)
            .Map(a => new Money(amount, currency));

    public static Result<Money?> CreateOptional(decimal? amount, string? currencyCode)
    {
        var validateResult = MoneyRules.Validate((amount, currencyCode));

        return validateResult.IsFailure
            ? Result<Money?>.Failure(validateResult.Error)
            : Result.Success(
                (Money?)Money.Create(amount!.Value, Currency.FromCode(currencyCode!).Value).Value
            );
    }
}
