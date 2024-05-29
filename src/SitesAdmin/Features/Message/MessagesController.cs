using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Message
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MessagesController(ILogger<MessagesController> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost("send", Name = "[controller]Send")]
        public async Task<ActionResult<MessageResponse>> Create([FromQuery] int siteId, MessageRequest messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = _mapper.Map<Message>(messageDto);

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = message.Id }, _mapper.Map<MessageResponse>(message));
        }

        [HttpGet]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult<PaginatedList<MessageResponse>>> List([FromQuery] int siteId, [FromQuery] PaginatedRequest? request=null)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Messages.OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Message>.CreateAsync<MessageResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Delete([FromQuery] int siteId, int id)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null) return NotFound(id);

            _dbContext.Messages.Remove(message);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
