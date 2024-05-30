namespace SitesAdmin.Features.Projects.Dto
{
    public class ProjectRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Github { get; set; }
        public string? Demo { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }
    }
}
