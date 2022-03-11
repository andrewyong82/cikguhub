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
                .Include(c => c.Course.Classes)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (Class == null)
            {
                return NotFound();
            }

            if (Class.Status == ClassStatus.New)
                return RedirectToPage("/class/setup", new { id = Class.ClassId });

            if (Class.Status == ClassStatus.Review)
                return Redirect("/class/" + Class.ClassId.ToString() + "/setup#4");

            return Page();
        }

        public async Task<PartialViewResult> OnGetActivitiesPartial(int id)
        {
            var activities = _activityLogger.GetActivities(EntityType.Class, id);

            return Partial("Partials/_Notes", activities);
        }

        public async Task<PartialViewResult> OnGetActivityPartial(int id, string note)
        {
            var activity = await _activityLogger.LogActivityAsync(EntityType.Class, id, ActivityType.Note, note);

            return Partial("Partials/_Timeline", activity);
        }

        public async Task<ActionResult> OnGetDeleteActivity(int activityId)
        {
            _activityLogger.DeleteActivity(activityId);

            return new OkResult();
        }
    }
}
