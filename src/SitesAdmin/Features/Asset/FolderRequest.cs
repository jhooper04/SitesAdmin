namespace SitesAdmin.Features.Asset
{
    public class FolderRequest
    {
        public required string Name { get; set; }
        public string? AccessRoles { get; set; }
        public int? ParentFolderId { get; set; }
    }
}
