using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageDescription { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; private set; }

    public Item() { }

    public Item(int id, string name, decimal price) 
        : base(id)
    {
        Name = name;
        Price = price;
        Quantity = 1;
    }

    public void SetQuantity(int quantity) => Quantity = quantity;

    public void IncrementQuantity() => Quantity++;
}
