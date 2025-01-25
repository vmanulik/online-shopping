using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CatalogService.Tests;

[TestFixture]
public class CategoryTests
{
    [Test]
    public void Update_ShouldUpdateProperties()
    {
        var category = new Category("Electronics", 1, "http://example.com/image.jpg");
        var newName = "Home Appliances";
        var newImageUrl = "http://example.com/newimage.jpg";
        int? newParentCategoryId = 2;

        category.Update(newName, newImageUrl, newParentCategoryId);

        Assert.That(category.Name, Is.EqualTo(newName));
        Assert.That(category.ImageUrl, Is.EqualTo(newImageUrl));
        Assert.That(category.ParentCategoryId, Is.EqualTo(newParentCategoryId));
    }
}