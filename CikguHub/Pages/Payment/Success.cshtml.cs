using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace CikguHub.Pages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;
        private readonly IActivityLogger _activityLogger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SuccessModel(ApplicationDbContext context, IActivityLogger activityLogger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _activityLogger = activityLogger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public Data.Payment Payment { get; set; }

        [BindProperty]
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGet(int? id, string session_id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Payment = await _context.Payments.Include(c => c.User).FirstOrDefaultAsync(m => m.PaymentId == id);

            Payment.Status = PaymentStatus.Paid;
            Payment.Modified = DateTime.UtcNow;
            Payment.User.SubscriptionStatus = SubscriptionStatus.Active;

            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);

            Payment.User.CustomerId = session.CustomerId;
            Payment.CustomerId = session.CustomerId;

            SuccessMessage = $"Thanks for your order, {customer.Name}!";

            await _context.SaveChangesAsync();
            _activityLogger.LogActivityAsync(EntityType.User, Payment.UserId, ActivityType.Status, SubscriptionStatus.Active);

            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            await _signInManager.RefreshSignInAsync(user);

            return Page();
        }
    }
}
