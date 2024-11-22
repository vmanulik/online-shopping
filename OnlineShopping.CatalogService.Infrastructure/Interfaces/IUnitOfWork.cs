namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}