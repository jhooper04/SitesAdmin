using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;
using SitesAdmin.Features.Posts.Dto;
using SitesAdmin.Features.Posts.Data;
using SitesAdmin.Features.Tags.Data;
using SitesAdmin.Features.Common.Interfaces;

namespace SitesAdmin.Features.Posts
{
    [Route("[controller]", Name ="[controller][action]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public PostsController(ILogger<PostsController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<PostResponse>> Create([FromQuery] int siteId, [FromBody] PostRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Post>(post);
            record.SiteId = siteId;
            SlugUtil.SetDefaultSlug(record);

            var category = _dbContext.Categories.Find(record.CategoryId);
            if (category == null) return BadRequest("Category not found");
            if (category.SiteId != siteId) return BadRequest("Category doesn't belong to this site");

            // lookup tag names and add them to the post
            var tagNames = post.Tags.Split(",").Distinct().ToList();
            var tags = _dbContext.Tags.Where(t => tagNames.Contains(t.Name)).ToList();
            
            if (tagNames.Count > tags.Count)
            {
                // add new tags that don't exist
                foreach (var newTagName in tagNames)
                {
                    if (tags.Find(t => t.Name == newTagName) == null)
                    {
                        var newTag = new Tag { 
                            Name = newTagName, 
                            Description = "" 
                        };
                        _dbContext.Tags.Add(newTag);
                        tags.Add(newTag);
                    }
                }
                
            }

            record.Tags.AddRange(tags);
            _dbContext.Posts.Add(record);

            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<PostResponse>(record));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<PostResponse>> Update([FromQuery] int siteId, int id, [FromBody] PostRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _dbContext.Posts.FindAsync(id);
            if (existing == null) return NotFound(id);
            if (existing.SiteId != siteId) return NotFound(id);

            var record = _mapper.Map<Post>(post);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return Ok(Result.Success());
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var existing = await _dbContext.Posts.FindAsync(id);
            if (existing == null) return NotFound(id);
            if (existing.SiteId != siteId) return NotFound(id);

            _dbContext.Posts.Remove(existing);

            await _dbContext.SaveChangesAsync();

            return Ok(Result.Success());
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<PostResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Posts
                .Where(p => p.SiteId == siteId)
                .AsQueryable();

            if (request.OrderDesc != null && request.OrderDesc.Value)
            {
                query = query.OrderByDescending(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }
            else
            {
                query = query.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }

            query = query.Include(p => p.Category);

            return Ok(await PaginatedList<Post>.CreateAsync<PostResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("featured", Name = "[controller]Featured")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<PostResponse>>> Featured([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Posts
                .Where(p => p.IsFeatured && p.SiteId == siteId)
                .AsQueryable();

            if (request.OrderDesc != null && request.OrderDesc.Value)
            {
                query = query.OrderByDescending(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }
            else
            {
                query = query.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }

            query = query.Include(p => p.Category);

            return Ok(await PaginatedList<Post>.CreateAsync<PostResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("tag/{tagSlug}", Name = "[controller]GetByTag")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<PostResponse>>> GetByTag([FromQuery] int siteId, string tagSlug, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Posts
                .Where(p => p.Tags.Any(tag=>(tag.Slug ?? string.Empty).ToLower() == tagSlug.ToLower()) && p.SiteId == siteId)
                .AsQueryable();

            if (request.OrderDesc != null && request.OrderDesc.Value)
            {
                query = query.OrderByDescending(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }
            else
            {
                query = query.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }

            query = query.Include(p => p.Category);

            return Ok(await PaginatedList<Post>.CreateAsync<PostResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("category/{categorySlug}", Name = "[controller]GetByCategory")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<PostResponse>>> GetByCategory([FromQuery] int siteId, string categorySlug, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Posts
                .Where(p => (p.Category.Slug ?? string.Empty).ToLower() == categorySlug.ToLower() && p.SiteId == siteId)
                .AsQueryable();

            if (request.OrderDesc != null && request.OrderDesc.Value)
            {
                query = query.OrderByDescending(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }
            else
            {
                query = query.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "PublishDate"))
                    .AsQueryable();
            }

            query = query.Include(p => p.Category);

            return Ok(await PaginatedList<Post>.CreateAsync<PostResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name="[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PostResponse>> GetById([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null) return NotFound(id);
            if (post.SiteId != siteId) return NotFound(id);

            return Ok(_mapper.Map<PostResponse>(post));
        }
    }
}
