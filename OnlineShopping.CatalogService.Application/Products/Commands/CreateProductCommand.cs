using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;

namespace OnlineShopping.CatalogService.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string? ImageUrl,
    string? ImageDescription,
    decimal Price,
    int CategoryId) : IRequest<int>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ISharedRepository<Product> _productRepository;

    public CreateProductCommandHandler(
            ISharedRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Price, request.CategoryId, request.ImageUrl, request.ImageDescription);

        return await _productRepository.AddAsync(product, cancellationToken);
    }
}
