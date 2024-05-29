using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Asset
{
    public class Asset : BaseAuditableEntity
    {
        public required string Filename { get; set; }
        public required string UniqueFilename { get; set; }
        public required string Caption { get; set; }
        public required string Description { get; set; } = "";
        public required string Type { get; set; }
        public string? AccessRoles { get; set; }
        public string? ImageWidth { get; set; }
        public string? ImageHeight { get; set; }
        public bool? GenerateThumbnails { get; set; }
        public int? FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}
