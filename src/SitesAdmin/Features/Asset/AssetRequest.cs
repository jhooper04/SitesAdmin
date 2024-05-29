namespace SitesAdmin.Features.Asset
{
    public class AssetRequest
    {
        public IFormFile? File { get; set; }
        public string? Caption { get; set; }
        public string? Description { get; set; } = "";
        public string? AccessRoles { get; set; }
        public string? ImageWidth { get; set; }
        public string? ImageHeight { get; set; }
        public bool? GenerateThumbnails { get; set; }
        public int? FolderId { get; set; }
    }
}
