using OnlineShopping.CatalogService.Infrastructure.Repositories;
using OnlineShopping.Shared.Infrastructure;

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

            builder.Services.AddScoped(typeof(ISharedRepository<>), typeof(Repository<>));

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
