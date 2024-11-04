using OnlineShopping.CatalogService.Application.Common.DTOs;

namespace OnlineShopping.CatalogService.Application.Categories.DTOs;

public abstract class WithLinksDTO : BaseDTO
{
    public List<LinkDTO> Links { get; private set; } = new();
}
