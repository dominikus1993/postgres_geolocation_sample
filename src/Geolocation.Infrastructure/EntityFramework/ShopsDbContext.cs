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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}