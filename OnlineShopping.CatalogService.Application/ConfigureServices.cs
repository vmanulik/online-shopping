using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopping.CatalogService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure;
using System.Reflection;

namespace OnlineShopping.CatalogService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddCatalogServiceApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(ISharedRepository<>), typeof(Repository<>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
