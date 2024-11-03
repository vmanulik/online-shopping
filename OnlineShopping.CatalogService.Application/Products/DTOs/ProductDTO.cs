namespace OnlineShopping.CatalogService.Application.Categories.DTOs;

public record ProductDTO
{
    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? ImageDescription { get; private set; }
    public decimal Price { get; private set; }

    public int CategoryId { get; private set; }
}
