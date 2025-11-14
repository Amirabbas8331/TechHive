

using TechHive.Domain.Base;
using TechHive.Domain.Errors.ProductErrors;
using TechHive.Domain.Guards;
using TechHive.Domain.Guardss;
using TechHive.Domain.Results;
using static TechHive.Domain.Errors.ProductErrors.ProductErrors;

namespace TechHive.Domain.Rules.Validation;

public class ProductNameRules : IValueObjectRules<string>
{
    private const int Max = 30;
    private const int Min = 3;

    protected ProductNameRules() { }

    static Result<string> IValueObjectRules<string>.Validate(string value) =>
         ValidationChain
            .For(value)
            .Ensure(Guard.Against.NotEmpty, Name.Empty)
            .Ensure(c => Guard.Against.MinLength(c, Min), Name.TooShort(Min))
            .Ensure(c => Guard.Against.MaxLength(c, Max), Name.TooLong(Max));
}
