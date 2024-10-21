using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Categories.Commands;

public record DeleteCategoryCommand(
    int Id) : IRequest;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ISharedRepository<Category> _categoryRepository;

    public DeleteCategoryCommandHandler(
            ISharedRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Category)}");
        }

        await _categoryRepository.RemoveAsync(category, cancellationToken);
    }
}
