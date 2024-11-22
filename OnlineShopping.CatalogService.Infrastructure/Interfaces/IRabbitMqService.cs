using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastracture.Interfaces;

public interface IRabbitMqService
{
    void SendMessageAsync(IntegrationEvent message, CancellationToken cancellation);
}