using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
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
        Cart cart = await _cartRepository.GetByGuidAsync(request.Id);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        var item = _itemRepository
            .GetAllAsQueryable()
            .Where(x => x.CartId == request.Id);
        if (item == null)
        {
            throw new NotFoundException($"Item ID {request.ItemId} was not found in the {nameof(item)}");
        }


        await _itemRepository.RemoveByIdAsync(request.ItemId);
    }
}
