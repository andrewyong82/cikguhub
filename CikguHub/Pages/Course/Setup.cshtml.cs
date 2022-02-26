using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CikguHub.Pages.Course
{
    public class SetupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SetupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //if (!User.HasCase(id.Value))
            //{
            //    return NotFound();
            //}

            Course = await _context.Courses
                .Include(c => c.ImageResource)
                .Include(c => c.Classes)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            //Default values
            //if (String.IsNullOrWhiteSpace(Course.TenantEmail))
            //{
            //    Course.TenantEmail = User.GetUserEmail();
            //}

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
