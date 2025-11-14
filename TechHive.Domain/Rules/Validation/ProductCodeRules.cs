
using System.Text.RegularExpressions;
using TechHive.Domain.Base;
using TechHive.Domain.Errors.ProductErrors;
using TechHive.Domain.Guards;
using TechHive.Domain.Guardss;
using TechHive.Domain.Results;

namespace TechHive.Domain.Rules.Validation;

public sealed class ProductCodeRules : IValueObjectRules<string>
{
    private ProductCodeRules() { }

    private const int CodeLength = 10;
    private static readonly Regex CodePattern = new(
        @"^[A-Za-z0-9_-]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );


   public static Result<string> Validate(string value)=>
        ValidationChain
            .For(value)
            .Ensure(Guard.Against.NotEmpty,ProductErrors.Code.Empty)
            .Ensure(c => Guard.Against.Length(c, CodeLength), ProductErrors.Code.Empty)
            .Ensure(c => Guard.Against.Pattern(c, CodePattern), ProductErrors.Code.Empty);
    
}
