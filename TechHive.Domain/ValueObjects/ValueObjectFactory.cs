
using TechHive.Domain.Results;

namespace TechHive.Domain.ValueObjects;

public class ValueObjectFactory
{
    protected ValueObjectFactory() { }

    public static Result<TVO> Create<TVO, T>(
        T value,
        Func<T, Result<T>> validate,
        Func<T, TVO> creator
    )
    {
        var isValid = validate(value);

        return isValid.IsFailure
            ? Result.Failure<TVO>(isValid.Error)
            : Result.Success(creator(value));
    }
}
