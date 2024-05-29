using AutoMapper;

namespace SitesAdmin.Features.Project
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectResponse>();
            CreateMap<ProjectResponse, Project>();

            CreateMap<Project, ProjectRequest>();
            CreateMap<ProjectRequest, Project>();
        }
    }
}
