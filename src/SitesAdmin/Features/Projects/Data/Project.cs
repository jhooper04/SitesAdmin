using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Common.Data;
using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Projects.Data
{
    public class Project : BaseAuditableEntity, ISluggable
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Slug { get; set; }
        public string? Github { get; set; }
        public string? Demo { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }

        public string GetSlugDisplayName() { return Name; }
    }
}
