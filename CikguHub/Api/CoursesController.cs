using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using System.IO;
using System.Reflection.Metadata;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using CikguHub.Api.Model;
using AutoMapper;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<CoursesController> _logger;
        private readonly IActivityLogger _activityLogger;

        public CoursesController(
            ApplicationDbContext context, 
            BlobServiceClient blobServiceClient, 
            IConfiguration configuration, 
            IMapper mapper,
            ILogger<CoursesController> logger,
            IActivityLogger activityLogger)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _activityLogger = activityLogger;
        }

        #region to remove

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/CaseRenterDeposits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/CaseRenterDeposits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaseRenterDeposit", new { id = course.CourseId }, course);
        }

        // DELETE: api/CaseRenterDeposits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        #endregion

        // Below actions for wizard --->
        [HttpPost("Step1")]
        public async Task<ActionResult<Course>> PostStep1(List<IFormFile> files, [FromForm]int courseId)
        {
            var course = await _context.Courses.FirstAsync(c => c.CourseId == courseId);
            if (course == null)
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

            string folderName = User.GetFolderName();
            string blobName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName;
            resource.ContainerName = folderName;
            resource.BlobName = blobName;
            resource.StoragePath = folderName + "/" + blobName;

            course.ImageResource = resource;
            course.ImageUrl = _configuration["Azure:BlobHost"] + resource.StoragePath;

            //Save results later...

            //Upload contract to cloud storage
            BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient(folderName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(file.OpenReadStream(), true);

            //Save all results
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpPost("Step2")]
        public async Task<ActionResult<Course>> PostStep2([FromForm] CourseModel model)
        {
            if (model.CourseId == 0)
            {
                return BadRequest();
            }


            var course = await _context.Courses.FindAsync(model.CourseId);

            course = _mapper.Map<CourseModel, Course>(model, course);
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await _activityLogger.LogCaseActivityAsync(model.CourseId, ActivityType.Edited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.CourseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(course);
        }

        [HttpPost("Step3")]
        public async Task<ActionResult<Course>> PostStep3([FromForm] CourseModel model)
        {
            if (model.CourseId == 0)
            {
                return BadRequest();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
            



            return Ok(course);
        }
    }
}