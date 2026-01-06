

using TechHive.Domain.Results;
using TechHive.Domain.Rules.Validation;

namespace TechHive.Domain.ValueObjects;

public record ProductName
{
    public string Value { get; }

    public ProductName(string value) => Value = value;

    public static Result<ProductName> Create(string value) =>
     ValueObjectFactory.Create(value, ProductNameRules.Validate, v => new ProductName(v));
}
