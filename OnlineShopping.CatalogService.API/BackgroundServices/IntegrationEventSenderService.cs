using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CatalogService.API.BackgroundServices;

public class IntegrationEventSenderService : BackgroundService
{
    private readonly IServiceScope _scope;

    public IntegrationEventSenderService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();

        using var dbContext = _scope.ServiceProvider.GetRequiredService<ICatalogServiceDbContext>();
            dbContext.Database.EnsureCreated();   
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        var options = _scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        var rabbitMqService = _scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
        var eventsRepository = _scope.ServiceProvider.GetRequiredService<ISharedRepository<IntegrationEvent>>();

        try
        {
            while (!cancellation.IsCancellationRequested)
            {
                var events = await eventsRepository
                    .GetAllAsQueryable()
                    .OrderBy(o => o.Id)
                    .ToListAsync(cancellation);

                foreach (var message in events)
                {
                    rabbitMqService.SendMessageAsync(message);

                    await eventsRepository.RemoveAsync(message, cancellation);
                }

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