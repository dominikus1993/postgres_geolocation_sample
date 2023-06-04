using Geolocation.Core.Model;
using NetTopologySuite.Geometries;

namespace Geolocation.Infrastructure.EntityFramework;

public sealed class EfShop
{
    public ShopId Id { get; set; }
    public Point Location { get; set; }
}