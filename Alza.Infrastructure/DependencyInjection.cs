﻿using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        var connectionString = builderConfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

        if (bool.TryParse(builderConfiguration.GetSection("UseInMemoryDatabase").Value ?? "false", out var useInMemoryDb) && useInMemoryDb)
        {
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IProductRepository, ProductRepository>();
        }

        services.AddScoped<DatabaseMockDataSeeder>();

        return services;
    }
}