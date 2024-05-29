namespace SitesAdmin.Features.Common
{
    public abstract class BaseResponse
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public Site.SiteResponse Site { get; set; } = null!;
    }
}
