using AutoMapper;
using SitesAdmin.Features.Categories.Dto;
using SitesAdmin.Features.Categories.Data;
using SitesAdmin.Features.Sites.Dto;

namespace SitesAdmin.Features.Categories
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResponse>();
                
            CreateMap<CategoryResponse, Category>()
                .ForMember(e => e.Posts, c => c.Ignore())
                .ForMember(e => e.Site, c => c.Ignore());

            CreateMap<Category, CategoryRequest>();
            CreateMap<CategoryRequest, Category>()
                .ForMember(e => e.Site, c => c.Ignore())
                .ForMember(e => e.SiteId, c => c.Ignore())
                .ForMember(e => e.Id, c => c.Ignore())
                .ForMember(e => e.Posts, c => c.Ignore())
                .ForMember(e => e.Created, c => c.Ignore())
                .ForMember(e => e.CreatedBy, c => c.Ignore())
                .ForMember(e => e.LastModified, c => c.Ignore())
                .ForMember(e => e.LastModifiedBy, c => c.Ignore());


        }
    }
}
