using Microsoft.Extensions.Options;
using OnlineShopping.CartService.Infrastructure;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CartService.BackgroundServices;

public class IntegrationEventListenerService : BackgroundService
{
    private readonly IServiceScope _scope;
    private readonly IRabbitMqListener _rabbitMqListener;

    public IntegrationEventListenerService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();

        _rabbitMqListener.ReceivedAsync += (model, eventArgs) =>
        {
            //var body = eventArgs.Body.ToArray();

            //var message = Encoding.UTF8.GetString(body);

            // TODO Process

            return Task.CompletedTask;
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        var options = _scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        try
        {
            while (!cancellation.IsCancellationRequested)
            {
                // TODO Process

                await Task.Delay(options.EventFetchPeriod, cancellation);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            await Task.Delay(5 * options.EventFetchPeriod, cancellation);
        }
    }
}