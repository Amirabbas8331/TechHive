

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechHive.Domain.Enums;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace eShop.Catalog.Infrastructure.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
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
                  .HasColumnName("StatusCode")
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

    }
}
