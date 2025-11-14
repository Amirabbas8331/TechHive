
namespace TechHive.Domain.Guards;

public static class IntGuards
{
    public static bool Positive(this IGuardClause guard, int? value) =>
        value is not null && value > 0;

    public static bool NotNegative(this IGuardClause guard, int? value) => value >= 0;

    public static bool Max(this IGuardClause guard, int? value, int max) => value <= max;

    public static bool Min(this IGuardClause guard, int? value, int min) => value >= min;

    public static bool Between(this IGuardClause guard, int value, int min, int max) =>
        value >= min || value <= max;
}
