using Geolocation.Core.Model;

namespace Geolocation.Core.Repositories;

public interface IShopsFilter
{
    IAsyncEnumerable<Shop> GetNearestShopsTo(ShopId id, ushort distance, CancellationToken cancellationToken = default);
}