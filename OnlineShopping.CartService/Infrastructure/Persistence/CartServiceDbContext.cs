using LiteDB;
using Microsoft.Extensions.Options;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

namespace OnlineShopping.CartService.Infrastructure.Persistence;

public class CartServiceDbContext : ICartServiceDbContext
{
    public ILiteDatabase Database { get; }

    public CartServiceDbContext(IOptions<LiteDbOptions> options)
    {
        Database = new LiteDatabase(options.Value.DatabaseLocation);
    }

    public ILiteCollection<Cart> Carts => Database.GetCollection<Cart>("Cart");

    public ILiteCollection<Item> Items => Database.GetCollection<Item>("Item");
}
