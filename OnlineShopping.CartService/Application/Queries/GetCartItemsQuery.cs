using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.API.Queries;

public record GetCartItemsQuery(Guid Id) : IRequest<List<ItemDTO>>;

public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, List<ItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly ILiteDbRepository<Cart> _cartRepository;

    public GetCartItemsQueryHandler(
        IMapper mapper,
        ILiteDbRepository<Cart> cartRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    public async Task<List<ItemDTO>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByGuidWithIncludeAsync(request.Id, x => x.Items);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        return _mapper.Map<List<ItemDTO>>(cart.Items);
    }
}
