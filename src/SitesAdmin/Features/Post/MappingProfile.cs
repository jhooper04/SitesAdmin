using AutoMapper;

namespace SitesAdmin.Features.Post
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(p => p.Tags, p => p.MapFrom(tag => string.Join(", ", tag.Tags.Select(t => t.Name).ToArray())));
           // CreateMap<PostResponse, Post>();

            //CreateMap<Post, PostRequest>();
            CreateMap<PostRequest, Post>()
                .ForMember(p => p.Tags, p => p.Ignore());

        }
    }
}
