using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public static class CaseType
    {
        public const string RenterDeposit = "RenterDeposit";
    }

    public static class CaseStatus
    {
        /// <summary>
        /// Freshly created.. user in the process of submitting briefing info
        /// </summary>
        public const string New = "New";

        /// <summary>
        /// Details entered.. awaiting checking and payment
        /// </summary>
        public const string Review = "Review";

        /// <summary>
        /// Case formally begins.. -> go to child case status flow
        /// </summary>
        public const string Active = "Active";

        ///// <summary>
        ///// Awaiting review by Fastlaw before we take on the case
        ///// </summary>
        //public const string Review = "Review";

        /// <summary>
        /// All actions have been exhausted by client and fastlaw; just waiting for outcome of case
        /// </summary>
        public const string Pending = "Pending";

        /// <summary>
        /// Case is closed and successful
        /// </summary>
        public const string Closed = "Closed";

        /// <summary>
        /// Case is closed and successful
        /// </summary>
        public const string Failed = "Failed";

        /// <summary>
        /// Case is closed but no outcome; client or Fastlaw decided not to continue
        /// </summary>
        public const string Hold = "Hold";
    }

    public static class ServiceLevel
    {
        public const string Free = "Free";
        public const string Standard = "Standard";
        public const string Premium = "Premium";
    }

    public class Case
    {
        public int CaseId { get; set; }

        public int ClientId { get; set; }
        public ApplicationUser Client { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Status { get; set; } = CaseStatus.New;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }

        public List<Payment> Payments { get; set; }
    }
}
