using SitesAdmin.Data;

namespace SitesAdmin.Features.Asset
{
    public interface IAssetManager
    {

    }
    public class AssetManager
    {
        private readonly IApplicationDbContext _context; 
        public AssetManager(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
