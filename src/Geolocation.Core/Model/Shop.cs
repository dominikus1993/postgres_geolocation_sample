namespace Geolocation.Core.Model;

public sealed record ShopLocation(double Longitude, double Latitude)
{
}
public readonly record struct ShopId(int Value);
public sealed record Shop(ShopId Id, ShopLocation Location)
{
    
}