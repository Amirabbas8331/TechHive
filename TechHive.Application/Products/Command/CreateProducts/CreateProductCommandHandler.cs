using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Abstraction;
using TechHive.Domain.Enums;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Command.CreateProducts;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductId>>
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IIdGenerator<long> generator;


    public CreateProductCommandHandler(IGenericRepository<Product> repository, IIdGenerator<long> generator)
    {
        _repository = repository;
        this.generator = generator;

    }

    public async Task<Result<ProductId>> Handle(
    CreateProductCommand request,
    CancellationToken cancellationToken)
    {
        var nameResult = ProductName.Create(request.Name);
        if (nameResult.IsFailure)
            return Result.Failure<ProductId>(nameResult.Error);

        var codeResult = ProductCode.Create(request.Code);
        if (codeResult.IsFailure)
            return Result.Failure<ProductId>(codeResult.Error);

        var currencyResult = Currency.FromCode(request.PriceCurrency);
        if (currencyResult.IsFailure)
            return Result.Failure<ProductId>(currencyResult.Error);

        var moneyResult = Money.Create(
            request.PriceAmount,
            currencyResult.Value
        );

        if (moneyResult.IsFailure)
            return Result.Failure<ProductId>(moneyResult.Error);


        var productResult = Product.Create(
            nameResult.Value.Value,
            codeResult.Value.Value,
            moneyResult.Value.Amount,
            moneyResult.Value.Currency.Code,
            request.Description ?? string.Empty,
            generator
        );

        if (productResult.IsFailure)
            return Result.Failure<ProductId>(productResult.Error);

        await _repository.AddAsync(productResult.Value);

        return Result.Success(productResult.Value.Id);
    }

}