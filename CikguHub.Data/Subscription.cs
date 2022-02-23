using System;
using System.Collections.Generic;
using System.Text;

namespace CikguHub.Data
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;
    }
}
