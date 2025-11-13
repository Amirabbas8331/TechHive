using ErrorOr;

namespace RazorTemplate.Model;

public class Product
{
    public Product(string name,int price)
    {
        Name = name;
        Price = price;
    }
    public int Id { get;private set; }
    public string? Name { get; private set; }
    public int Price { get; private set; }
    public int CategoryId { get; private set; }
    public Category category { get; private set; }
    public ErrorOr<Product> Create(string name,int Price)
    {
        return new Product(name, Price);
    }
}
