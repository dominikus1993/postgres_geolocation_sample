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

    private static readonly Func<ShopsDbContext, ShopId, CancellationToken, Task<EfShop?>> GetByIdQ =
        EF.CompileAsyncQuery(
            (ShopsDbContext context, ShopId id, CancellationToken cancellationToken) =>
                context.Shops.AsNoTracking().FirstOrDefault(c => c.Id == id));
    
    public Task<EfShop?> GetById(ShopId id, CancellationToken cancellationToken)
    {
        return GetByIdQ(this, id, cancellationToken);
    }
}