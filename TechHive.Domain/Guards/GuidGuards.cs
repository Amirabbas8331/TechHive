

namespace TechHive.Domain.Guards;

public static class GuidGuards
{
    public static bool NotEmpty(this IGuardClause guard, Guid? value) =>
        value is not null && !Guid.Empty.Equals(value);
}
