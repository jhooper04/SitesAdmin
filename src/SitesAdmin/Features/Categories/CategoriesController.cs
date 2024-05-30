using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Categories.Dto;
using SitesAdmin.Features.Categories.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Categories
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoriesController(ILogger<CategoriesController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<CategoryResponse>> Create([FromQuery] int siteId, [FromBody] CategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Category>(category);
            record.SiteId = siteId;

            _dbContext.Categories.Add(record);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryResponse>(record));
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<CategoryResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Categories
                .Where(e => e.SiteId == siteId)
                .OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Name"))
                .AsQueryable();

            return Ok(await PaginatedList<Category>.CreateAsync<CategoryResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name = "[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<CategoryResponse>> GetById([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Categories.FindAsync(id);

            if (post == null) return NotFound(id);
            if (post.SiteId != siteId) return NotFound(id);

            return Ok(_mapper.Map<CategoryResponse>(post));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<CategoryResponse>> Update([FromQuery] int siteId, int id, [FromBody] CategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _dbContext.Categories.FindAsync(id);

            if (existing == null) return NotFound(id);
            if (existing.SiteId != siteId) return NotFound(id);

            var record = _mapper.Map<Category>(category);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryResponse>(record));
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return NotFound(id);
            if (category.SiteId != siteId) return NotFound(id);

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
