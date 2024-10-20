using LiteDB.Async;
using Microsoft.Extensions.Options;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CartService.Infrastructure.Interfaces;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

namespace OnlineShopping.CartService.Infrastructure.Persistence;

public class CartServiceDbContext : ICartServiceDbContext
{
    public ILiteDatabaseAsync Database { get; }

    public CartServiceDbContext(IOptions<LiteDbOptions> options)
    {
        Database = new LiteDatabaseAsync(options.Value.DatabaseLocation);
    }

    public ILiteCollectionAsync<Cart> Carts => Database.GetCollection<Cart>("Cart");

    public ILiteCollectionAsync<Item> Items => Database.GetCollection<Item>("Item");
}
