using Microsoft.Extensions.Options;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using RabbitMQ.Client;
using System.Text;

namespace OnlineShopping.CatalogService.Infrastructure.Messaging;

public class RabbitMqService : IRabbitMqService
{
    private readonly IOptions<RabbitMqOptions> _options;

    public RabbitMqService(IOptions<RabbitMqOptions> options)
    {
        _options = options;
    }

    public async void SendMessageAsync(IntegrationEvent message, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Url,
            VirtualHost = _options.Value.VirtualHost,
            Port = _options.Value.Port,
            UserName = _options.Value.ClientId,
            Password = _options.Value.ClientSecret
        };

        using (var connection = await factory.CreateConnectionAsync(cancellation))
        using (var channel = await connection.CreateChannelAsync(cancellationToken: cancellation))
        {
            await channel.QueueDeclareAsync(
                queue: message.Name,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellation);

            var body = Encoding.UTF8.GetBytes(message.Data);

            await channel.BasicPublishAsync(exchange: string.Empty,
                           routingKey: message.Name,
                           mandatory: true,
                           body: body,
                           cancellation);
        }
    }
}
