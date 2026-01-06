using TechHive.Domain;
using TechHive.Domain.Base;
using TechHive.Domain.Results;

namespace TechHive.Model;

public class Category : Entity<int>
{
    private Category() : base(default)
    {
        Products = new List<Product>();
    }

    public Category(int id, string name) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Products = new List<Product>();
    }

    public string Name { get; private set; } = null!;

    public List<Product> Products { get; private set; } = new();

    public static Result<Category> Create(int id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Category>.Failure(
                Error.Validation("401", "Category.Name.Empty")
            );

        if (name.Length > 100) // مثلاً محدودیت طول
            return Result<Category>.Failure(
                Error.Validation("401", "Category.Name.TooLong")
            );

        return Result<Category>.Success(new Category(id, name));
    }

    public Result ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result.Failure(Error.Validation("401", "Category.Name.Empty"));

        Name = newName.Trim();
        return Result.Success();
    }
}