namespace SitesAdmin.Features.Category
{
    public class CategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; } = null!;
        public string? Banner { get; set; }
        public string? Icon { get; set; }
    }
}
