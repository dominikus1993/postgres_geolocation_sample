using Geolocation.Core.Repositories;
using Geolocation.Infrastructure.EntityFramework;
using Geolocation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Geolocation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IShopsFilter, EfShopsFilter>();
        services.AddPooledDbContextFactory<ShopsDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("ShopsDb"), optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure(5);
                optionsBuilder.CommandTimeout(720);
                optionsBuilder.UseNetTopologySuite();
                optionsBuilder.MigrationsAssembly(typeof(ShopsDbContext).Assembly.FullName);
            });
        });
        return services;
    }
}