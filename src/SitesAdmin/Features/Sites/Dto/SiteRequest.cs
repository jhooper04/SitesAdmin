namespace SitesAdmin.Features.Sites.Dto
{
    public class SiteRequest
    {
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public string? Slug { get; set; }
        public required string BaseUrl { get; set; }
    }
}
