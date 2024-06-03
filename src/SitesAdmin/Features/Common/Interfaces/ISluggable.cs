namespace SitesAdmin.Features.Common.Interfaces
{
    public interface ISluggable
    {
        string? Slug { get; set; }

        string GetSlugDisplayName();
    }
}
