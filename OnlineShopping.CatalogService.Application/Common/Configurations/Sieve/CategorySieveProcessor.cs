using Microsoft.Extensions.Options;
using OnlineShopping.CartService.Domain.Entities;
using Sieve.Models;
using Sieve.Services;

namespace OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;

public class CategorySieveProcessor : SieveProcessor
{
    public string DefaultSorting { get; } = "-id";

    public CategorySieveProcessor(
        IOptions<SieveOptions> options) :
        base(options)
    { }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Category>(p => p.Name)
            .CanSort()
            .CanFilter()
            .HasName("name");

        return mapper;
    }
}
