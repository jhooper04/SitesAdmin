
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SitesAdmin.Features.Assets.Data;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder
            .HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Folders_Slug");
    }
}
