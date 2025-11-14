

using TechHive.Domain.Guards;

namespace TechHive.Domain.Guardss;

public class Guard : IGuardClause
{
    private Guard() { }

    public static IGuardClause Against { get; set; } = new Guard();
}
