using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
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

            _context.Classs.Remove(c);
            await _context.SaveChangesAsync();

            return c;
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }
    }
}
