using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.Extensions.Configuration;
using CikguHub.Api.Model;
using AutoMapper;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<ClassesController> _logger;
        private readonly IActivityLogger _activityLogger;

        public ClassesController(
            ApplicationDbContext context,
            BlobServiceClient blobServiceClient,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<ClassesController> logger,
            IActivityLogger activityLogger)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _activityLogger = activityLogger;
        }

        // GET: api/Classs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasss()
        {
            return await _context.Classes.ToListAsync();
        }

        // GET: api/CaseRenterDeposits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var c = await _context.Classes.FindAsync(id);

            if (c == null)
            {
                return NotFound();
            }

            return c;
        }

        // PUT: api/CaseRenterDeposits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class c)
        {
            if (id != c.ClassId)
            {
                return BadRequest();
            }

            _context.Entry(c).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/CaseRenterDeposits
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class c)
        {
            _context.Classes.Add(c);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaseRenterDeposit", new { id = c.ClassId }, c);
        }

        // DELETE: api/CaseRenterDeposits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Class>> DeleteClass(int id)
        {
            var c = await _context.Classes.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(c);
            await _context.SaveChangesAsync();

            return c;
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }

        // Below actions for wizard --->
        [HttpPost("Step1")]
        public async Task<ActionResult<Class>> PostStep1(List<IFormFile> files, [FromForm] int classId)
        {
            var c = await _context.Classes.FirstAsync(c => c.ClassId == classId);
            if (c == null)
            {
                return BadRequest();
            }

            if (files == null || files.Count <= 0)
            {
                return BadRequest();
            }

            var file = files.First();
            if (!(file != null && file.Length > 0))
                return BadRequest();


            var fileType = Path.GetExtension(file.FileName);
            if (!(fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg"))
                return BadRequest();

            var fileName = Path.GetFileName(file.FileName);

            //Create Resource
            Resource resource = new Resource();
            resource.Name = fileName;
            resource.FileType = fileType;
            resource.Size = file.Length;
            resource.ContentType = file.ContentType;
            resource.OwnerId = User.GetUserId();

            string folderName = "class-" + c.ClassId.ToString("D4");
            string blobName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName;
            resource.ContainerName = folderName;
            resource.BlobName = blobName;
            resource.StoragePath = folderName + "/" + blobName;

            c.ImageResource = resource;
            c.ImageUrl = _configuration["Azure:BlobHost"] + resource.StoragePath;

            //Save results later...

            //Upload contract to cloud storage
            BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient(folderName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(file.OpenReadStream(), true);

            //Save all results
            await _context.SaveChangesAsync();

            return Ok(c);
        }

        [HttpPost("Step2")]
        public async Task<ActionResult<Class>> PostStep2([FromForm] ClassModel model)
        {
            if (model.ClassId == 0)
            {
                return BadRequest();
            }


            var c = await _context.Classes.FindAsync(model.ClassId);

            c = _mapper.Map<ClassModel, Class>(model, c);


            if (c.Status == ClassStatus.New)
            {
                c.Status = ClassStatus.Review;
                await _activityLogger.LogActivityAsync(EntityType.Class, c.ClassId, ActivityType.Status, ClassStatus.Review);
            }
            else
            {
                await _activityLogger.LogActivityAsync(EntityType.Class, c.ClassId, ActivityType.Edited);
            }

            _context.Entry(c).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await _activityLogger.LogActivityAsync(EntityType.Class, model.ClassId, ActivityType.Edited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(c.ClassId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(c);
        }

        [HttpPost("Step3")]
        public async Task<ActionResult<Class>> PostStep3([FromForm] ClassModel model)
        {
            if (model.ClassId == 0)
            {
                return BadRequest();
            }

            var c = await _context.Classes.FindAsync(model.ClassId);

            c = _mapper.Map<ClassModel, Class>(model, c);
            _context.Entry(c).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await _activityLogger.LogActivityAsync(EntityType.Class, model.ClassId, ActivityType.Edited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(c.ClassId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(c);
        }

        [HttpPost("Final")]
        public async Task<ActionResult<Class>> PostFinal([FromForm] ClassModel model)
        {
            if (model.ClassId == 0)
            {
                return BadRequest();
            }

            var c = await _context.Classes.FindAsync(model.ClassId);

            c = _mapper.Map<ClassModel, Class>(model, c);

            c.Status = ClassStatus.Active;
            await _activityLogger.LogActivityAsync(EntityType.Class, c.ClassId, ActivityType.Status, ClassStatus.Active);

            _context.Entry(c).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(c.ClassId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(c);
        }
    }
}
