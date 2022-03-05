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
    public class IndexModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;
        private readonly IActivityLogger _activityLogger;

        public IndexModel(ApplicationDbContext context, IActivityLogger activityLogger)
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
                .Include(c => c.Classes)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (Course == null)
            {
                return NotFound();
            }

            if (Course.Status == CourseStatus.New)
                return RedirectToPage("/course/setup", new { id = Course.CourseId });

            if (Course.Status == CourseStatus.Active)
                return Redirect("/course/" + Course.CourseId.ToString() + "/setup#4");

            return Page();
        }

        public async Task<PartialViewResult> OnGetActivitiesPartial(int id)
        {
            var activities = _activityLogger.GetActivities(EntityType.Course, id);

            return Partial("Partials/_Notes", activities);
        }

        public async Task<PartialViewResult> OnGetActivityPartial(int id, string note)
        {
            var activity = await _activityLogger.LogActivityAsync(EntityType.Course, id, ActivityType.Note, note);

            return Partial("Partials/_Timeline", activity);
        }

        public async Task<ActionResult> OnGetDeleteActivity(int activityId)
        {
            _activityLogger.DeleteActivity(activityId);

            return new OkResult();
        }
    }
}
