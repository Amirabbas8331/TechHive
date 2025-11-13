using ErrorOr;
namespace TechHive.Model;

public class Category
{
    public Category(string name,List<Product> products)
    {
        Name = name;
        Products = products;
    }
    public int Id { get;private set; }
    public string Name { get; private set; }
    public List<Product> Products { get; private set; } = new();
    public ErrorOr<Category> Create(string name,List<Product> products)
    {
        return new Category(name,products);
    }
}
