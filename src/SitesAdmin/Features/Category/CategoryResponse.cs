using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Category
{
    public class CategoryResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string Slug { get; set; } = null!;
        public string? Banner { get; set; }
        public string? Icon { get; set; }

        public List<Post.Post> Posts { get; } = [];
    }
}
