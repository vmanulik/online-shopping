using Microsoft.Extensions.Options;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

public class RabbitMqListener : BackgroundService
{
    private IConnection _connection;
    private IChannel _channel;
    private IOptions<RabbitMqOptions> _options;

    public RabbitMqListener(IOptions<RabbitMqOptions> options)
    {
        _options = options;

        var factory = new ConnectionFactory { HostName = _options.Value.ConnectionString };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);
            
            // TODO Process

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(_options.Value.CatalogUpdatesQueue, false, consumer);

        return;
    }

    public override void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();

        base.Dispose();
    }
}
