using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Project
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProjectsController(ILogger<ProjectsController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<ProjectResponse>> Create([FromQuery] int siteId, [FromBody] ProjectRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Project>(post);

            _dbContext.Projects.Add(record);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = record.Id }, _mapper.Map<ProjectResponse>(record));
        }

        [HttpGet]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<PaginatedList<ProjectResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request = null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Projects.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Project>.CreateAsync<ProjectResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name = "[controller]GetById")]
        [Authorize(Policy = "ApiKeyOrAdmin")]
        public async Task<ActionResult<ProjectResponse>> GetById([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Projects.FindAsync(id);

            if (post == null) return NotFound(id);

            return Ok(_mapper.Map<ProjectResponse>(post));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<ProjectResponse>> Update([FromQuery] int siteId, int id, [FromBody] ProjectRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = _mapper.Map<Project>(post);

            _dbContext.Update(record);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var post = await _dbContext.Projects.FindAsync(id);

            if (post == null) return NotFound(id);

            _dbContext.Projects.Remove(post);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
