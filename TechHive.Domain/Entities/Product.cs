using ErrorOr;
using TechHive.Domain.Base;
using TechHive.Domain.Enums;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;

namespace TechHive.Model;

public class Product:Entity<ProductId>
{

    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public Money? Price { get; private set; }
    public ProductStatus Status { get; private set; }
    public string? Description { get; set; }

    private Product(
        ProductId id,
        ProductName name,
        ProductCode code,
        Money? price,
        ProductStatus status,
        string? description = default
    )
        : base(id)
    {
        Name = name;
        Code = code;
        Price = price;
        Status = status;
        Description = description;
    }
    public static Result<Product> Create(
         string name,
         string code,
         decimal? price,
         string? currencyCode,
         string description,
         IIdGenerator<long> generator
     )
    {
        //? Chain (1) --> Result<ProductName>
        var createNameResult = ProductName.Create(name);
        if (createNameResult.IsFailure)
        {
            return Result.Failure<Product>(createNameResult.Error);
        }
        //? Chain (2) --> Result<ProductCode>
        var createCodeResult = ProductCode.Create(code);
        if (createCodeResult.IsFailure)
        {
            return Result.Failure<Product>(createCodeResult.Error);
        }

        //? Chain (3) --> Result<Money>
        var priceResult = Money.CreateOptional(price, currencyCode);
        if (priceResult.IsFailure)
        {
            return Result.Failure<Product>(priceResult.Error);
        }

        //? Refactor (ROP)
        var productId = ProductId.New(generator);

        return Result.Success(
            new Product(
                productId,
                createNameResult.Value,
                createCodeResult.Value,
                priceResult.Value,
                ProductStatus.Draft,
                description
            )
        );
    }

    // public Result ChangeStatus(ProductStatus status)
    // {
    //     return Result.Success();
    // }

    public Result Activate()
    {
        //? I1
        if (!Status.CanBeActivated)
            return Result.Failure(ProductErrors.InvalidState);

        //? I3
        if (Price is null)
        {
            return Result.Failure(ProductErrors.InvalidState);
        }

        Status = ProductStatus.Active;

        return Result.Success();
    }

    public Result UpdateName(string name)
    {
        var result = ProductName.Create(name);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Name = result.Value;

        return Result.Success();
    }

    public Result UpdateCode(string code)
    {
        var result = ProductCode.Create(code);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Code = result.Value;

        return Result.Success();
    }
}
