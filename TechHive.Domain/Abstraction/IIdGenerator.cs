
namespace TechHive.Domain.Abstraction;

public interface IIdGenerator<out T>
{
    T NewId();
}
