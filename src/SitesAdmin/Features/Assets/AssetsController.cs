﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Data;
using SitesAdmin.Features.Assets.Dto;
using SitesAdmin.Features.Assets.Data;
using SitesAdmin.Features.Common;
using SitesAdmin.Features.Identity;

namespace SitesAdmin.Features.Assets
{
    [Route("[controller]", Name = "[controller][action]")]
    [ApiController]
    [Authorize(Roles = Role.Administrator)]
    public class AssetsController : ControllerBase
    {
        private readonly ILogger<AssetsController> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AssetsController(ILogger<AssetsController> logger, IApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<AssetResponse>> Upload([FromQuery] int siteId, [FromForm] UploadRequest request)
        {
            var uploadsDir = _configuration.GetValue<string>("UploadsDirectory") ?? throw new Exception("Uploads directory not specified in settings");

            var folder = await _dbContext.Folders.Where(f=>f.SiteId == siteId && f.Id == request.FolderId).Include(f => f.Site).FirstOrDefaultAsync();

            if (folder == null) return BadRequest(new { Error= $"Folder not found {request.FolderId}" });

            if (folder.SiteId != siteId) return BadRequest(new { Error = $"Mismatched Site Ids, aborting upload" });

            if (request.File == null) return BadRequest(new { Error = $"File to upload is missing" });

            if (request.File.ContentType != Path.GetExtension(request.File.FileName)) return BadRequest(new { Error = $"File content type doesn't match extension" });

            var dbModel = _mapper.Map<Asset>(request);

            var uniqueFileName = GetUniqueFileName(request.File.FileName);
            var filePath = Path.Combine(uploadsDir, folder.Site.Slug ?? throw new Exception("Should Never happen, invalid slug in Upload Assets"), uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            dbModel.Filename = request.File.FileName;
            dbModel.UniqueFilename = uniqueFileName;
            dbModel.Type = Path.GetExtension(dbModel.Filename);

            _dbContext.Assets.Add(dbModel);

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<AssetResponse>(dbModel));
        }

        [HttpGet("all", Name ="[controller]List")]
        public async Task<ActionResult<PaginatedList<AssetResponse>>> List([FromQuery] PaginatedRequest request, [FromQuery] int siteId)
        {
            if (request == null) request = new PaginatedRequest();

            var query = _dbContext.Assets
                .Where(a => a.SiteId == siteId)
                .OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Asset>.CreateAsync<AssetResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("folder", Name = "[controller]GetByFolder")]
        public async Task<ActionResult<PaginatedList<AssetResponse>>> GetByFolder([FromQuery] int folderId, [FromQuery] PaginatedRequest request, [FromQuery] int siteId)
        {
            if (request == null) request = new PaginatedRequest();

            var folder = await _dbContext.Folders.FindAsync(folderId);

            if (folder == null) return BadRequest(new { Error = $"Folder not found {folderId}" });

            if (folder.SiteId != siteId) BadRequest(new { Error = $"Site Ids don't match the folder {folderId}" });

            var query = _dbContext.Assets
                .Where(a => a.FolderId == folderId && a.SiteId == siteId)
                .OrderBy(p => EF.Property<object>(p, request.OrderBy ?? "Id")).AsQueryable();

            return Ok(await PaginatedList<Asset>.CreateAsync<AssetResponse>(_mapper, query, request.PageNumber, request.PageSize));
        }

        [HttpGet("{id}", Name = "[controller]GetById")]
        public async Task<ActionResult<AssetResponse>> GetById(int id, [FromQuery] int siteId)
        {
            var model = await _dbContext.Assets.FindAsync(id);

            if (model == null) return NotFound(id);

            if (model.SiteId != siteId) return BadRequest(new { Error = $"Site Ids don't match the asset {model.Id} at site {model.SiteId}" });

            return Ok(_mapper.Map<AssetResponse>(model));
        }

        [HttpPost("{id}", Name = "[controller]Update")]
        public async Task<ActionResult<AssetResponse>> Update(int id, AssetRequest model, [FromQuery] int siteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _dbContext.Assets.FindAsync(id);

            if (existing == null) return NotFound(id);

            if (existing.SiteId != siteId) return NotFound(id);

            var dbModel = _mapper.Map<Asset>(model);

            _dbContext.Update(dbModel);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "[controller]Delete")]
        public async Task<ActionResult> Delete(int id, [FromQuery] int siteId)
        {
            var model = await _dbContext.Assets.FindAsync(id);

            if (model == null) return NotFound(id);

            if (model.SiteId != siteId) return BadRequest(new { Error = $"Site Ids don't match the asset {model.Id} at site {model.SiteId}" });

            _dbContext.Assets.Remove(model);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet(Name = "[controller]GetAllFolders")]
        public async Task<ActionResult<List<FolderResponse>>> GetAllFolders([FromQuery] int siteId)
        {
            var rootFolders = _mapper.Map<List<FolderResponse>>(
                await _dbContext.Folders
                .Where(f => f.ParentFolderId == null && f.SiteId == siteId)
                .ToListAsync()
            );

            foreach (var folder in rootFolders)
            {
                await LoadSubFoldersAsync(folder, siteId);
            }

            return Ok(rootFolders);
        }

        private async Task LoadSubFoldersAsync(FolderResponse folder, int siteId)
        {
            var subFolders = _mapper.Map<List<FolderResponse>>(
                await _dbContext.Folders
                .Where(f => f.ParentFolderId == folder.Id && f.SiteId == siteId)
                .ToListAsync()
            );

            foreach (var subFolder in subFolders)
            {
                folder.SubFolders.Add(subFolder);
                await LoadSubFoldersAsync(subFolder, siteId);
            }
        }

        [HttpPost("folder", Name = "[controller]CreateFolder")]
        public async Task<ActionResult<FolderResponse>> CreateFolder([FromQuery] int siteId, [FromBody] FolderRequest request)
        {
            if (request.ParentFolderId != null)
            {
                var parentFolder = await _dbContext.Folders.Where(f => f.SiteId == siteId && f.Id == request.ParentFolderId).Include(f => f.Site).FirstOrDefaultAsync();

                if (parentFolder == null) return BadRequest(new { Error = $"Parent folder not found {request.ParentFolderId}" });

                if (parentFolder.SiteId != siteId) return BadRequest(new { Error = $"Mismatched Site Ids, aborting upload" });
            }

            if (request.ParentFolderId == 0) request.ParentFolderId = null;

            var dbModel = _mapper.Map<Folder>(request);
            
            dbModel.SiteId = siteId;
            SlugUtil.SetDefaultSlug(dbModel);

            _dbContext.Folders.Add(dbModel);

            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<FolderResponse>(dbModel));
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = SlugUtil.Slugify(Path.GetFileName(fileName));
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
