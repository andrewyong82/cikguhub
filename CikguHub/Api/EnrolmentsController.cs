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
    public class EnrolmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrolmentsController> _logger;
        private readonly IActivityLogger _activityLogger;

        public EnrolmentsController(
            ApplicationDbContext context,
            BlobServiceClient blobServiceClient,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<EnrolmentsController> logger,
            IActivityLogger activityLogger)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _activityLogger = activityLogger;
        }

        // GET: api/Enrolments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
        {
            return await _context.Enrolments.ToListAsync();
        }

        // GET: api/CaseRenterDeposits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
        {
            var c = await _context.Enrolments.FindAsync(id);

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
        public async Task<IActionResult> PutEnrolment(int id, Enrolment c)
        {
            if (id != c.EnrolmentId)
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
                if (!EnrolmentExists(id))
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
        public async Task<ActionResult<Enrolment>> PostEnrolment(Enrolment c)
        {
            _context.Enrolments.Add(c);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaseRenterDeposit", new { id = c.EnrolmentId }, c);
        }

        // DELETE: api/CaseRenterDeposits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Enrolment>> DeleteEnrolment(int id)
        {
            var c = await _context.Enrolments.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }

            _context.Enrolments.Remove(c);
            await _context.SaveChangesAsync();

            return c;
        }

        private bool EnrolmentExists(int id)
        {
            return _context.Enrolments.Any(e => e.EnrolmentId == id);
        }
    }
}
