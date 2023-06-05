using Geolocation.Core.Model;

namespace Geolocation.Core.Repositories;

public interface IShopsFilter
{
    IAsyncEnumerable<Shop> GetNearestShopsTo(ShopId id, DistanceKilometers distance, CancellationToken cancellationToken = default);
}