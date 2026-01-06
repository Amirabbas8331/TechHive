using TechHive.Domain.Enums;

namespace TechHive.Domain;

// None = 0,
// Validation = 1,   // Rule or data validation failed
// Business = 2,     // Domain/business invariant broken
// NotFound = 3,     // Entity/resource not found
// Conflict = 4,     // State conflict or version mismatch
// Unauthorized = 5, // AuthN/AuthZ failure
// Problem = 6,      // Unexpected, technical, or infrastructure error

public sealed class ErrorType : Enumeration<ErrorType>
{
    public static readonly ErrorType None = new ErrorType(0, "NONE", nameof(None));
    public static readonly ErrorType Validation = new ErrorType(
        1,
        "VALIDATION",
        nameof(Validation)
    );
    public static readonly ErrorType Business = new ErrorType(2, "Business", nameof(Business));
    public static readonly ErrorType NotFound = new ErrorType(3, "NOTFOUND", nameof(NotFound));
    public static readonly ErrorType Conflict = new ErrorType(4, "CONFLICT", nameof(Conflict));
    public static readonly ErrorType Unauthorized = new ErrorType(
        5,
        "UnAUTHORIZED",
        nameof(Unauthorized)
    );
    public static readonly ErrorType Problem = new ErrorType(6, "PROBLEM", nameof(Problem));

    private ErrorType(int id, string code, string name)
        : base(id, code, name)
    {

    }
}
