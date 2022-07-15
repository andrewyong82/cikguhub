using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CikguHub.Pages.Course
{
    public class ListModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;
        private readonly IActivityLogger _activityLogger;

        public ListModel(ApplicationDbContext context, IActivityLogger activityLogger)
        {
            _context = context;
            _activityLogger = activityLogger;
        }

        [BindProperty]
        public List<Data.Course> Courses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Courses = await _context.Courses
                .Where(c => !c.Deleted)
                .ToListAsync();

            return Page();
        }
    }
}
