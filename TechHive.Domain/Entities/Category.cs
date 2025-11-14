using TechHive.Domain.Base;
using TechHive.Domain.Results;

namespace TechHive.Model;

public class Category:Entity<int>
{
    public Category(int id,string name):base(id)
    {
        Name = name;
    }
    public string Name { get; private set; }
    public List<Product> Products { get; private set; } = new();
    public Result<Category> Create(string name)
    {
        return Result.Success(new Category(Id,name));
    }
}
