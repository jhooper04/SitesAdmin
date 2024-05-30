using Microsoft.EntityFrameworkCore;
using SitesAdmin.Features.Assets.Data;
using SitesAdmin.Features.Categories.Data;
using SitesAdmin.Features.Identity.Data;
using SitesAdmin.Features.Messages.Data;
using SitesAdmin.Features.Posts.Data;
using SitesAdmin.Features.Projects.Data;
using SitesAdmin.Features.Tags.Data;
using SitesAdmin.Features.Sites.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SitesAdmin.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Site> Sites { get; }
        DbSet<Post> Posts { get; }
        DbSet<Message> Messages { get; }
        DbSet<Category> Categories { get; }
        DbSet<Tag> Tags { get; }
        DbSet<Project> Projects { get; }
        DbSet<Folder> Folders { get; }
        DbSet<Asset> Assets { get; }

        DbSet<ApplicationUser> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    }

}
