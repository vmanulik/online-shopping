using LiteDB;
using LiteDB.Queryable;
using OnlineShopping.CartService.Infrastructure.Persistence.Common;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.Infrastructure.Repositories;

public class Repository<T> : ISharedRepository<T> where T : BaseLiteDbEntity
{
    private ICartServiceDbContext _dbContext;

    public Repository(ICartServiceDbContext cartServiceDbContext)
    {
        _dbContext = cartServiceDbContext;
    }

    public IQueryable<T> GetAllAsQueryable()
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .AsQueryable();
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .AsQueryable()
            .ToList();
    }

    public Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .FindByIdAsync(id);
    }

    public Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .InsertAsync(entity);
    }

    public Task AddRangeAsync(IEnumerable<T> entities)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .InsertBulkAsync(entities);
    }

    public Task RemoveAsync(T entity, CancellationToken cancellationToken)
    {
        return _dbContext.Database
                .GetCollection<T>(nameof(T))
                .DeleteAsync(entity.Id);
    }

    public Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        var ids = entities.Select(e => e.Id);

        return _dbContext.Database
                .GetCollection<T>(nameof(T))
                .DeleteManyAsync(e => ids.Contains(e.Id));
    }
}
