using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Assets.Dto
{
    public class FolderResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public string? AccessRoles { get; set; }
        public int? ParentFolderId { get; set; }
        public FolderResponse? ParentFolder { get; set; }
        public List<FolderResponse> SubFolders { get; set; } = [];
    }
}
