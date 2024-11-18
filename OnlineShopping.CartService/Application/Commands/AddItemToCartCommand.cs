using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CartService.Infrastructure;

namespace OnlineShopping.CartService.API.Commands;

public record AddItemToCartCommand(
    Guid Id,
    Item Item) : IRequest;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
{
    private readonly ILiteDbRepository<Cart> _cartRepository;
    private readonly ILiteDbRepository<Item> _itemRepository;

    public AddItemToCartCommandHandler(
            ILiteDbRepository<Cart> cartRepository,
            ILiteDbRepository<Item> itemRepository)
    {
        _cartRepository = cartRepository;
        _itemRepository = itemRepository;
    }

    public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        Cart cart = await _cartRepository.GetByGuidWithIncludeAsync(request.Id, x => x.Items);
        if (cart == null)
        {
            cart = new Cart(request.Id);

            request.Item.SetCart(cart.Id);
            cart.AddItem(request.Item);

            await _cartRepository.AddAsync(cart);
        }
        else
        {
            request.Item.SetCart(cart.Id);
            cart.AddItem(request.Item);

            await _cartRepository.UpdateAsync(cart);
        }
    }
}
