

using MediatR;
using TechHive.Application.Common;
using TechHive.Model;

namespace TechHive.Application.Products.Query.GetProducts;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IGenericRepository<Product> _repository;


    public GetProductQueryHandler(IGenericRepository<Product> repository)
    {
        _repository = repository;

    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        return product.Value;
    }
}
