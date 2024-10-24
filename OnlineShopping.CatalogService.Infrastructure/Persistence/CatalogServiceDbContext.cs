﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using System.Reflection;

namespace OnlineShopping.CatalogService.Infrastracture.Persistence;

public class CatalogServiceDbContext : DbContext, ICatalogServiceDbContext
{
    public CatalogServiceDbContext(
        DbContextOptions<CatalogServiceDbContext> options)
       : base(options)
    {
    }

    public override DatabaseFacade Database => base.Database;

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
