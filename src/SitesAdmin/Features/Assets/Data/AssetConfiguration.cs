
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SitesAdmin.Features.Assets.Data;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder
            .HasIndex(e => e.UniqueFilename)
            .IsUnique()
            .HasDatabaseName("IX_Assets_UniqueFilename");
    }
}
