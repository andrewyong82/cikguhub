using System;
using System.Collections.Generic;
using System.Text;

namespace CikguHub.Data
{
    public class Enrolment
    {
        public int EnrolmentId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int? ClassId { get; set; }
        public Class Class { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;
    }
}
