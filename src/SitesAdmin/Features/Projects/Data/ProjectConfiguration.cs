
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SitesAdmin.Features.Projects.Data;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder
            .HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Projects_Slug");
    }
}
