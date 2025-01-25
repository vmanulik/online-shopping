using System.Reflection;
using OnlineShopping.CartService.Application.Common.Configurations;
using OnlineShopping.CartService.BackgroundServices;
using OnlineShopping.CartService.Configuration;
using OnlineShopping.CartService.Infrastructure;
using OnlineShopping.CartService.Infrastructure.Interfaces;
using OnlineShopping.CartService.Infrastructure.Persistence;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;
using OnlineShopping.CartService.Infrastructure.Repositories;
using OnlineShopping.Shared.Auth;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CartService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            app.UseSwaggerDocumentation(builder.Configuration);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.ConfigureApiVersioning();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

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
            builder.Services.Configure<KeycloakOptions>(
                builder.Configuration.GetSection(nameof(KeycloakOptions))
            );

            var keycloakOptions = builder.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
            builder.Services.AddKeycloakAuthentication(keycloakOptions!);

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSwaggerKeycloakSecurity(keycloakOptions!);
            });

            builder.Services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAllPolicy", policyBuilder =>
            {
                policyBuilder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

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