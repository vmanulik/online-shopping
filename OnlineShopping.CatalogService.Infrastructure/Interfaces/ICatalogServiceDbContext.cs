using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface ICatalogServiceDbContext
{
    DatabaseFacade Database { get; }

    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}