using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Post
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

            var category = _dbContext.Categories.Find(record.CategoryId);
            if (category == null) return BadRequest("Category not found");
            if (category.SiteId != siteId) return BadRequest("Category doesn't belong to this site");

            // lookup tag names and add them to the post
            var tagNames = post.Tags.Split(",");
            var tags = _dbContext.Tags.Where(t => tagNames.Contains(t.Name)).ToList();
            record.Tags.AddRange(tags);

            _dbContext.Posts.Add(record);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = record.Id }, _mapper.Map<PostResponse>(record));
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<PostResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Posts.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Post>.CreateAsync<PostResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name="[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PostResponse>> GetById([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null) return NotFound(id);

            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<PostResponse>> Update([FromQuery] int siteId, int id, [FromBody] PostRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Post>(post);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null) return NotFound(id);

            _dbContext.Posts.Remove(post);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
