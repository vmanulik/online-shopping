using OnlineShopping.CartService.Infrastructure.Persistence.Common;

namespace OnlineShopping.CartService.Domain.Entities;

public class Item : BaseLiteDbEntity
{
    public int ExternalId { get; init; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageDescription { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; private set; }

    public Item() { }

    public Item(int externalId, string name, decimal price)
    {
        ExternalId = externalId;
        Name = name;
        Price = price;
        Quantity = 1;
    }

    public void SetQuantity(int quantity) => Quantity = quantity;

    public void IncrementQuantity() => Quantity++;
}
