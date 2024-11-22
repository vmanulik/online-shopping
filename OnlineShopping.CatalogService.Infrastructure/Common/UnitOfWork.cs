using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;

namespace OnlineShopping.CatalogService.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
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
}