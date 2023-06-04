using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geolocation.Infrastructure.EntityFramework.Configuration;

public class ShopConfiguration: IEntityTypeConfiguration<EfShop>
{
    public void Configure(EntityTypeBuilder<EfShop> builder)
    {
        builder.HasKey(product => product.Id);
        builder.Property(b => b.Location).HasColumnType("geography (point)");
    }
}