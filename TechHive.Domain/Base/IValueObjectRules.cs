
using TechHive.Domain.Results;

namespace TechHive.Domain.Base;

public interface IValueObjectRules<T>
{
  public static abstract Result<T> Validate(T value);
}
