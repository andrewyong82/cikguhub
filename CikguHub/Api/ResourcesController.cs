using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Claims;
using CikguHub.Helpers;
using CikguHub.Api.Model;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ResourcesController> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public ResourcesController(ApplicationDbContext context, ILogger<ResourcesController> logger, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceModel>>> GetResources(ResourceFilterModel filter)
        {
            var query = _context.Resources
                        .Where(r => !(r.Deleted));

            if (!String.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(r => r.Name.Contains(filter.Search));
            }

            var result = await query
                .OrderByDescending(r => r.Created)
                .Skip((filter.Page - 1) * filter.Size).Take(filter.Size)
                .Select(r => new ResourceModel(r))
                .ToListAsync();

            return result;
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceModel>> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            return new ResourceModel(resource);
        }

        // PUT: api/Resources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, ResourceModel resourceModel)
        {
            if (id != resourceModel.ResourceId)
            {
                return BadRequest();
            }

            Resource resource = resourceModel.UpdateResource(new Resource());
            resource.Modified = DateTime.UtcNow;

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Resources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ResourceModel>> PostResource(ResourceModel resourceModel)
        {
            Resource resource = resourceModel.UpdateResource(new Resource());
            resource.OwnerId = User.GetUserId();
            resource.Modified = DateTime.UtcNow;
            resource.Created = DateTime.UtcNow;
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.ResourceId }, new ResourceModel(resource));
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Resource>> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            resource.Deleted = true;
            _context.Entry(resource).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return resource;
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string uniqueRef)
        {
            var fileType = Path.GetExtension(file.FileName);
            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    Resource resource = new Resource();
                    resource.Name = docName;
                    resource.FileType = fileType;

                    _context.Add(resource);
                    await _context.SaveChangesAsync();

                    BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("resources");
                    await containerClient.UploadBlobAsync(resource.StoragePath, file.OpenReadStream());
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok();
        }
    }
}
