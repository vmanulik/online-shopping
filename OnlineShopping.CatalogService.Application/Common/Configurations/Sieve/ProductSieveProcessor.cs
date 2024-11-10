using Microsoft.Extensions.Options;
using OnlineShopping.CartService.Domain.Entities;
using Sieve.Models;
using Sieve.Services;

namespace OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;

public class ProductSieveProcessor : SieveProcessor
{
    public string DefaultSorting { get; } = "-id";

    public ProductSieveProcessor(
        IOptions<SieveOptions> options) :
        base(options)
    { }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Product>(p => p.Name)
            .CanSort()
            .HasName("name"); 
        
        mapper.Property<Product>(p => p.CategoryId)
            .CanSort()
            .CanFilter()
            .HasName("categoryId");

        return mapper;
    }
}
