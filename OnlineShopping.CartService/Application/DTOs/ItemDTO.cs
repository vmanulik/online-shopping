namespace OnlineShopping.CartService.Application.DTOs;

public record ItemDTO
{
    public Guid CartId { get; private set; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageDescription { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
}
