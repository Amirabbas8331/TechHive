

using TechHive.Domain.Results;
using TechHive.Domain.Rules.Validation;

namespace TechHive.Domain.ValueObjects;

public record ProductCode
{
    public string Value { get; }

    private ProductCode(string value) => Value = value;

    public static Result<ProductCode> Create(string value) =>
        ValueObjectFactory.Create(value, ProductCodeRules.Validate, v => new ProductCode(v));
}
