using SitesAdmin.Features.Common.Data;
using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Assets.Data
{
    public class Folder : BaseAuditableEntity, ISluggable
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string? AccessRoles { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<Folder> SubFolders { get; set; } = [];

        public string GetSlugDisplayName() { return Name; }
    }
}
