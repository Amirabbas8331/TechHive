using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Model;

namespace TechHive.Application.Products.Command.DeleteProducts;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IGenericRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.ProductId);
        await _unitOfWork.CommitChangesAsync();
        return Result.Success("Deleted");
    }
}
