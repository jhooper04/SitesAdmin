using AutoMapper;

namespace SitesAdmin.Features.Category
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryResponse, Category>();

            CreateMap<Category, CategoryRequest>();
            CreateMap<CategoryRequest, Category>();
        }
    }
}
