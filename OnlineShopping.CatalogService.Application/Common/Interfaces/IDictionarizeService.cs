using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CatalogService.Application.Common.Interfaces;

public interface IDictionarizeService<T> where T : BaseEntity
{
    public IDictionary<string, string> CreateDictionary(T entity);
}
