using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Data;
using SitesAdmin.Features.Common.Interfaces;
using SitesAdmin.Features.Identity.Interfaces;
using SitesAdmin.Features.Sites.Data;

namespace SitesAdmin.Data.Interceptors
{
    public class SluggableSaveChangesInterceptor : SaveChangesInterceptor
    {
        public SluggableSaveChangesInterceptor()
        {
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<ISluggable>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    SlugUtil.SetDefaultSlug(entry.Entity);
                }
            }
        }
    }
}
