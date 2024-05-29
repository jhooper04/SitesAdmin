using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using System.Xml.Linq;
using SitesAdmin.Features.Asset;

namespace SitesAdmin.Features.Post
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
        
        public List<Tag.Tag> Tags { get; } = [];
        public int CategoryId { get; set; }
        public Category.Category Category { get; } = null!;
    }
}
