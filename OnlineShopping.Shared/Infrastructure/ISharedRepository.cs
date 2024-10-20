namespace OnlineShopping.Shared.Infrastructure
{
    public interface ISharedRepository<T>
    {
        IQueryable<T> GetAllAsQueryable();
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task RemoveAsync(T entity, CancellationToken cancellationToken);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
