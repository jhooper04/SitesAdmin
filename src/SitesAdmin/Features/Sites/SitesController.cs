using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;
using SitesAdmin.Features.Sites.Dto;
using SitesAdmin.Features.Sites.Data;

namespace SitesAdmin.Features.Sites
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ILogger<SitesController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SitesController(ILogger<SitesController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<SiteResponse>> Create([FromBody] SiteRequest site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Site>(site);

            _dbContext.Sites.Add(record);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = record.Id }, _mapper.Map<SiteResponse>(record));
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<SiteResponse>>> List([FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Sites.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Site>.CreateAsync<SiteResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name = "[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<SiteResponse>> GetById(int id)
        {
            var post = await _dbContext.Sites.FindAsync(id);

            if (post == null) return NotFound(id);

            return Ok(_mapper.Map<SiteResponse>(post));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<SiteResponse>> Update(int id, [FromBody] SiteRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Site>(post);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete(int id)
        {
            var post = await _dbContext.Sites.FindAsync(id);

            if (post == null) return NotFound(id);

            _dbContext.Sites.Remove(post);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
