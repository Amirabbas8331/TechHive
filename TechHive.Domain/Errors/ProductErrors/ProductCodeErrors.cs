namespace TechHive.Domain.Errors.ProductErrors;

public static partial class ProductErrors
{
    public static class Code
    {
        public static readonly Error Empty = Error.Validation(
            "Product.Code.Empty",
            "Product code is required."
        );

        public static Error TooShort(int min) =>
            Error.Validation(
                "Product.Code.TooShort",
                $"Product code must be at least {min} characters."
            );

        public static Error TooLong(int max) =>
            Error.Validation(
                "Product.Code.TooLong",
                $"Product code must be at most {max} characters."
            );

        public static readonly Error InvalidFormat = Error.Validation(
            "Product.Code.InvalidFormat",
            "Product code can only contain uppercase letters, digits, or hyphens."
        );

        public static readonly Error LeadingOrTrailingSpace = Error.Validation(
            "Product.Code.LeadingOrTrailingSpace",
            "Product code must not contain leading or trailing spaces."
        );
    }
}
