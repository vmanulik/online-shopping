using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Application.Exceptions;

namespace OnlineShopping.CatalogService.Application.Categories.Commands;

public record UpdateCategoryCommand(
    int Id,
    string Name,
    int? ParentCategoryId,
    string? ImageUrl) : IRequest;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ISharedRepository<Category> _categoryRepository;

    public UpdateCategoryCommandHandler(
            ISharedRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Category)}");
        }

        category.Update(request.Name, request.ImageUrl, request.ParentCategoryId);

        await _categoryRepository.SaveAsync();
    }
}
