namespace SitesAdmin.Features.Common
{
    public abstract class BaseAuditableResponse : BaseResponse
    {
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
