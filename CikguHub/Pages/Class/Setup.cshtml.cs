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

namespace CikguHub.Pages.Class
{
    public class SetupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SetupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Class Class { get; set; }

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

            Class = await _context.Classes
                .Include(c => c.ImageResource)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            //Default values
            //if (String.IsNullOrWhiteSpace(Class.TenantEmail))
            //{
            //    Class.TenantEmail = User.GetUserEmail();
            //}

            if (Class == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
