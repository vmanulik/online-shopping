using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CartService.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.API.Commands;

public record RemoveItemFromCartCommand(
    Guid Id,
    int ItemId) : IRequest;

public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand>
{
    private readonly ILiteDbRepository<Cart> _cartRepository;
    private readonly ILiteDbRepository<Item> _itemRepository;

    public RemoveItemFromCartCommandHandler(
            ILiteDbRepository<Cart> cartRepository,
            ILiteDbRepository<Item> itemRepository)
    {
        _cartRepository = cartRepository;
        _itemRepository = itemRepository;
    }

    public async Task Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByGuidWithIncludeAsync(request.Id, x => x.Items);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        cart.RemoveItem(request.ItemId);

        await _cartRepository.UpdateAsync(cart);
    }
}
