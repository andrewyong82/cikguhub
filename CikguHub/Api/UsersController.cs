using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Api.Model;
using CikguHub.Data;

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        /*        [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }*/

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserModel(user);
        }

/*        // POST: api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
*/
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UserModel model)
        {
            if (model.UserId <= 0)
            {
                return BadRequest();
            }

            ApplicationUser user = new ApplicationUser();
            user.Id = model.UserId;
            user.Name = model.Name;
            user.ImageUrl = model.ImageUrl;
            user.BannerUrl = model.BannerUrl;
            user.Organisation = model.Organisation;

            user.Modified = DateTime.UtcNow;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        /*        // DELETE: api/ApiWithActions/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }*/


        [Route("api/users/{id}/resources")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceModel>>> GetResources(int id)
        {
            return await _context.Resources
                            .Where(c => c.OwnerId == id)
                            .Select(c => new ResourceModel(c))
                            .ToListAsync();
        }
    }
}
