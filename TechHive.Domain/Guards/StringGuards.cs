using System.Text.RegularExpressions;

namespace TechHive.Domain.Guards;

public static class StringGuards
{
    public static bool NotEmpty(this IGuardClause guard, string? value) =>
        string.IsNullOrEmpty(value);

    public static bool Length(this IGuardClause guard, string? value, int length) =>
        value is not null && value.Length == length;

    public static bool MinLength(this IGuardClause guard, string? value, int min) =>
        value is not null && value.Length >= min;

    public static bool MaxLength(this IGuardClause guard, string? value, int max) =>
        value is not null && value.Length <= max;

    public static bool Pattern(this IGuardClause guard, string? value, Regex pattern) =>
        value is not null && pattern.IsMatch(value);
}
