using AutoMapper;
using SitesAdmin.Features.Messages.Dto;
using SitesAdmin.Features.Messages.Data;

namespace SitesAdmin.Features.Messages
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageResponse>();
            CreateMap<MessageResponse, Message>()
                .ForMember(p => p.Site, p => p.Ignore());

            CreateMap<Message, MessageRequest>();
            CreateMap<MessageRequest, Message>()
                .ForMember(p => p.Created, p => p.Ignore())
                .ForMember(p => p.Id, p => p.Ignore())
                .ForMember(p => p.SiteId, p => p.Ignore())
                .ForMember(p => p.Site, p => p.Ignore());

        }
    }
}
