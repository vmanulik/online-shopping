using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.API.Queries;

public record GetCartInfoQuery(Guid Id) : IRequest<CartDTO>;

public class GetCartInfoQueryHandler : IRequestHandler<GetCartInfoQuery, CartDTO>
{
    private readonly IMapper _mapper;
    private readonly ILiteDbRepository<Cart> _cartRepository;

    public GetCartInfoQueryHandler(
        IMapper mapper,
        ILiteDbRepository<Cart> cartRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    public async Task<CartDTO> Handle(GetCartInfoQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByGuidWithIncludeAsync(request.Id, x => x.Items);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        var cartDTO = _mapper.Map<CartDTO>(cart);
        cartDTO.ItemsQuantity = cart.Items.Sum(x => x.Quantity);

        return cartDTO;
    }
}
