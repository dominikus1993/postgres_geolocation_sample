using Geolocation.Core.Model;
using Geolocation.Core.Repositories;
using Geolocation.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Geolocation.Infrastructure.Repositories;

public sealed class EfShopsFilter : IShopsFilter
{
    private IDbContextFactory<ShopsDbContext> _dbContextFactory;

    public EfShopsFilter(IDbContextFactory<ShopsDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async IAsyncEnumerable<Shop> GetNearestShopsTo(ShopId id, ushort distance, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var shop = await context.GetById(id, cancellationToken);

        if (shop is null)
        {
            yield break;
        }

        var shops = context.Shops.AsNoTracking().Where(x => x.Location.Distance(shop.Location) <= distance)
            .Select(x => new { x.Id, x.Location, Distance = x.Location.Distance(shop.Location) });

        await foreach (var nearestShop in shops.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return new Shop() { };
        }
    }
}