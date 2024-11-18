using Microsoft.EntityFrameworkCore.Storage;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using System.Data;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);

    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellation = default);


    public ISharedRepository<Category> Categories { get; }

    public ISharedRepository<Product> Products { get; }

    public ISharedRepository<IntegrationEvent> Events { get; }
}