using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);

    Task BeginTransactionAsync(CancellationToken cancellation = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);


    public ISharedRepository<Category> Categories { get; }

    public ISharedRepository<Product> Products { get; }

    public ISharedRepository<IntegrationEvent> Events { get; }
}