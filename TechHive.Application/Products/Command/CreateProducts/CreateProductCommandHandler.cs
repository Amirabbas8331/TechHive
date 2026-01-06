using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Abstraction;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Command.CreateProducts;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductId>>
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdGenerator<long> generator;

    public CreateProductCommandHandler(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IIdGenerator<long> generator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        this.generator = generator;
    }

    public async Task<Result<ProductId>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        var productResult = Product.Create(
        name: request.Name.Value,
        code: request.Code.Value,
        price: request.Price?.Amount,
        currencyCode: request.Price?.Currency.Code,
        description: request.Description ?? string.Empty,
        generator: generator
    );

        if (productResult.IsFailure)
        {
            return Result.Failure<ProductId>(productResult.Error);
        }

        var product = productResult.Value;
        await _repository.AddAsync(product);
        await _unitOfWork.CommitChangesAsync();
        return Result.Success(product.Id);
    }
}