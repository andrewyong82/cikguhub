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
            Data.Course course = new Data.Course
            {
                Status = CourseStatus.New
            };
            _context.Add(course);

            await _context.SaveChangesAsync();

            await _activityLogger.LogCaseActivityAsync(course.CourseId, ActivityType.Created);

            //ApplicationUser user = _userManager.GetUserAsync(User).Result;
            //await _signInManager.RefreshSignInAsync(user);

            return RedirectToPage("/Course/Setup", new { id = course.CourseId });
        }
    }
}
