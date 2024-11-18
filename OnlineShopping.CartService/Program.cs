using OnlineShopping.CartService.Application.Common.Configurations;
using OnlineShopping.CartService.BackgroundServices;
using OnlineShopping.CartService.Configuration;
using OnlineShopping.CartService.Infrastructure;
using OnlineShopping.CartService.Infrastructure.Interfaces;
using OnlineShopping.CartService.Infrastructure.Persistence;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;
using OnlineShopping.CartService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using System.Reflection;

namespace OnlineShopping.CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            app.UseSwaggerDocumentation(builder.Configuration);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.ConfigureApiVersioning();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            builder.Services.AddSingleton<ICartServiceDbContext, CartServiceDbContext>();
            builder.Services.AddScoped(typeof(ILiteDbRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IRabbitMqListener, RabbitMqListener>();
            builder.Services.AddHostedService<IntegrationEventListenerService>();

            builder.Services.Configure<LiteDbOptions>(
                builder.Configuration.GetSection(nameof(LiteDbOptions))
            );
            builder.Services.Configure<RabbitMqOptions>(
                builder.Configuration.GetSection(nameof(RabbitMqOptions))
            );

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}
