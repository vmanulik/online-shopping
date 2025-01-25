using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.API;
using OnlineShopping.CatalogService.Application.Categories.Commands;
using OnlineShopping.CatalogService.Tests.Configuration;
using System.Net.Http.Json;

namespace OnlineShopping.CatalogService.Tests;

[TestFixture]
public class CategoryIntegrationTests
{
    private CustomWebApplicationFactory<Program> _factory;

    [OneTimeSetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task Add_ShouldBeStoredAndRetrieved()
    {
        using var client = _factory.CreateClient();

        var response1 = await client.GetFromJsonAsync<List<Category>>("/category");
        Assert.That(response1 != null);
        Assert.That(response1!.Count, Is.EqualTo(0));

        var request = new CreateCategoryCommand("Category 1", null, null);
        var response2 = await client.PostAsJsonAsync("/category", request);
        response2.EnsureSuccessStatusCode();

        var response3 = await client.GetFromJsonAsync<List<Category>>("/category");
        Assert.That(response3 != null);
        Assert.That(response3!.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Remove_ShouldBeDeletedAndNotRetrieved()
    {
        using var client = _factory.CreateClient();

        var request = new CreateCategoryCommand("Category 1", null, null);
        var response1 = await client.PostAsJsonAsync("/category", request);
        response1.EnsureSuccessStatusCode(); 
            
        var response2 = await client.GetFromJsonAsync<List<Category>>("/category");
        Assert.That(response2 != null);
        Assert.That(response2!.Count, Is.EqualTo(1));

        var response4 = await client.DeleteAsync("/category/1");
        response4.EnsureSuccessStatusCode();

        var response5 = await client.GetFromJsonAsync<List<Category>>("/category");
        Assert.That(response5 != null);
        Assert.That(response5!.Count, Is.EqualTo(response2.Count - 1));
    }
}