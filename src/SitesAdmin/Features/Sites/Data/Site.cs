using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Sites.Data
{
    public class Site : IBaseAuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Slug { get { return SlugUtil.Slugify(Name); } }
        public string Description { get; set; } = "";
        public required string BaseUrl { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
