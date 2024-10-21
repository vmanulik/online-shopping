using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Application.Exceptions;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductQuery(int Id) : IRequest<Product>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly ISharedRepository<Product> _productRepository;

    public GetProductQueryHandler(
            ISharedRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Product)}");
        }

        return product;
    }
}
