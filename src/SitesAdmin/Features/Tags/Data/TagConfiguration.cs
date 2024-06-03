
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

namespace SitesAdmin.Features.Tags.Data;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Tags_Slug");
    }
}
