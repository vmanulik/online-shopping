using OnlineShopping.CatalogService.Application.Categories.DTOs;

namespace OnlineShopping.CatalogService.Application.Common.Interfaces;

public interface ILinksService<T> where T : WithLinksDTO
{
    public T CreateLinks(T entity, string type);
}
