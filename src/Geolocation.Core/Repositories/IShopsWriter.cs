using Geolocation.Core.Model;

namespace Geolocation.Core.Repositories;

public interface IShopsWriter
{
    Task Write(IEnumerable<Shop> shops, CancellationToken cancellationToken = default);
}