using Geolocation.Core.Model;
using Geolocation.Core.Repositories;
using Geolocation.Infrastructure.Repositories;
using Geolocation.Tests.Fixture;

namespace Geolocation.Tests.Infrastructure.Repositories;

public class EfShopsFilterTests : IClassFixture<PostgresSqlFixture>, IAsyncLifetime
{
    private IShopsWriter _shopsWriter;
    private IShopsFilter _shopsFilter;
    private PostgresSqlFixture _postgresSqlFixture;

    public EfShopsFilterTests(PostgresSqlFixture postgresSqlFixture)
    {
        _postgresSqlFixture = postgresSqlFixture;
        _shopsFilter = new EfShopsFilter(postgresSqlFixture.DbContextFactory);
        _shopsWriter = new EfShopsWriter(postgresSqlFixture.DbContextFactory);
    }

    [Fact]
    public async Task TestNearestShopsWhenExists()
    {
        var subject = await _shopsFilter.GetNearestShopsTo(new ShopId(735), new DistanceKilometers(10)).ToListAsync();
        
        Assert.NotNull(subject);
        Assert.NotEmpty(subject);
    }

    public async Task InitializeAsync()
    {
        await _shopsWriter.Write(new[]
        {
            new Shop(new ShopId(735), new ShopLocation(19.381181, 51.812276)),
            new Shop(new ShopId(1241), new ShopLocation(19.37609757, 51.7973951)),
            new Shop(new ShopId(749), new ShopLocation(19.39249588, 51.79717099)),
            new Shop(new ShopId(1328), new ShopLocation(19.3921075, 51.78932352)),
            new Shop(new ShopId(1687), new ShopLocation(19.421356, 51.801341)),
        });
    }

    public async Task DisposeAsync()
    {
        _postgresSqlFixture.DbContext.Shops.RemoveRange(_postgresSqlFixture.DbContext.Shops);
        await _postgresSqlFixture.DbContext.SaveChangesAsync();
    }
}