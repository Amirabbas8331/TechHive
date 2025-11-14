

namespace TechHive.Domain.Results;

public static class ResultExtensions
{
    public static Result<TIn> Ensure<TIn>(
        this Result<TIn> result,
        Func<TIn, bool> predicate,
        Error error
    )
    {
        if (result.IsFailure)
            return result;

        var isValid = predicate(result.Value);

        if (isValid)
            return result;

        return Result<TIn>.Failure(error);
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> map)
    {
        return result.IsSuccess
            ? Result<TOut>.Success(map(result.Value))
            : Result<TOut>.Failure(result.Error);
    }
}
