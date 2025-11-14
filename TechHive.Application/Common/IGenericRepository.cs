
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Common;

public interface IGenericRepository<T>
{
    Task<Result<ProductId>> AddAsync(T entity);
    Task<Result> DeleteAsync(ProductId productId);
    Task<Result> UpdateAsync(T entity);
    Task<Result<T>> GetByIdAsync(ProductId productId);
}
