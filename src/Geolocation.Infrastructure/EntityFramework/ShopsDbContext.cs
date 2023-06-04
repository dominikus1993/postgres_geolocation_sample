using Geolocation.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Geolocation.Infrastructure.EntityFramework;

public sealed class ShopsDbContext : DbContext
{
    public DbSet<EfShop> Shops { get; set; } = null!;

    public ShopsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<EfShop?> GetById(ShopId id, CancellationToken cancellationToken)
    {
        return await Shops.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
}