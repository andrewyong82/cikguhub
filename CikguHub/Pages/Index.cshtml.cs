using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CikguHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public bool IsEmailConfirmed { get; set; }

        public List<CaseRenterDeposit> Cases { get; set; } = new List<CaseRenterDeposit>();

        //public int TaskCount
        //{
        //    get
        //    {
        //        return (IsEmailConfirmed ? 0 : 1) + (Cases.Count > 0 ? 0 : 1);
        //    }
        //}

        private async Task LoadAsync(ApplicationUser user)
        {
            Cases = await _context.CaseRenterDeposits
                .Include(c => c.Case)
                .Where(m => m.Case.ClientId == user.Id).ToListAsync();

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
