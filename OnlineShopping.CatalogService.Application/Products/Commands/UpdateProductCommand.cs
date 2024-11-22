using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using Shared.Domain.Exceptions;
using System.Data;

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
    private readonly ISharedRepository<Product> _productsRepository;
    private readonly ISharedRepository<Category> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        ISharedRepository<Product> productsRepository,
        ISharedRepository<Category> categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productsRepository = productsRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetByIdAsync(request.Id, cancellationToken); 
        if (product == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Product)}");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Category ID {request.CategoryId} was not found in the {nameof(Category)}");
        }

        // SQLite has only 2 type of transactions: serialazable and readuncommited,
        // so here the latter one used to avoid blocks. Dirty reads should not be a problem in this cases
        using (var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken))
        {
            try
            { 
                product.Update(request.Name, request.ImageUrl, request.ImageDescription, request.Price, request.CategoryId);

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }
    }
}
