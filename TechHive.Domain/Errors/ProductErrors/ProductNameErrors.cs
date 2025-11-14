namespace TechHive.Domain.Errors.ProductErrors;

public static partial class ProductErrors
{
    public static class Name
    {
        public static readonly Error Empty = Error.Validation(
            "Product.Name.Empty",
            "Product name cannot be empty."
        );

        public static Error TooShort(int min) =>
            Error.Validation(
                "Product.Name.TooShort",
                $"Product name must be at least {min} characters long."
            );

        public static Error TooLong(int max) =>
            Error.Validation(
                "Product.Name.TooLong",
                $"Product name must not exceed {max} characters."
            );

        public static readonly Error InvalidCharacters = Error.Validation(
            "Product.Name.InvalidCharacters",
            "Product name contains invalid characters."
        );

        public static readonly Error LeadingOrTrailingSpace = Error.Validation(
            "Product.Name.LeadingOrTrailingSpace",
            "Product name must not contain leading or trailing spaces."
        );
    }
}
