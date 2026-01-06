using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechHive.Model;

namespace TechHive.Infrastructure.Context;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
                  .ValueGeneratedNever();
        builder.Property(c => c.Name)
                  .HasMaxLength(100)
                  .IsRequired();
    }
}
