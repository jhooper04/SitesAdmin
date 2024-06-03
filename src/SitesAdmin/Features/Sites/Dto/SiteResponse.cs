using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Sites.Dto
{
    public class SiteResponse : IBaseAuditableResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; } = "";
        public required string BaseUrl { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
