using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CikguHub.Pages.Payment
{
    public class CancelledModel : PageModel
    {
        private readonly CikguHub.Data.ApplicationDbContext _context;

        public CancelledModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Payment Payment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Payment = await _context.Payments.Include(c => c.Case).FirstOrDefaultAsync(m => m.PaymentId == id);

            Payment.Status = PaymentStatus.Cancelled;
            Payment.Modified = DateTime.UtcNow;
            Payment.Case.Status = CourseStatus.Review;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Case/RenterDeposit/Index", new { id = Payment.CaseId });
        }
    }
}
