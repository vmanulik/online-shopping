namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class RabbitMqOptions
{
    public string ConnectionString { get; init; }
    public string CatalogUpdatesQueue { get; init; }
    public int EventFetchPeriod { get; init; }
}
