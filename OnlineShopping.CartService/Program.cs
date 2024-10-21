using OnlineShopping.CartService.Infrastructure.Interfaces;
using OnlineShopping.CartService.Infrastructure.Persistence;
using OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;
using OnlineShopping.CartService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure;
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<ICartServiceDbContext, CartServiceDbContext>();
            builder.Services.AddScoped(typeof(ILiteDbRepository<>), typeof(Repository<>));
            
            builder.Services.Configure<LiteDbOptions>(
                builder.Configuration.GetSection("LiteDbOptions")
            );

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}
