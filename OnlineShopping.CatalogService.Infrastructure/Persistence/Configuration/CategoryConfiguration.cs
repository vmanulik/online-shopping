using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastracture.Persistence.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.ParentCategory)
            .WithMany()
            .HasForeignKey(x => x.ParentCategoryId);

        builder
            .HasMany(x => x.SubCategories)
            .WithOne(t => t.ParentCategory)
            .HasForeignKey(e => e.ParentCategoryId);

        builder
            .HasMany(x => x.Products)
            .WithOne(t => t.Category)
            .HasForeignKey(e => e.CategoryId);
    }
}

