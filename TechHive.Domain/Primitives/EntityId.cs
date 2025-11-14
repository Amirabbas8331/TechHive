
namespace TechHive.Domain.Primitives;

public record EntityId<TId>(TId value)
{
    public string Tostring() => value.ToString();
}
