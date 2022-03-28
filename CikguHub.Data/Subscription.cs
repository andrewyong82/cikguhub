using System;
using System.Collections.Generic;
using System.Text;

namespace CikguHub.Data
{
    public static class SubscriptionStatus
    {
        public const string Inactive = "Inactive";
        public const string Active = "Active";
        public const string Expired = "Expired";
        public const string Cancelled = "Cancelled";
        public const string Completed = "Completed";
        public const string Suspended = "Suspended";
        public const string PendingCancel = "PendingCancel";
    }

    //public static class Products
    //{
    //    public interface IProduct
    //    {
    //        string Name { get; }
    //        decimal Price { get; }
    //    }

    //    public class MonthlyStandard : IProduct
    //    {
    //        string IProduct.Name => "Standard Membership - Monthly";

    //        decimal IProduct.Price => 50;
    //    }

    //    public class YearlyStandard : IProduct
    //    {
    //        string IProduct.Name => "Standard Membership - Yearly";

    //        decimal IProduct.Price => 200;
    //    }

    //    public static IProduct GetProduct(string serviceLevel)
    //    {
    //        switch (serviceLevel)
    //        {
    //            case ServiceLevel.Standard:
    //                return new RentalDepositLetterOfDemandStandard();
    //            case ServiceLevel.Premium:
    //                return new RentalDepositLetterOfDemandPremium();
    //            default:
    //                break;
    //        }

    //        return new RentalDepositLetterOfDemandStandard();
    //    }
    //}

    public class Subscription
    {
        public int SubscriptionId { get; set; }

        public string Status { get; set; } = SubscriptionStatus.Inactive;

        public bool Recurring { get; set; } = false;

        public int TimePeriod { get; set; } = 0;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CancelDate { get; set; }

        public decimal TotalCost { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;
    }
}
