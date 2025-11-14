
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Model;

namespace TechHive.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    public Task<Result<int>> AddAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product>> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
