namespace SitesAdmin.Features.Common
{
    public class PaginatedRequest
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public string? OrderBy { get; set; }
        public bool? OrderDesc { get; set; }
    }
}
