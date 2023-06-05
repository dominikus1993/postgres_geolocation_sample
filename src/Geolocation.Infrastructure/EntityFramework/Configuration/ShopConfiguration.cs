using Geolocation.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Geolocation.Infrastructure.EntityFramework.Configuration;

internal sealed class ShopIdConverter : ValueConverter<ShopId, int>
{
    public ShopIdConverter()
        : base(
            v => v.Value,
            v => new ShopId(v))
    {
    }
}

public class ShopConfiguration: IEntityTypeConfiguration<EfShop>
{
    public void Configure(EntityTypeBuilder<EfShop> builder)
    {
        builder.HasKey(product => product.Id);
        builder.Property(x => x.Id).HasConversion<ShopIdConverter>();
        builder.Property(b => b.Location).HasColumnType("geography (point)");
    }
}