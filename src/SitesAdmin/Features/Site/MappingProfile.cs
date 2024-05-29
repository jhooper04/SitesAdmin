using AutoMapper;

namespace SitesAdmin.Features.Site
{
    public class SiteMappingProfile : Profile
    {
        public SiteMappingProfile()
        {
            CreateMap<Site, SiteResponse>();
            CreateMap<SiteResponse, Site>();

            CreateMap<Site, SiteRequest>();
            CreateMap<SiteRequest, Site>();
        }
    }
}
