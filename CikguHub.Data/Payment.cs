using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public static class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Cancelled = "Cancelled";
        public const string Paid = "Paid";
        public const string Rejected = "Rejected";
        public const string RefundRequested = "RefundRequested";
        public const string Refunded = "Refunded";
    }

    public static class PaymentMethod
    {
        public const string CreditCard = "CreditCard";
        public const string FPX = "FPX";
        public const string DirectDeposit = "DirectDeposit";
        public const string Stripe = "Stripe";
    }

    public static class Products
    {
        public interface IProduct
        {
            string Name { get; }
            decimal Price { get; }
        }

        public class RentalDepositLetterOfDemandStandard : IProduct
        {
            string IProduct.Name => "Rental Deposit Letter of Demand - Standard";

            decimal IProduct.Price => 50;
        }

        public class RentalDepositLetterOfDemandPremium : IProduct
        {
            string IProduct.Name => "Rental Deposit Letter of Demand - Premium";

            decimal IProduct.Price => 200;
        }

        public static IProduct GetProduct(string serviceLevel)
        {
            switch (serviceLevel)
            {
                case ServiceLevel.Standard:
                    return new RentalDepositLetterOfDemandStandard();
                case ServiceLevel.Premium:
                    return new RentalDepositLetterOfDemandPremium();
                default:
                    break;
            }

            return new RentalDepositLetterOfDemandStandard();
        }
    }

    public class Payment
    {
        public int PaymentId { get; set; }
        public string Status { get; set; } = PaymentStatus.Pending;

        public decimal Amount { get; set; }
        public string Method { get; set; }

        public string Product { get; set; }

        public int? CaseId { get; set; }
        public Course Case { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;
    }
}
