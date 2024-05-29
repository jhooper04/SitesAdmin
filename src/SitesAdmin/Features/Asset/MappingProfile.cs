using AutoMapper;

namespace SitesAdmin.Features.Asset
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset, AssetResponse>();
            CreateMap<AssetResponse, Asset>();

            CreateMap<AssetRequest, Asset>();
            CreateMap<Asset, AssetRequest>();

            CreateMap<Folder, FolderRequest>();
            CreateMap<FolderRequest, Folder>();

            CreateMap<Folder, FolderResponse>();
            CreateMap<FolderResponse, Folder>();
        }
    }
}
