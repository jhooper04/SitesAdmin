using Microsoft.EntityFrameworkCore;
using SitesAdmin.Features.Asset;
using SitesAdmin.Features.Category;
using SitesAdmin.Features.Identity;
using SitesAdmin.Features.Message;
using SitesAdmin.Features.Post;
using SitesAdmin.Features.Project;
using SitesAdmin.Features.Tag;
using SitesAdmin.Features.Site;
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
