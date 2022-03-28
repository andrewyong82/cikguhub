using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CikguHub.Pages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;
        private readonly IActivityLogger _activityLogger;

        public SuccessModel(ApplicationDbContext context, IActivityLogger activityLogger)
        {
            _context = context;
            _activityLogger = activityLogger;
        }

        [BindProperty]
        public Data.Payment Payment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Payment = await _context.Payments.Include(c => c.User).FirstOrDefaultAsync(m => m.PaymentId == id);

            Payment.Status = PaymentStatus.Paid;
            Payment.Modified = DateTime.UtcNow;
            Payment.User.SubscriptionStatus = SubscriptionStatus.Active;

            await _context.SaveChangesAsync();

            await _activityLogger.LogActivityAsync(EntityType.User, Payment.UserId, ActivityType.Status, SubscriptionStatus.Active);

            return RedirectToPage("/Payment/Index");
        }
    }
}
