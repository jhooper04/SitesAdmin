using SitesAdmin.Features.Common;
using SitesAdmin.Features.Assets;
using SitesAdmin.Features.Categories.Dto;

namespace SitesAdmin.Features.Posts.Dto
{
    public class PostResponse : BaseAuditableResponse
    {
        public required string Title { get; set; }
        public string Description { get; set; } = "";
        public required string Slug { get; set; }
        public required string Author { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Image { get; set; }
        public string? OgImage { get; set; }
        public string? OgTitle { get; set; }
        public string? OgType { get; set; }
        public required string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsFeatured { get; set; }
        public string Tags { get; } = "";
        public int CategoryId { get; set; }
        public CategoryResponse Category { get; set; } = null!;
    }
}
