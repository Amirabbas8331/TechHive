

using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Model;

namespace TechHive.Application.Products.Query.GetProducts;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductQueryHandler(IGenericRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
      var product= await _repository.GetByIdAsync(request.ProductId);
        await _unitOfWork.CommitChangesAsync();
        return product.Value;
    }
}
