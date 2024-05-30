using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;
using SitesAdmin.Features.Tags.Dto;
using SitesAdmin.Features.Tags.Data;

namespace SitesAdmin.Features.Tags
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TagsController(ILogger<TagsController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<TagResponse>> Create([FromQuery] int siteId, [FromBody] TagRequest tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Tag>(tag);

            _dbContext.Tags.Add(record);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = record.Id }, _mapper.Map<TagResponse>(record));
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<TagResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Tags.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Tag>.CreateAsync<TagResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name = "[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<TagResponse>> GetById([FromQuery] int siteId, int id)
        {
            var tag = await _dbContext.Tags.FindAsync(id);

            if (tag == null) return NotFound(id);

            return Ok(_mapper.Map<TagResponse>(tag));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<TagResponse>> Update([FromQuery] int siteId, int id, [FromBody] TagRequest tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Tag>(tag);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var tag = await _dbContext.Tags.FindAsync(id);

            if (tag == null) return NotFound(id);

            _dbContext.Tags.Remove(tag);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
