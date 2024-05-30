using AutoMapper;
using SitesAdmin.Features.Tags.Dto;
using SitesAdmin.Features.Tags.Data;

namespace SitesAdmin.Features.Tags
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagResponse>();

            CreateMap<TagResponse, Tag>()
                .ForMember(p => p.Posts, p => p.Ignore())
                .ForMember(p => p.Site, p => p.Ignore());
        }
    }
}
