
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SitesAdmin.Features.Sites.Data;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder
            .HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Sites_Slug");
    }
}
