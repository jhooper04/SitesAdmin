namespace SitesAdmin.Features.Common.Interfaces
{
    public interface IBaseAuditableEntity
    {
        int Id { get; set; }
        DateTimeOffset Created { get; set; }
        string? CreatedBy { get; set; }
        DateTimeOffset LastModified { get; set; }
        string? LastModifiedBy { get; set; }
    }
}
