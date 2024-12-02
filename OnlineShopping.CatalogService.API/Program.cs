using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
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
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please provide JWT with bearer (Bearer {jwt token})",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new List<string>() }
                });
            });

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
            IdentityModelEventSource.ShowPII = true;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Url;
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://{keycloakHost}:{keycloakPort}/realms/{yourRealm}"
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CatalogReadAccess", policy => policy.RequireClaim("permission", "catalog:read"));
                options.AddPolicy("CatalogWriteAccess", policy => policy.RequireClaim("permission", "catalog:write"));
            });
        }
    }
}
