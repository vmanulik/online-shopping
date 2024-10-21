using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OnlineShopping.CatalogService.Infrastracture.Persistence;

public partial class CatalogServiceDbContextInitializer
{
    private readonly ILogger<CatalogServiceDbContextInitializer> _logger;
    private readonly CatalogServiceDbContext _context;

    public CatalogServiceDbContextInitializer(ILogger<CatalogServiceDbContextInitializer> logger, CatalogServiceDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}