using AutoMapper;
using SitesAdmin.Features.Projects.Dto;
using SitesAdmin.Features.Projects.Data;

namespace SitesAdmin.Features.Projects
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectResponse>();

            CreateMap<ProjectResponse, Project>()
                .ForMember(p => p.Site, p => p.Ignore());

            CreateMap<Project, ProjectRequest>();

            CreateMap<ProjectRequest, Project>()
                .ForMember(p => p.Created, p => p.Ignore())
                .ForMember(p => p.CreatedBy, p => p.Ignore())
                .ForMember(p => p.LastModified, p => p.Ignore())
                .ForMember(p => p.LastModifiedBy, p => p.Ignore())
                .ForMember(p => p.Id, p => p.Ignore())
                .ForMember(p => p.SiteId, p => p.Ignore())
                .ForMember(p => p.Site, p => p.Ignore());
        }
    }
}
