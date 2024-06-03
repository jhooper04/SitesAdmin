using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Data;
using SitesAdmin.Features.Posts.Data;
using SitesAdmin.Features.Common.Interfaces;


namespace SitesAdmin.Features.Categories.Data
{
    public class Category : BaseAuditableEntity, ISluggable
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public required string Description { get; set; }
        public string? Body { get; set; }
        public string? Banner { get; set; }
        public string? Icon { get; set; }
        public List<Post> Posts { get; set; } = [];

        public string GetSlugDisplayName() { return Name; }
    }
}
