
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SitesAdmin.Features.Categories.Data;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Slug");
    }
}
