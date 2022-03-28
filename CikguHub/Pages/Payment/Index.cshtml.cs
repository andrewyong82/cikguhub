using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.BillingPortal;

namespace CikguHub.Pages.Payment
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetCustomerPortal()
        {
            var domain = "https://" + this.Request.Host.Value;

            // Authenticate your user.
            var options = new SessionCreateOptions
            {
                Customer = User.GetCustomerId(),
                ReturnUrl = domain + "/payment",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }
    }
}
