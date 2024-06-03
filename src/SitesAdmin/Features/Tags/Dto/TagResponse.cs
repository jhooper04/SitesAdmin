using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Tags.Dto
{
    public class TagResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Slug { get; set; }
        public required string Icon { get; set; }
        public required string Banner { get; set; }
        public string? Body { get; set; }
    }
}
