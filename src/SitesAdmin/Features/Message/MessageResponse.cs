using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Message
{
    public class MessageResponse : BaseResponse
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Body { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
