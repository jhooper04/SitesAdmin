namespace SitesAdmin.Features.Site
{
    public class SiteRequest
    {
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public required string BaseUrl { get; set; }
    }
}
