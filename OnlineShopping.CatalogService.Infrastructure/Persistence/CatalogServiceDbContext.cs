using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastructure.Common;
using OnlineShopping.Shared.Domain.Entities;
using System.Reflection;

namespace OnlineShopping.CatalogService.Infrastracture.Persistence;

public class CatalogServiceDbContext : DbContext, ICatalogServiceDbContext
{
    private readonly IMediator _mediator;

    public CatalogServiceDbContext(
        DbContextOptions<CatalogServiceDbContext> options,
        IMediator mediator)
       : base(options)
    {
        _mediator = mediator;
    }

    public override DatabaseFacade Database => base.Database;

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<IntegrationEvent> Events => Set<IntegrationEvent>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
