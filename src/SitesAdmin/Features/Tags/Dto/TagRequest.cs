namespace SitesAdmin.Features.Tags.Dto
{
    public class TagRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Slug { get; set; }
        public string? Icon { get; set; }
        public string? Banner { get; set; }
        public string? Body { get; set; }
    }
}
