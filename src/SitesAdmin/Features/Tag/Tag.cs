﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SitesAdmin.Features.Common;

namespace SitesAdmin.Features.Tag
{
    public class Tag : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public string Slug { get { return SlugUtil.Slugify(Name); } }
        public required string Description { get; set; }
        public string? Icon { get; set; }
        public string? Banner { get; set; }

        public List<Post.Post> Posts { get; } = [];
    }
}
