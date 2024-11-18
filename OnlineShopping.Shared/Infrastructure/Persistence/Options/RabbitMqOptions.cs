namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class RabbitMqOptions
{
    public string Url { get; init; }
    public int Port { get; init; }
    public string VirtualHost { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public int EventFetchPeriod { get; init; }
}
