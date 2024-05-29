﻿using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Asset
{
    public class Folder : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public string Slug { get { return SlugUtil.Slugify(Name); } }
        public string? AccessRoles { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<Folder> SubFolders { get; set; } = [];
    }
}
