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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var options = _scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        var rabbitMqService = _scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
        var eventsRepository = _scope.ServiceProvider.GetRequiredService<ISharedRepository<IntegrationEvent>>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var events = await eventsRepository
                    .GetAllAsQueryable()
                    .OrderBy(o => o.Id)
                    .ToListAsync(cancellationToken);

                foreach (var message in events)
                {
                    rabbitMqService.SendMessageAsync(message, cancellationToken);

                    await eventsRepository.RemoveAsync(message, cancellationToken);
                }

                await Task.Delay(options.EventFetchPeriod, cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            await Task.Delay(5 * options.EventFetchPeriod, cancellationToken);
        }
    }
}