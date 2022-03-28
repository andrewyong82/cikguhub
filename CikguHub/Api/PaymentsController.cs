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
using System.IO;

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

            var sessionId = Subscribe("price_1KiD93DR88TEzbMmefmpzaYL", "YearlyMembership");
            //var sessionId =  Subscribe("price_1KhtilDR88TEzbMmteJB1xui"); //live

            return Json(new { id = sessionId });
        }

        [HttpPost("SubscribeYearly")]
        public async Task<ActionResult> PostSubscribeYearly()
        {

            var sessionId = Subscribe("price_1KiD93DR88TEzbMmdrJ4z6bC", "YearlyMembership");
            //var sessionId = Subscribe("price_1KhvNbDR88TEzbMmvkiq5n9E"); //live

            return Json(new { id = sessionId });
        }

        private string Subscribe(string priceId, string product)
        {
            Payment payment = new Payment();

            payment.Product = product;
            payment.Method = CikguHub.Data.PaymentMethod.Stripe;
            payment.UserId = User.GetUserId();
            payment.PriceId = priceId;

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

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                var webhookSecret = "whsec_5688265d299e94b8a55bc3576121e8f12ea4f09ea1e4ea7a747cda0dff014b6b";
                stripeEvent = EventUtility.ConstructEvent(
                                json,
                                Request.Headers["Stripe-Signature"],
                                webhookSecret
                            );
                Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something failed {e}");
                return BadRequest();
            }

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    // Payment is successful and the subscription is created.
                    // You should provision the subscription and save the customer ID to your database.
                    break;
                case "invoice.paid":
                    // Continue to provision the subscription as payments continue to be made.
                    // Store the status in your database and check when a user accesses your service.
                    // This approach helps you avoid hitting rate limits.
                    break;
                case "invoice.payment_failed":
                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. Notify your customer and send them to the
                    // customer portal to update their payment information.
                    break;
                default:
                    // Unhandled event type
                    break;
            }

            return Ok();
        }
    }
}
