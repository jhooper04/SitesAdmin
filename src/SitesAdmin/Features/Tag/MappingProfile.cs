using AutoMapper;

namespace SitesAdmin.Features.Tag
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagResponse>();
            CreateMap<TagResponse, Tag>();
        }
    }
}
