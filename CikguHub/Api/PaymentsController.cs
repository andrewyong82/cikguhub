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
using CikguHub.Helpers;

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

        [HttpPost("SubscribeMonthly")]
        public async Task<ActionResult> PostSubscribeMonthly()
        {

            var sessionId = Subscribe("price_1KiD93DR88TEzbMmefmpzaYL");
            //var sessionId =  Subscribe("price_1KhtilDR88TEzbMmteJB1xui"); //live

            return Json(new { id = sessionId });
        }

        [HttpPost("SubscribeYearly")]
        public async Task<ActionResult> PostSubscribeYearly()
        {

            var sessionId = Subscribe("price_1KiD93DR88TEzbMmdrJ4z6bC");
            //var sessionId = Subscribe("price_1KhvNbDR88TEzbMmvkiq5n9E"); //live

            return Json(new { id = sessionId });
        }

        private string Subscribe(string priceId)
        {
            Payment payment = new Payment();

            payment.Product = priceId;
            payment.Method = CikguHub.Data.PaymentMethod.Stripe;
            payment.UserId = User.GetUserId();

            _context.Payments.Add(payment);

            _context.SaveChanges();


            //var priceId = "price_1KhtilDR88TEzbMmteJB1xui"; //monthly
            //var priceId = "price_1KhvNbDR88TEzbMmvkiq5n9E"; //yearly
            var domain = "https://" + this.Request.Host.Value;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = priceId,
                    Quantity = 1,
                  },
                },
                Mode = "subscription",
                SuccessUrl = domain + "/payment/success/" + payment.PaymentId + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/payment/cancelled/" + payment.PaymentId,
                SubscriptionData = new SessionSubscriptionDataOptions() { TrialPeriodDays = 30 }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Id;
        }
    }
}
