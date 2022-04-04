using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public enum EntityType
    {
        User = 1,
        Course = 2,
        Class = 3,
        Enrolment = 4,
        Payment = 5
    }

    public enum ActivityType
    {
        Created = 10,
        Edited = 20,
        Status = 30,
        Pending = 40,
        Progress = 50,
        Action = 60,
        Note = 70,
        Viewed = 80,
    }

    public class Activity
    {
        public int ActivityId { get; set; }

        public int? ActorId { get; set; }
        public ApplicationUser Actor { get; set; }

        public EntityType EntityType { get; set; }
        public int SubjectId { get; set; }

        public ActivityType ActivityType { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }

        public string Data { get; set; }

        public bool Notified { get; set; }
    }
}
