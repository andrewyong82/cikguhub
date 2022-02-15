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

namespace CikguHub.Pages.Case.RenterDeposit
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
        public CaseRenterDeposit CaseRenterDeposit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.HasCase(id.Value))
            {
                return NotFound();
            }

            CaseRenterDeposit = await _context.CaseRenterDeposits
                .Include(c => c.Case)
                .Include(c => c.TenancyContractResource)
                .Include(c => c.Case.Payments)
                .FirstOrDefaultAsync(m => m.CaseId == id);

            if (CaseRenterDeposit == null)
            {
                return NotFound();
            }

            if (CaseRenterDeposit.Case.Status == CaseStatus.New)
                return RedirectToPage("/Case/RenterDeposit/Setup", new { id = CaseRenterDeposit.CaseId });

            if (CaseRenterDeposit.Case.Status == CaseStatus.Review)
                return Redirect("/case/renterDeposit/"  + CaseRenterDeposit.CaseId.ToString() + "/setup#4");

            return Page();
        }

        public async Task<PartialViewResult> OnGetActivitiesPartial(int id)
        {
            var activities = _activityLogger.GetCaseActivities(id);

            return Partial("Partials/_Notes", activities);
        }

        public async Task<PartialViewResult> OnGetActivityPartial(int id, string note)
        {
            var activity = await _activityLogger.LogCaseActivityAsync(id, ActivityType.Note, note);

            return Partial("Partials/_Timeline", activity);
        }

        public async Task<ActionResult> OnGetDeleteActivity(int activityId)
        {
            _activityLogger.DeleteActivity(activityId);

            return new OkResult();
        }
    }
}
