using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CartService.Infrastructure;
using OnlineShopping.Shared.Domain.Events;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using RabbitMQ.Client.Events;

namespace OnlineShopping.CartService.BackgroundServices;

public class IntegrationEventListenerService : BackgroundService
{
    private readonly IServiceScope _scope;

    private readonly IRabbitMqListener _rabbitMqListener; 
    private readonly ILiteDbRepository<Cart> _cartRepository;

    public IntegrationEventListenerService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();

        _rabbitMqListener = _scope.ServiceProvider.GetRequiredService<IRabbitMqListener>();
        _rabbitMqListener.ReceivedAsync += ProcessMessage;

        _cartRepository = _scope.ServiceProvider.GetRequiredService<ILiteDbRepository<Cart>>();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        var options = _scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        try
        {
            while (!cancellation.IsCancellationRequested)
            {
                await _rabbitMqListener.BasicConsumeAsync(Events.ProductUpdate, cancellation);

                await Task.Delay(options.EventFetchPeriod, cancellation);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            await Task.Delay(5 * options.EventFetchPeriod, cancellation);
        }
    }

    private async Task ProcessMessage(object model, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray(); 
        var message = Encoding.UTF8.GetString(body);

        var template = new
        {
            Id = default(int),
            Name = string.Empty,
            ImageUrl = default(string?),
            ImageDescription = default(string?),
            Price = default(decimal),
            CategoryId = default(int)
        };
        var product = JsonConvert.DeserializeAnonymousType(message, template);

        var carts = _cartRepository.GetAllAsQueryable();

        foreach (var cart in carts)
        {
            var item = cart.Items.SingleOrDefault(x => x.Id == product.Id);
            if(item != null)
            {
                item.Change(product.Name, product.ImageUrl, product.ImageDescription, product.Price, product.CategoryId);
                await _cartRepository.UpdateAsync(cart);
            }
        }
    }
}