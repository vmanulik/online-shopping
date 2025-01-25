using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Interfaces;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductQuery(int Id) : IRequest<ProductDTO>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDTO>
{
    private readonly IMapper _mapper;
    private readonly ILinksService<ProductDTO> _linksService;
    private readonly ISharedRepository<Product> _productRepository;

    public GetProductQueryHandler(
        IMapper mapper,
        ILinksService<ProductDTO> linksService,
        ISharedRepository<Product> productRepository)
    {
        _mapper = mapper;
        _linksService = linksService;
        _productRepository = productRepository;
    }

    public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product ID {request.Id} was not found in the {nameof(Product)}");
        }

        var dto = _mapper.Map<ProductDTO>(product);
        var dtoWithLinks = _linksService.CreateLinks(dto, nameof(Product));

        return dtoWithLinks;
    }
}
