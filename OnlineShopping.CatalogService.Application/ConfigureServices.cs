using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopping.CatalogService.Application.Common.Configurations;
using OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;
using OnlineShopping.CatalogService.Application.Common.Interfaces;
using OnlineShopping.CatalogService.Application.Common.Services;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastructure.Common;
using OnlineShopping.CatalogService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure.Abstraction;

namespace OnlineShopping.CatalogService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddCatalogServiceApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped(typeof(ILinksService<>), typeof(LinksService<>));
        services.AddScoped(typeof(ISharedRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<CategorySieveProcessor>();
        services.AddTransient<ProductSieveProcessor>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
