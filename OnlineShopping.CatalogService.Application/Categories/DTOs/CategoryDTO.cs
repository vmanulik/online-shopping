using OnlineShopping.CatalogService.Application.Common.DTOs;

namespace OnlineShopping.CatalogService.Application.Categories.DTOs;

public class CategoryDTO : BaseDTO
{
    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }

    public int? ParentCategoryId { get; private set; }
}
