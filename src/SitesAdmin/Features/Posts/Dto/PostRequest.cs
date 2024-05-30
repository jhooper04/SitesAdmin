namespace SitesAdmin.Features.Posts.Dto
{
    public class PostRequest
    {
        public required string Title { get; set; }
        public string Description { get; set; } = "";
        public string? Author { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Image { get; set; }

        public string? OgImage { get; set; }
        public string? OgTitle { get; set; }
        public string? OgType { get; set; }
        public required string Body { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public string Tags { get; set; } = "";
    }
}
