using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Interfaces;

namespace OnlineShopping.CatalogService.Application.Common.Services;

public class LinksService<T> : ILinksService<T> where T : WithLinksDTO
{
    private LinkGenerator _linkGenerator { get; set; }
    private IHttpContextAccessor _httpContextAccessor { get; set; }

    public LinksService(
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public T CreateLinks(T entity, string type)
    {
        var links = new List<LinkDTO>()
        {
            new LinkDTO(
                _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, $"Get{type}", values: new { id = entity.Id }),
                "self",
                "GET"),
            new LinkDTO(
                _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, $"Update{type}", values: new { entity.Id })!,
                "update_product",
                "PUT"),
            new LinkDTO(
                _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, $"Delete{type}", values : new { entity.Id })!,
                "delete_product",
                "DELETE")
        };
        entity.Links.AddRange(links);

        return entity;
    }
}
