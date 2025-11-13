using Microsoft.EntityFrameworkCore;
using TechHive.Application.Common;
using TechHive.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace TechHive.Context;

public class ShopDbConext:IdentityDbContext,IUnitOfWork
{
    public DbSet<Product> products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public ShopDbConext(DbContextOptions<ShopDbConext> options):base(options)
    {

    }
   
    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
