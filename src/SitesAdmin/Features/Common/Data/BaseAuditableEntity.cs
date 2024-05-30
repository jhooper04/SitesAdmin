using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Common.Data
{
    public abstract class BaseAuditableEntity : BaseEntity, IBaseAuditableEntity
    {
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
