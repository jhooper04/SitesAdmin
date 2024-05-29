using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Asset
{
    public class AssetResponse : BaseAuditableResponse
    {
        public required string Filename { get; set; }
        public required string AssetPath { get; set; }
        public required string Caption { get; set; }
        public required string Description { get; set; } = "";
        public required string Type { get; set; }
        public string? AccessRoles { get; set; }
        public string? ImageWidth { get; set; }
        public string? ImageHeight { get; set; }
        public bool? GenerateThumbnails { get; set; }
    }
}
