using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Products.Commands;

public record DeleteProductCommand(
    int Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly ISharedRepository<Product> _productRepository;

    public DeleteProductCommandHandler(
            ISharedRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Product)}");
        }

        await _productRepository.RemoveAsync(product, cancellationToken);
    }
}
