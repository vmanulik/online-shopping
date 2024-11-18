using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface ICatalogServiceDbContext : IDisposable
{
    DatabaseFacade Database { get; }

    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<IntegrationEvent> Events { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}