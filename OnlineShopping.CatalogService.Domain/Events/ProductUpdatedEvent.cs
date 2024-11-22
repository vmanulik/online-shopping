using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Events;

namespace OnlineShopping.CartService.Domain.Events;

public class ProductUpdatedEvent : BaseEvent
{
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}
