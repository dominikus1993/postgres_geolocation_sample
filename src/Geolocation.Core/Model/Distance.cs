namespace Geolocation.Core.Model;

public readonly record struct DistanceKilometers(ushort Kilometers)
{

    private const ushort Division = 1000;
    public uint ToMeteres() => (uint)(Kilometers * Division);
}
