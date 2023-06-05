using Geolocation.Core.Model;
using Geolocation.Core.Repositories;
using Geolocation.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Geolocation.Infrastructure.Repositories;

public sealed class EfShopsWriter : IShopsWriter
{
    private IDbContextFactory<ShopsDbContext> _dbContextFactory;

    public EfShopsWriter(IDbContextFactory<ShopsDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task Write(IEnumerable<Shop> shops, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Shops.UpsertRange(shops.Select(shop => new EfShop()
                { Id = shop.Id, Location = new Point(new Coordinate(shop.Location.Longitude, shop.Location.Latitude)) }))
            .RunAsync(cancellationToken);
    }
}