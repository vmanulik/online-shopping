using LiteDB;
using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Item : BaseEntity
{
    [BsonId]
    public Guid CartId { get; private set; }

    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageDescription { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }

    public Item() { }

    public void SetQuantity(int quantity) => Quantity = quantity;

    public void SetCart(Guid cartId) => CartId = cartId;

    public void IncrementQuantity() => Quantity++;

    public void Change(string name, string? imageUrl, string? imageDescription, decimal price, int categoryId)
    {
        Name = name;
        Price = price;
        ImageUrl = imageUrl;
        ImageDescription = imageDescription;
        Price = price;
        CategoryId = categoryId;
    }
}
