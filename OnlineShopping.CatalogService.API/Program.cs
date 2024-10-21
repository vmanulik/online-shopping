using OnlineShopping.CatalogService.Application;
using OnlineShopping.CatalogService.Infrastructure;

namespace OnlineShopping.CatalogService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCatalogInfrastructureServices(builder.Configuration);
            builder.Services.AddCatalogServiceApplicationServices(builder.Configuration);

            var app = builder.Build();

            app.UseMigrationsEndPoint();

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
    }
}
