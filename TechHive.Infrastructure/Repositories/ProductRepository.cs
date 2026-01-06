
using Microsoft.EntityFrameworkCore;
using TechHive.Application.Common;
using TechHive.Context;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Infrastructure.Repositories;

public class ProductRepository : IGenericRepository<Product>
{
    private readonly ShopDbConext _context;
    public ProductRepository(ShopDbConext context)
    {
        _context = context;
    }
    public async Task<Result> DeleteAsync(ProductId productId)
    {
        var product = await _context.products.FirstOrDefaultAsync(x => x.Id == productId);
        if (product is null) return Result.Failure(Domain.Error.NotFound("404", "Entity not found."));
        _context.products.Remove(product);
        await _context.SaveChangesAsync();
        return Result.Success(productId);
    }


    public async Task<Result<Product>> GetByIdAsync(ProductId productId)
    {
        var product = await _context.products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == productId);
        if (product is null) return Result<Product>.Failure(Domain.Error.NotFound("404", "Entity not found."));
        return Result.Success(product);
    }

    public async Task<Result> UpdateAsync(Product entity)
    {
        _context.products.Update(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity.Status);

    }

    public async Task<Result<ProductId>> AddAsync(Product entity)
    {
        await _context.products.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity.Id);
    }
}
