using Microsoft.EntityFrameworkCore;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Infrastructure.Repositories;

public class Repository<T> : ISharedRepository<T> where T : BaseEntity
{
    protected CatalogServiceDbContext _context;
    protected readonly DbSet<T> _set;

    public Repository(CatalogServiceDbContext context)
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public async Task<int> AddAsync(T entity, CancellationToken cancellation = default)
    {
        var addedEntity = await _set.AddAsync(entity, cancellation);
        await _context.SaveChangesAsync(cancellation);

        return addedEntity.Entity.Id;
    }
    
    public void AddWithoutSave(T entity, CancellationToken cancellation = default)
    {
        _set.Add(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _set.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public void AddRangeWithoutSave(IEnumerable<T> entities)
    {
        _set.AddRange(entities);
    }

    public IQueryable<T> GetAllAsQueryable() => _set;

    public async Task<List<T>> GetAllAsync(CancellationToken cancellation = default)
    {
        return await _set.ToListAsync(cancellation);
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        var dbItem = await _set.FirstOrDefaultAsync(x => x.Id == id, cancellation);

        return dbItem ?? throw new NotFoundException($"{typeof(T).Name} not found by ID: {id}.");
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellation)
    {
        _set.Remove(entity);
        await _context.SaveChangesAsync(cancellation);
    }

    public void RemoveWithoutSave(T entity, CancellationToken cancellation)
    {
        _set.Remove(entity);
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        _set.RemoveRange(entities);
        await _context.SaveChangesAsync();
    } 

    public void RemoveRangeWithoutSave(IEnumerable<T> entities)
    {
        _set.RemoveRange(entities);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
