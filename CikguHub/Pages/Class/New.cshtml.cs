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
    public class NewModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IActivityLogger _activityLogger;

        public NewModel(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IActivityLogger activityLogger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Data.Class c = new Data.Class
            {
                Status = ClassStatus.New
            };
            _context.Add(c);

            await _context.SaveChangesAsync();

            await _activityLogger.LogCaseActivityAsync(c.ClassId, ActivityType.Created);

            //ApplicationUser user = _userManager.GetUserAsync(User).Result;
            //await _signInManager.RefreshSignInAsync(user);

            return RedirectToPage("/Class/Setup", new { id = c.ClassId });
        }
    }
}
