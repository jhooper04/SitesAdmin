using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Categories.Data;
using SitesAdmin.Features.Tags.Data;
using SitesAdmin.Features.Common.Data;

namespace SitesAdmin.Features.Posts.Data
{
    public class Post : BaseAuditableEntity
    {
        public required string Title { get; set; }
        public string Slug { get { return SlugUtil.Slugify(Title); } }
        public string Description { get; set; } = "";
        public required string Author { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Image { get; set; }

        public string? OgImage { get; set; }
        public string? OgTitle { get; set; }
        public string? OgType { get; set; }
        public required string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsFeatured { get; set; }
        public List<Tag> Tags { get; set; } = [];
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
