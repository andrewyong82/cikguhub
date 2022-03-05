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
        private readonly IActivityLogger _activityLogger;

        public SetupModel(ApplicationDbContext context, IActivityLogger activityLogger)
        {
            _context = context;
            _activityLogger = activityLogger;
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

        public async Task<PartialViewResult> OnGetNewClassPartial(int id, DateTime startDate)
        {
            Course = await _context.Courses
            .FirstOrDefaultAsync(m => m.CourseId == id);

            Data.Class c = new Data.Class();
            c.StartTime = startDate;
            c.Duration = Course.Duration;
            c.Name = Course.Name;
            c.Description = Course.Description;
            c.Content = Course.Content;
            c.ImageResourceId = Course.ImageResourceId;
            c.VideoUrl = Course.VideoUrl;
            c.ChatChannel = Course.ChatChannel;

            _context.Add(c);

            await _context.SaveChangesAsync();

            await _activityLogger.LogActivityAsync(EntityType.Class, c.ClassId, ActivityType.Created);

            return Partial("Partials/_NewClass", c);
        }

    }
}
