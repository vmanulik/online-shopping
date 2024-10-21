using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageDescription { get; init; }
    public decimal Price { get; init; }

    public int CategoryId { get; set; }
    public Category Category { get; private set; }

    public Product() { }

    public Product(string name, decimal price, int categoryId)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }
}