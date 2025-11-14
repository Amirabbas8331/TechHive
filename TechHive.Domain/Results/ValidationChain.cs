namespace TechHive.Domain.Results;

//? class, record, readonly struct
public readonly struct ValidationChain
{
    public static Result<T> For<T>(T value) => Result.Success(value);
}
