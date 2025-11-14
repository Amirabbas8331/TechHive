
using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Command.UpdateProducts;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductId>>
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IGenericRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<ProductId>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
       await _repository.UpdateAsync(request.Product);
        await _unitOfWork.CommitChangesAsync();
        return Result.Success(request.Product.Id);
    }
}
