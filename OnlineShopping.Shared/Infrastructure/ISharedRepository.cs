namespace OnlineShopping.Shared.Infrastructure
{
    public interface ISharedRepository<T>
    {
        IQueryable<T> GetAllAsQueryable();
        Task<List<T>> GetAllAsync(CancellationToken cancellation = default);

        Task<T> GetByIdAsync(int id, CancellationToken cancellation = default);

        Task<int> AddAsync(T entity, CancellationToken cancellation = default);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task RemoveAsync(T entity, CancellationToken cancellation);
        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task SaveAsync();
    }
}
