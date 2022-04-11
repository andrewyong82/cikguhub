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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailTemplateSender _emailSender;

        public IndexModel(ApplicationDbContext context, IActivityLogger activityLogger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IEmailTemplateSender emailSender)
        {
            _context = context;
            _activityLogger = activityLogger;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Data.Class Class { get; set; }
        public int ViewCount { get; set; }

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

            await _activityLogger.LogActivityAsync(EntityType.Class, id.Value, ActivityType.Viewed, null, User.GetUserId());
            ViewCount = _activityLogger.GetActivitiesCount(EntityType.Class, id.Value, ActivityType.Viewed);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Data.Enrolment c = new Data.Enrolment
            {
                Status = EnrolmentStatus.Active, //Depends on whether they pay?
                ClassId = id,
                UserId = User.GetUserId(),
            };
            _context.Add(c);

            await _context.SaveChangesAsync();

            await _activityLogger.LogActivityAsync(EntityType.Enrolment, c.EnrolmentId, ActivityType.Created);

            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            await _signInManager.RefreshSignInAsync(user);

            await _emailSender.SendSubscribedEmail(User.GetUserEmail(), User.GetUserName());

            if (User.GetSubscriptionStatus() != SubscriptionStatus.Active)
            {
                RedirectToPage("/Payment/Index");
            }

            return RedirectToPage("/Class/Index", new { id = c.ClassId });
        }
    }
}
