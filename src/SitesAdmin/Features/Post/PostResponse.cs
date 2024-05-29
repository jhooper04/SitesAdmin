using SitesAdmin.Features.Common;
using SitesAdmin.Features.Asset;

namespace SitesAdmin.Features.Post
{
    public class PostResponse : BaseAuditableResponse
    {
        public required string Title { get; set; }
        public string Description { get; set; } = "";
        public required string Author { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Image { get; set; }

        public string? OgImage { get; set; }
        public string? OgTitle { get; set; }
        public string? OgType { get; set; }
        public required string Body { get; set; }
        public DateTime PublishDate { get; set; }

        public string Tags { get; } = "";
        public Category.Category Category { get; } = null!;
    }
}
