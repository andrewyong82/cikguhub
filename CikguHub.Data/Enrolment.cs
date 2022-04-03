using System;
using System.Collections.Generic;
using System.Text;

namespace CikguHub.Data
{
    public static class EnrolmentStatus
    {
        /// <summary>
        /// User applied but not accepted/paid
        /// </summary>
        public const string New = "New";

        /// <summary>
        /// User applied and accepted/paid
        /// </summary>
        public const string Active = "Active";

        /// <summary>
        /// User ended before completion
        /// </summary>
        public const string Cancelled = "Cancelled";

        /// <summary>
        /// User enrolled but never completed
        /// </summary>
        public const string Expired = "Expired";

        /// <summary>
        /// User enrolled and completed
        /// </summary>
        public const string Completed = "Completed";
    }

    public class Enrolment
    {
        public int EnrolmentId { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Status { get; set; } = EnrolmentStatus.New;

        public string CertificateUrl { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;
    }
}
