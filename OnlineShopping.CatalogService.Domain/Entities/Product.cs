using OnlineShopping.Shared.Domain.Entities;
using Sieve.Attributes;

namespace OnlineShopping.CartService.Domain.Entities;

public class Product : BaseEntity
{
    [Sieve(CanSort = true)]
    public string Name { get; private set; }

    public string? ImageUrl { get; private set; }
    public string? ImageDescription { get; private set; }
    public decimal Price { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }

    public Product() { }

    public Product(string name, decimal price, int categoryId, string? imageUrl, string? imageDescription)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        ImageDescription = imageDescription;
    }

    public void Update(
        string name,
        string? imageUrl,
        string? imageDescription,
        decimal price,
        int categoryId)
    {
        Name = name;
        ImageUrl = imageUrl;
        ImageDescription = imageDescription;
        Price = price;
        CategoryId = categoryId;
    }
}