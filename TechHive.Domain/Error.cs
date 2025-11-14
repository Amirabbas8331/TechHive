namespace TechHive.Domain;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error Unexpected = new(
        "Error.Unexpected",
        "Unexpected error occurred.",
        ErrorType.Problem
    );

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    public Error? InnerError { get; }

    private const string Separator = ":";

    public Error(string code, string description, ErrorType type, Error? innerError = null)
    {
        Code = code;
        Description = description;
        Type = type;
        InnerError = innerError;
    }

    public Error WithInner(Error inner) =>
        new($"{inner.Code}.{Code}", $"{Description} ? {inner.Description}", Type, inner);

    public override string ToString() =>
        $"{Type} | {Code}{Separator} {Description}"
        + (InnerError is null ? "" : $" [Inner: {InnerError}]");

    public static implicit operator string(Error error) => error.Code;

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Business(string code, string description) =>
        new(code, description, ErrorType.Business);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);
}
