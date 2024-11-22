using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);

    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable, CancellationToken cancellation = default);
}