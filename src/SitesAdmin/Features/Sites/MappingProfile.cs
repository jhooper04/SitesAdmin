using AutoMapper;
using SitesAdmin.Features.Sites.Dto;
using SitesAdmin.Features.Sites.Data;

namespace SitesAdmin.Features.Sites
{
    public class SiteMappingProfile : Profile
    {
        public SiteMappingProfile()
        {
            CreateMap<Site, SiteResponse>();
            CreateMap<SiteResponse, Site>();

            CreateMap<Site, SiteRequest>();

            CreateMap<SiteRequest, Site>()
                .ForMember(p => p.Id, p => p.Ignore())
                .ForMember(p => p.Created, p => p.Ignore())
                .ForMember(p => p.CreatedBy, p => p.Ignore())
                .ForMember(p => p.LastModified, p => p.Ignore())
                .ForMember(p => p.LastModifiedBy, p => p.Ignore());
        }
    }
}
