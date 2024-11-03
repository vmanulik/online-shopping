namespace OnlineShopping.CatalogService.Application.Categories.DTOs;

public record CategoryDTO
{
    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }

    public int? ParentCategoryId { get; private set; }
}
