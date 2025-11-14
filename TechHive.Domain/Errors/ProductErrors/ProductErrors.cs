namespace TechHive.Domain.Errors.ProductErrors;

public static partial class ProductErrors
{
    public static Error InvalidState =>
        Error.Validation("Product.InvalidState", $"Invalid state transition.");
}
