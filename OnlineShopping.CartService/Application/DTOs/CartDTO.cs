namespace OnlineShopping.CartService.Application.DTOs;

public record CartDTO
{
    public Guid Id { get; private set; }
    public List<ItemDTO> Items { get; set; }
}
