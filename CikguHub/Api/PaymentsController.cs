using CikguHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;
using CikguHub.Api.Model;

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> PostCreate([FromForm] CaseRenterDepositModel model)
        {
            Payment payment = new Payment();
            Products.IProduct product = Products.GetProduct(model.LetterOfDemandServiceLevel);

            payment.Amount = product.Price;
            payment.Product = product.Name;

            payment.Method = CikguHub.Data.PaymentMethod.Stripe;
            payment.CaseId = model.CaseId;

            if (model.CaseRenterDepositId == 0)
            {
                return BadRequest();
            }

            var caseRenterDeposit = await _context.CaseRenterDeposits.FindAsync(model.CaseRenterDepositId);
            caseRenterDeposit.LetterOfDemandServiceLevel = model.LetterOfDemandServiceLevel;
            caseRenterDeposit.LetterOfDemandPayment = payment;
            
            await _context.SaveChangesAsync();

            var domain = "https://" + this.Request.Host.Value;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card", "fpx"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long)Decimal.Multiply(product.Price, 100),
                      Currency = "myr",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = product.Name,
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/payment/success/" + payment.PaymentId,
                CancelUrl = domain + "/payment/cancelled/" + payment.PaymentId,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id });
        }
    }
}
