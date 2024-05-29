using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Tag
{
    public class TagResponse : BaseAuditableResponse
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Icon { get; set; }
        public required string Banner { get; set; }
        public List<Post.Post> Posts { get; } = [];
    }
}
