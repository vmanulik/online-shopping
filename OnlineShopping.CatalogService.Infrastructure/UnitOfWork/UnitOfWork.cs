using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using System.Data;

namespace OnlineShopping.CatalogService.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ICatalogServiceDbContext _context;

    public UnitOfWork(
        ICatalogServiceDbContext context,
        ISharedRepository<Category> categoryRepository,
        ISharedRepository<Product> productsRepository,
        ISharedRepository<IntegrationEvent> integrationEventsRepository)
    {
        _context = context;
        Categories = categoryRepository;
        Products = productsRepository;
        Events = integrationEventsRepository;
    }

    public ISharedRepository<Category> Categories { get; }

    public ISharedRepository<Product> Products { get; }

    public ISharedRepository<IntegrationEvent> Events { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return await _context.SaveChangesAsync(cancellation);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellation = default)
    {
        return await _context.Database.BeginTransactionAsync(isolationLevel, cancellation);
    }

    #region IDisposable

    private bool _disposed;

    protected void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}