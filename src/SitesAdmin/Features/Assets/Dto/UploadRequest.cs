namespace SitesAdmin.Features.Assets.Dto
{
    public class UploadRequest
    {
        public IFormFile? File { get; set; }
        public string? Caption { get; set; }
        public string? Description { get; set; } = "";
        public string? AccessRoles { get; set; }
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public bool? GenerateThumbnails { get; set; }
        public int? FolderId { get; set; }
    }
}
