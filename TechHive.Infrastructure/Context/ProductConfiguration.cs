

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechHive.Model;

namespace eShop.Catalog.Infrastructure.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Name).IsRequired().IsUnicode().HasMaxLength(100);

        builder.Property(m => m.Price).IsRequired().HasPrecision(10, 3);

        builder.Property(m => m.Description).IsRequired(false).IsUnicode().HasMaxLength(250);
    }
}
