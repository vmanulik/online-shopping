using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using Sieve.Services;

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

        return services;
    }
}
