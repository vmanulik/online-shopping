using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Common.Interfaces;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductDetailsQuery(int Id) : IRequest<IDictionary<string, string>>;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, IDictionary<string, string>>
{
    private readonly IDictionarizeService<Product> _dictionarizeService;
    private readonly ISharedRepository<Product> _productRepository;

    public GetProductDetailsQueryHandler(
        IDictionarizeService<Product> dictionarizeService,
        ISharedRepository<Product> productRepository)
    {
        _dictionarizeService = dictionarizeService;
        _productRepository = productRepository;
    }

    public async Task<IDictionary<string, string>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product ID {request.Id} was not found in the {nameof(Product)}");
        }

        var dictionary = _dictionarizeService.CreateDictionary(product);

        return dictionary;
    }
}
