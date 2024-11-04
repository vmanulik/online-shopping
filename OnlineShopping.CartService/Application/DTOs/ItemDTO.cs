namespace OnlineShopping.CartService.Application.DTOs;

public record ItemDTO
{
    public Guid CartId { get; private set; }

    public int ItemsQuantity { get; private set; }
}
