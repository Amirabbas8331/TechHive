namespace TechHive.Domain.Results;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private Result(bool success, Error error)
    {
        IsSuccess = success;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) => new(default!, false, error);

    public static Result<TResult> Combine<TResult>(
        Func<object?[], TResult> map,
        params object[] results
    )
    {
        var values = new List<object?>();
        foreach (var r in results)
        {
            dynamic dyn = r;
            if (dyn.IsFailure)
                return Result.Failure<TResult>(dyn.Error);
            values.Add(dyn.Value);
        }
        return Success(map(values.ToArray()));
    }

    public static Result Combine(params Result[] results)
    {
        foreach (var r in results)
        {
            if (r.IsFailure)
                return r;
        }
        return Success();
    }

    public override string ToString() => IsSuccess ? "Success" : $"Failure: {Error}";
}

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public Error Error { get; }

    internal Result(T value, bool success, Error error)
    {
        Value = value;
        IsSuccess = success;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value, true, Error.None);

    public static Result<T> Failure(Error error) => new(default!, false, error);

    public static implicit operator Result(Result<T> result) =>
        result.IsSuccess ? Result.Success() : Result.Failure(result.Error);

    public override string ToString() => IsSuccess ? $"Success: {Value}" : $"Failure: {Error}";
}
