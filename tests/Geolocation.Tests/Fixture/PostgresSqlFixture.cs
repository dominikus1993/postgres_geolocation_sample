using Geolocation.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Geolocation.Tests.Fixture;

public sealed class PostgresSqlFixture: IAsyncLifetime, IDisposable
{
    public PostgreSqlContainer PostgreSql { get; }
    public TestDbContextFactory DbContextFactory { get; private set; } = null!;
    public ShopsDbContext DbContext { get; private set; } = null!;
    public PostgresSqlFixture()
    {
        this.PostgreSql = new PostgreSqlBuilder().Build();
    }

    public async Task InitializeAsync()
    {
        await this.PostgreSql.StartAsync()
            .ConfigureAwait(false);
        var builder = new DbContextOptionsBuilder<ShopsDbContext>()
            // .UseModel(ProductsDbContextModel.Instance)
            .UseNpgsql(this.PostgreSql.GetConnectionString(),
                optionsBuilder =>
                {
                    optionsBuilder.EnableRetryOnFailure(5);
                    optionsBuilder.CommandTimeout(500);
                    optionsBuilder.MigrationsAssembly("Geolocation.Infrastructure");
                });
        var context = new ShopsDbContext(builder.Options);
        DbContextFactory = new TestDbContextFactory(builder);
        DbContext = context;
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await this.PostgreSql.DisposeAsync()
            .ConfigureAwait(false);
        await DbContext.DisposeAsync();
    }

    public void Dispose()
    {
    }
}

public sealed class TestDbContextFactory : IDbContextFactory<ShopsDbContext>
{
    private readonly  DbContextOptionsBuilder<ShopsDbContext> _builder;

    public TestDbContextFactory(DbContextOptionsBuilder<ShopsDbContext> builder)
    {
        _builder = builder;
    }

    public ShopsDbContext CreateDbContext()
    {
        return new ShopsDbContext(_builder.Options);
    }
}

[CollectionDefinition(nameof(PostgresSqlFixtureCollectionTests))]
public sealed class PostgresSqlFixtureCollectionTests : ICollectionFixture<PostgresSqlFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}