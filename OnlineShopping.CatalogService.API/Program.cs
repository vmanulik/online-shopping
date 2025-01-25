using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OnlineShopping.CatalogService.API.BackgroundServices;
using OnlineShopping.CatalogService.Application;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.CatalogService.Infrastructure;
using OnlineShopping.CatalogService.Infrastructure.Messaging;
using OnlineShopping.Shared.Auth;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CatalogService.API
{
    public partial class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            app.UseMigrationsEndPoint();

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializator = scope.ServiceProvider.GetRequiredService<CatalogServiceDbContextInitializer>();

                await dbInitializator.InitialiseAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                var keycloakOptions = app.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API");
                    options.OAuthClientId(keycloakOptions!.ClientId);
                    options.OAuthClientSecret(keycloakOptions!.ClientSecret);
                    options.EnableTryItOutByDefault();
                    //options.EnablePersistAuthorization();
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseResponseCaching();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Category",
                    new CacheProfile()
                    {
                        Duration = 600
                    });
                options.CacheProfiles.Add("Product",
                    new CacheProfile()
                    {
                        Duration = 60
                    });
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCatalogInfrastructureServices(builder.Configuration);
            builder.Services.AddCatalogServiceApplicationServices(builder.Configuration);

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddSingleton<IAuthenticationTokenExchange, AuthenticationTokenExchange>();
            builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
            builder.Services.AddHostedService<IntegrationEventSenderService>();

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection(nameof(KeycloakOptions)));

            var keycloakOptions = builder.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Online Shopping API",
                    Description = "API for managing Products and Categories items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/license/mit")
                    }
                });

                options.AddSwaggerKeycloakSecurity(keycloakOptions!);
            });

            builder.Services.AddKeycloakAuthentication(keycloakOptions!);

            //builder.Services.AddAuthorization();
        }
    }
}