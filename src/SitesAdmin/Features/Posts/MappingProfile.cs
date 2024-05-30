using AutoMapper;
using SitesAdmin.Features.Posts.Dto;
using SitesAdmin.Features.Posts.Data;
using SitesAdmin.Features.Categories.Dto;
using SitesAdmin.Features.Categories.Data;

namespace SitesAdmin.Features.Posts
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(p => p.Tags, p => p.MapFrom(tag => string.Join(", ", tag.Tags.Select(t => t.Name).ToArray())))
                .AfterMap((p, pr, rc) => pr.Category = rc.Mapper.Map<CategoryResponse>(p.Category));
                //.ForMember(p => p.Category, c => c.MapFrom<Category>((p=>p.Category)));
            // CreateMap<PostResponse, Post>();

            //CreateMap<Post, PostRequest>();
            CreateMap<PostRequest, Post>()
                .ForMember(p => p.Category, p => p.Ignore())
                .ForMember(p => p.Created, p => p.Ignore())
                .ForMember(p => p.CreatedBy, p => p.Ignore())
                .ForMember(p => p.LastModified, p => p.Ignore())
                .ForMember(p => p.LastModifiedBy, p => p.Ignore())
                .ForMember(p => p.Id, p => p.Ignore())
                .ForMember(p => p.SiteId, p => p.Ignore())
                .ForMember(p => p.Site, p => p.Ignore())
                .ForMember(p => p.Tags, p => p.Ignore());

        }
    }
}
