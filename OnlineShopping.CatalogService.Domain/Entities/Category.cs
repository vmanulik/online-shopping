using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Category : BaseEntity
{
    private readonly List<Product> _products = new();
    private readonly List<Category> _subCategories = new();

    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }

    public int? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    public IReadOnlyCollection<Product> Products => _products;
    public IReadOnlyCollection<Category> SubCategories => _subCategories;

    public Category() { }

    public Category(string name, int? parentCategoryId, string? imageUrl = null)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
        ImageUrl = imageUrl;
    }

    public void Update(
        string name,
        string? imageUrl,
        int? parentCategoryId)
    {
        Name = name;
        ImageUrl = imageUrl;
        ParentCategoryId = parentCategoryId;
    }
}