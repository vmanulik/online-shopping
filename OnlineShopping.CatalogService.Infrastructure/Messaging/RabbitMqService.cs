using Microsoft.Extensions.Options;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using RabbitMQ.Client;
using System.Text;

namespace OnlineShopping.CatalogService.Infrastructure.Messaging;

public class RabbitMqService : IRabbitMqService
{
    private IOptions<RabbitMqOptions> _options;

    public RabbitMqService(IOptions<RabbitMqOptions> options)
    {
        _options = options;
    }

    public async void SendMessageAsync(IntegrationEvent message)
    {
        var factory = new ConnectionFactory() { HostName = _options.Value.ConnectionString };

        using (var connection = await factory.CreateConnectionAsync())
        using (var channel = await connection.CreateChannelAsync())
        {
            var body = Encoding.UTF8.GetBytes(message.Data);

            await channel.BasicPublishAsync(exchange: _options.Value.CatalogUpdatesQueue,
                           routingKey: message.Name,
                           mandatory: true,
                           body: body);
        }
    }
}
