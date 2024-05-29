using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Project
{
    public class ProjectResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public string Slug { get; set; } = null!;
        public required string Github { get; set; }
        public required string Demo { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }
    }
}
