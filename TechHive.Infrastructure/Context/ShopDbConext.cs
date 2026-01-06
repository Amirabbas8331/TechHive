
using Microsoft.EntityFrameworkCore;
using TechHive.Application.Common;
using TechHive.Domain.Enums;
using TechHive.Domain.ValueObjects;
using TechHive.Model;
namespace TechHive.Context;

public class ShopDbConext : DbContext, IUnitOfWork
{
    public DbSet<Product> products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public ShopDbConext(DbContextOptions<ShopDbConext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {

            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                      .HasConversion(
                          productId => productId.Value,
                          value => new ProductId(value)
                      )
                      .ValueGeneratedNever()
                      .HasColumnName("Id");

            builder.Property(p => p.Name)
                      .HasConversion(name => name.Value, value => new ProductName(value))
                      .HasColumnName("Name")
                      .HasMaxLength(200)
                      .IsRequired();

            builder.Property(p => p.Code)
                      .HasConversion(code => code.Value, value => new ProductCode(value))
                      .HasColumnName("Code")
                      .HasMaxLength(50)
                      .IsRequired();

            builder.Property(p => p.Description)
                      .HasMaxLength(1000)
                      .IsRequired(false);


            builder.Property(p => p.Status)
        .HasConversion(
            status => status.Code,
            code => ProductStatus.FromCode(code).Value
        )
        .HasMaxLength(20)
        .IsRequired();

            builder.OwnsOne(p => p.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("PriceAmount")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();

                money.Property(m => m.Currency)
                     .HasColumnName("PriceCurrency")
                     .HasConversion(
                         c => c.Code,
                         code => Currency.FromCode(code).Value
                     )
                     .HasMaxLength(3)
                     .IsRequired();
            });
        });
        modelBuilder.Entity<Category>(builder =>
         {
             builder.ToTable("Categories");
             builder.HasKey(c => c.Id);
             builder.Property(c => c.Id)
                       .ValueGeneratedNever();
             builder.Property(c => c.Name)
                       .HasMaxLength(100)
                       .IsRequired();
         });


        base.OnModelCreating(modelBuilder);
    }
    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
