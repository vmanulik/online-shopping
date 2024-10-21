using LiteDB;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;

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
        Cart cart = await _cartRepository.GetByGuidAsync(request.Id);
        if (cart == null)
        {
            cart = new Cart(request.Id);
            await _cartRepository.AddAsync(cart);

            request.Item.SetCart(cart.Id);
            await _itemRepository.AddAsync(request.Item);
        }
        else
        {
            var items = _itemRepository
                .GetAllAsQueryable()
                .Where(x => x.CartId == request.Id);
            cart.Items = items.ToList();

            cart.AddItem(request.Item);

            var item = cart.Items.Single(x => x.Id == request.Item.Id);
            await _itemRepository.UpdateAsync(item);
        }
    }
}
