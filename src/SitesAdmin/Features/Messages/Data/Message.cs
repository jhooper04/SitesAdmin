using SitesAdmin.Features.Common.Data;

namespace SitesAdmin.Features.Messages.Data
{
    public class Message : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Body { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
