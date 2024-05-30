using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Data;

namespace SitesAdmin.Features.Projects.Data
{
    public class Project : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string Slug { get { return SlugUtil.Slugify(Name); } }
        public string? Github { get; set; }
        public string? Demo { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }
    }
}
