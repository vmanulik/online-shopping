using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Products.Commands;

public record UpdateProductCommand(
    int Id,
    string Name,
    string? ImageUrl,
    string? ImageDescription,
    decimal Price,
    int CategoryId) : IRequest;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly ISharedRepository<Product> _productRepository;
    private readonly ISharedRepository<Category> _categoryRepository;

    public UpdateProductCommandHandler(
            ISharedRepository<Product> productRepository,
            ISharedRepository<Category> categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken); 
        if (product == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Product)}");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Category ID {request.CategoryId} was not found in the {nameof(Category)}");
        }

        product.Update(request.Name, request.ImageUrl, request.ImageDescription, request.Price, request.CategoryId);

        await _productRepository.SaveAsync();
    }
}
