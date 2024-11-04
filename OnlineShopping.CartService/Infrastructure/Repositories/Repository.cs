using LiteDB;
using LiteDB.Queryable;
using OnlineShopping.CartService.Infrastructure.Interfaces;
using OnlineShopping.Shared.Infrastructure;
using System.Linq.Expressions;

namespace OnlineShopping.CartService.Infrastructure.Repositories;

public class Repository<T> : ILiteDbRepository<T> where T : new()
{
    protected ICartServiceDbContext _dbContext;

    public Repository(ICartServiceDbContext cartServiceDbContext)
    {
        _dbContext = cartServiceDbContext;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .AsQueryable()
            .ToList();
    }  

    public IQueryable<T> GetAllAsQueryable()
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .AsQueryable();
    }

    public Task<T> GetByIdAsync(int id)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .FindByIdAsync(id);
    }

    public Task<T> GetByGuidAsync(Guid id)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .FindByIdAsync(id);
    }

    public Task<T> GetByGuidWithIncludeAsync<K>(Guid id, Expression<Func<T, K>> includeClause)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .Include(includeClause)
            .FindByIdAsync(id);
    }

    public Task AddAsync(T entity)
    {
        return _dbContext.Database
            .GetCollection<T>(nameof(T))
            .InsertAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        return _dbContext.Database
                .GetCollection<T>(nameof(T))
                .UpdateAsync(entity);
    }

    public Task RemoveByIdAsync(int id)
    {
        return _dbContext.Database
                .GetCollection<T>(nameof(T))
                .DeleteAsync(id);
    } 
    
    public Task RemoveByGuidAsync(Guid id)
    {
        return _dbContext.Database
                .GetCollection<T>(nameof(T))
                .DeleteAsync(id);
    }
}
