using LiteDB;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

public interface ICartServiceDbContext
{
    ILiteDatabase Database { get; }

    ILiteCollection<Cart> Carts { get; }
    ILiteCollection<Item> Items { get; }
}
