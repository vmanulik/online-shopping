using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using System.Data;

namespace OnlineShopping.CatalogService.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly CatalogServiceDbContext _context;

    public UnitOfWork(CatalogServiceDbContext context)
    {
        _context = context;
    }

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
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}