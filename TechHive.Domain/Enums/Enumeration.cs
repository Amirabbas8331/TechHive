using TechHive.Domain.Results;
namespace TechHive.Domain.Enums;

public abstract class Enumeration<T> : IEquatable<T>
where T : Enumeration<T>
{
    private static readonly Dictionary<string, T> _byCode = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<int, T> _byId = new();
    public int Id { get; }
    public string Code { get; }
    public string Name { get; }
    protected Enumeration(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
    protected static void Register(T item)
    {
        if (!_byCode.ContainsKey(item.Code))
            _byCode.Add(item.Code, item);
        if (!_byId.ContainsKey(item.Id))
            _byId.Add(item.Id, item);
    }
    public static IReadOnlyCollection<T> GetAll() => _byCode.Values;
    public static Result<T> FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result<T>.Failure(
            Error.Validation("Enumeration.EmptyCode", "Code cannot be empty.")
            );
        return _byCode.TryGetValue(code, out var value)
        ? Result<T>.Success(value)
        : Result<T>.Failure(
        Error.NotFound(
        $"{typeof(T).Name}.NotFound",
        $"Invalid code '{code}' for {typeof(T).Name}."
        )
        );
    }
    public static Result<T> FromId(int id)
    {
        return _byId.TryGetValue(id, out var value)
        ? Result<T>.Success(value)
        : Result<T>.Failure(
        Error.NotFound(
        $"{typeof(T).Name}.NotFound",
        $"Invalid id '{id}' for {typeof(T).Name}."
        )
        );
    }
    public override bool Equals(object? obj) => obj is T other && Equals(other);
    public bool Equals(T? other) =>
    other != null && Code.Equals(other.Code, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() => Code.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Name;
}