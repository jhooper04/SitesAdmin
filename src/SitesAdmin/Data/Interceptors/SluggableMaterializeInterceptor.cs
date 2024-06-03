using Microsoft.EntityFrameworkCore.Diagnostics;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Data.Interceptors
{
    public class SluggableMaterializeInterceptor : IMaterializationInterceptor
    {
        public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
        {
            if (entity is ISluggable { } sluggable)
            {
                SlugUtil.SetDefaultSlug(sluggable);
            }

            return entity;
        }
    }
}
