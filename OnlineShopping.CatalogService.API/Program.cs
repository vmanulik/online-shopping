using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShopping.CatalogService.API.BackgroundServices;
using OnlineShopping.CatalogService.Application;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.CatalogService.Infrastructure;
using OnlineShopping.CatalogService.Infrastructure.Messaging;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using System.Text.Json.Serialization;

namespace OnlineShopping.CatalogService.API
{
    public partial class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            });

            builder.Services.AddCatalogInfrastructureServices(builder.Configuration);
            builder.Services.AddCatalogServiceApplicationServices(builder.Configuration);

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
            builder.Services.AddHostedService<IntegrationEventSenderService>();

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection(nameof(KeycloakOptions)));

            var keycloakOptions = builder.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
            AddKeycloakAuthentication(builder.Services, keycloakOptions!);

            var app = builder.Build();

            app.UseMigrationsEndPoint();

            // Initialise and seed database
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializator = scope.ServiceProvider.GetRequiredService<CatalogServiceDbContextInitializer>();

                await dbInitializator.InitialiseAsync();
            }
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseResponseCaching(); 

            app.MapControllers();

            app.MapControllerRoute(
                name: "login-callback",
                pattern: "/login-callback",
                defaults: new { controller = "Account", action = "LoginCallback" });

            app.Run();
        }

        private static void AddKeycloakAuthentication(IServiceCollection services, KeycloakOptions settings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            })
            .AddOpenIdConnect(options =>
            {
                options.Authority = settings.Url;
                options.ClientId = settings.ClientId;
                options.ClientSecret = settings.ClientSecret;
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.Scope.Add("openid");
                options.RequireHttpsMetadata = false;
                options.CallbackPath = "/login-callback"; // Update callback path
                options.SignedOutCallbackPath = "/logout-callback"; // Update signout callback path
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };
            });
        }
    }
}
