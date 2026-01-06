using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Model;

namespace TechHive.Application.Products.Command.DeleteProducts;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IGenericRepository<Product> _repository;

    public DeleteProductCommandHandler(IGenericRepository<Product> repository)
    {
        _repository = repository;

    }
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.ProductId);
        return Result.Success("Deleted");
    }
}
