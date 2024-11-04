using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastracture.Persistence.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Category)
            .WithMany(t => t.Products)
            .HasForeignKey(e => e.CategoryId);
    }
}

