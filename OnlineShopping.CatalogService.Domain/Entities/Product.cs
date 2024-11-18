using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; private set; }

    public string? ImageUrl { get; private set; }
    public string? ImageDescription { get; private set; }
    public decimal Price { get; private set; }

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

        Created = DateTime.Now;
        Version = 1;
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

        LastModified = DateTime.Now;
        Version++;
    }
}