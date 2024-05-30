using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Projects.Dto
{
    public class ProjectResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Slug { get; set; }
        public required string Github { get; set; }
        public required string Demo { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }
    }
}
