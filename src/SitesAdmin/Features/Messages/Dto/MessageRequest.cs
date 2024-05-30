namespace SitesAdmin.Features.Messages.Dto
{
    public class MessageRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Body { get; set; }
    }
}
