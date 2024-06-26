﻿namespace SitesAdmin.Features.Assets.Dto
{
    public class FolderRequest
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string? AccessRoles { get; set; }
        public int? ParentFolderId { get; set; }
    }
}
