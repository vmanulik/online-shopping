using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OnlineShopping.CatalogService.Application;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.CatalogService.Infrastructure;

namespace OnlineShopping.CatalogService.API
{
    public partial class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCatalogInfrastructureServices(builder.Configuration);
            builder.Services.AddCatalogServiceApplicationServices(builder.Configuration);

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

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseResponseCaching(); 

            app.MapControllers();

            app.Run();
        }
    }
}
