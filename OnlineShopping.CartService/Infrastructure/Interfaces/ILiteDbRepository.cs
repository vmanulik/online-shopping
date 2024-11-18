using System.Linq.Expressions;

namespace OnlineShopping.CartService.Infrastructure
{
    public interface ILiteDbRepository<T> where T : new()
    {
        Task<List<T>> GetAllAsync();

        IQueryable<T> GetAllAsQueryable();

        Task<T> GetByIdAsync(int id);

        Task<T> GetByGuidAsync(Guid id);

        Task<T> GetByGuidWithIncludeAsync<K>(Guid id, Expression<Func<T, K>> includeClause);

        Task AddAsync(T entity);

        Task RemoveByIdAsync(int id);

        Task RemoveByGuidAsync(Guid id);

        Task UpdateAsync(T entity);
    }
}
