using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Site
{
    public class SiteResponse : IBaseAuditableResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Slug { get; set; } = null!;
        public string Description { get; set; } = "";
        public required string BaseUrl { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
