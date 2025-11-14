
using TechHive.Domain.Results;
using TechHive.Model;

namespace TechHive.Application.Common;

public interface IProductRepository
{
    Task<Result<int>> AddAsync(Product product);
    Task<Result> DeleteAsync(int id);
    Task<Result<Product>> UpdateAsync(Product product);
    Task<Result<Product>> GetByIdAsync(int id);


}
