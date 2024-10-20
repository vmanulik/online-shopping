using LiteDB;
using LiteDB.Async;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

public interface ICartServiceDbContext
{
    ILiteDatabaseAsync Database { get; }

    ILiteCollectionAsync<Cart> Carts { get; }
    ILiteCollectionAsync<Item> Items { get; }
}
