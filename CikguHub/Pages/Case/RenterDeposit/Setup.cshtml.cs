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
    public class SetupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SetupModel(ApplicationDbContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(m => m.CaseId == id);

            //Default values
            if (String.IsNullOrWhiteSpace(CaseRenterDeposit.TenantEmail))
            {
                CaseRenterDeposit.TenantEmail = User.GetUserEmail();
            }

            if (CaseRenterDeposit == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
