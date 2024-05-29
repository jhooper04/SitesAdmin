namespace SitesAdmin.Features.Tag
{
    public class TagRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Icon { get; set; }
        public string? Banner { get; set; }
    }
}
