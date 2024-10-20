namespace OnlineShopping.Shared.Infrastructure
{
    public interface ILiteDbRepository<T> where T : new()
    {
        Task<List<T>> GetAllAsync();

        IQueryable<T> GetAllAsQueryable();

        Task<T> GetByIdAsync(int id);

        Task<T> GetByGuidAsync(Guid id);

        Task AddAsync(T entity);

        Task RemoveByIdAsync(int id);

        Task RemoveByGuidAsync(Guid id);

        Task UpdateAsync(T entity);
    }
}
