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

    public CreateProductCommandHandler(IGenericRepository<Product> repository, IUnitOfWork unitOfWork,IIdGenerator<long> generator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        this.generator = generator;
    }

   public async Task<Result<ProductId>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
 
       var product= Product.Create(request.Name.ToString(), request.Code.ToString(), request?.Price?.Amount, request?.Price?.Currency.ToString(), request?.Description!,generator);
        await _repository.AddAsync(product.Value);
        await _unitOfWork.CommitChangesAsync();
        return Result.Success(product.Value.Id);
    }
}