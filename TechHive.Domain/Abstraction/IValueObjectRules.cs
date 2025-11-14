using TechHive.Domain.Results;

namespace TechHive.Domain.Abstraction;

public interface IValueObjectRules<T>
{
 static abstract Result<T> Validate(T value);
}
