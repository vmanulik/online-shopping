using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Item : BaseEntity
{
    public Guid CartId { get; private set; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageDescription { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; set; }

    public Item() { }

    public Item(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = 1;
    }

    public void SetQuantity(int quantity) => Quantity = quantity;

    public void SetCart(Guid cartId) => CartId = cartId;

    public void IncrementQuantity() => Quantity++;
}
