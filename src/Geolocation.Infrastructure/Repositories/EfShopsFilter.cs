using System.Runtime.CompilerServices;
using Geolocation.Core.Model;
using Geolocation.Core.Repositories;
using Geolocation.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Geolocation.Infrastructure.Repositories;

public sealed class EfShopsFilter : IShopsFilter
{
    private IDbContextFactory<ShopsDbContext> _dbContextFactory;

    public EfShopsFilter(IDbContextFactory<ShopsDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async IAsyncEnumerable<Shop> GetNearestShopsTo(ShopId id, DistanceKilometers distance,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var shop = await context.GetById(id, cancellationToken);

        if (shop is null)
        {
            yield break;
        }

        var meters = distance.ToMeteres();
        var shops = context.Shops.AsNoTracking()
            .Select(x => new { x.Id, x.Location, Distance = x.Location.Distance(shop.Location) })
            .Where(x => x.Id != shop.Id && x.Distance <= meters);
        var q = shops.ToQueryString();
        await foreach (var nearestShop in shops.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return new Shop(nearestShop.Id, new ShopLocation(nearestShop.Location.X, nearestShop.Location.Y));
        }
    }
}