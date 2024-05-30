using AutoMapper;
using SitesAdmin.Features.Assets.Data;
using SitesAdmin.Features.Assets.Dto;

namespace SitesAdmin.Features.Assets
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset, AssetResponse>()
                .ForMember(e => e.AssetPath, c => c.Ignore());

            CreateMap<AssetResponse, Asset>()
                .ForMember(e => e.UniqueFilename, c => c.Ignore())
                .ForMember(e => e.Folder, c => c.Ignore())
                .ForMember(e => e.Site, c => c.Ignore());

            CreateMap<AssetRequest, Asset>()
                .ForMember(e => e.Filename, c => c.Ignore())
                .ForMember(e => e.UniqueFilename, c => c.Ignore())
                .ForMember(e => e.Type, c => c.Ignore())
                .ForMember(e => e.Folder, c => c.Ignore())
                .ForMember(e => e.Created, c => c.Ignore())
                .ForMember(e => e.CreatedBy, c => c.Ignore())
                .ForMember(e => e.LastModified, c => c.Ignore())
                .ForMember(e => e.LastModifiedBy, c => c.Ignore())
                .ForMember(e => e.Id, c => c.Ignore())
                .ForMember(e => e.SiteId, c => c.Ignore())
                .ForMember(e => e.Site, c => c.Ignore());


            CreateMap<Asset, AssetRequest>()
                .ForMember(e => e.File, c => c.Ignore());

            CreateMap<Folder, FolderRequest>();
            CreateMap<FolderRequest, Folder>()
                .ForMember(e => e.ParentFolder, c => c.Ignore())
                .ForMember(e => e.SubFolders, c => c.Ignore())
                .ForMember(e => e.Created, c => c.Ignore())
                .ForMember(e => e.CreatedBy, c => c.Ignore())
                .ForMember(e => e.LastModified, c => c.Ignore())
                .ForMember(e => e.LastModifiedBy, c => c.Ignore())
                .ForMember(e => e.Id, c => c.Ignore())
                .ForMember(e => e.SiteId, c => c.Ignore())
                .ForMember(e => e.Site, c => c.Ignore());

            CreateMap<Folder, FolderResponse>();

            CreateMap<FolderResponse, Folder>()
                .ForMember(e => e.Site, c => c.Ignore());
        }
    }
}
