using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
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
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapControllerRoute(
                name: "login-callback",
                pattern: "/login-callback",
                defaults: new { controller = "Account", action = "LoginCallback" });

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
            builder.Services.Configure<KeycloakOptions>(
                builder.Configuration.GetSection(nameof(KeycloakOptions))
            );

            var keycloakOptions = builder.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
            AddKeycloakAuthentication(builder.Services, keycloakOptions!);

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
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
                options.AddPolicy("CartReadAccess", policy => policy.RequireClaim("permission", "cart:read"));
                options.AddPolicy("CartWriteAccess", policy => policy.RequireClaim("permission", "cart:write"));
            });
        }
    }
}
