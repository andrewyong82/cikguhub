using System;
using System.Collections.Generic;
using System.Text;

namespace CikguHub.Data
{
    public class Class
    {
        public int ClassId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }
    }
}
