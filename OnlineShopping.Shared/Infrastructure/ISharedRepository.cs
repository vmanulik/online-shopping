namespace OnlineShopping.Shared.Infrastructure
{
    public interface ISharedRepository<T>
    {
        IQueryable<T> GetAllAsQueryable();
        Task<List<T>> GetAllAsync(CancellationToken cancellation = default);

        Task<T> GetByIdAsync(int id, CancellationToken cancellation = default);

        Task<int> AddAsync(T entity, CancellationToken cancellation = default);
        void AddWithoutSave(T entity, CancellationToken cancellation = default);
        Task AddRangeAsync(IEnumerable<T> entities);
        void AddRangeWithoutSave(IEnumerable<T> entities);

        Task RemoveAsync(T entity, CancellationToken cancellation);
        void RemoveWithoutSave(T entity, CancellationToken cancellation);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        void RemoveRangeWithoutSave(IEnumerable<T> entities);

        Task SaveAsync();
    }
}
