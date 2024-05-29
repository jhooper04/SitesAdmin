using AutoMapper;

namespace SitesAdmin.Features.Message
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageResponse>();
            CreateMap<MessageResponse, Message>();

            CreateMap<Message, MessageRequest>();
            CreateMap<MessageRequest, Message>();
        }
    }
}
