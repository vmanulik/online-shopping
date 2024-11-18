using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.CatalogService.Infrastructure.Messaging;
using OnlineShopping.CatalogService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure.Abstraction;

namespace OnlineShopping.CatalogService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddCatalogInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogServiceDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                builder =>
                {
                    builder.MigrationsAssembly(typeof(CatalogServiceDbContext).Assembly.FullName);
                    builder.MigrationsHistoryTable("_EFMigrationsHistory_CatalogService");
                }
            ));

        services.AddScoped<CatalogServiceDbContextInitializer>();
        services.AddScoped<ICatalogServiceDbContext, CatalogServiceDbContext>();
        services.AddScoped(typeof(ISharedRepository<>), typeof(Repository<>));
        services.AddScoped<IRabbitMqService, RabbitMqService>();

        return services;
    }
}
