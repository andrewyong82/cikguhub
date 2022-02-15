using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int RecipientId { get; set; }

        public int ActivityId { get; set; }

        public DateTime Created { get; set; }

        public bool Sent { get; set; }
        public bool Read { get; set; }
    }
}
