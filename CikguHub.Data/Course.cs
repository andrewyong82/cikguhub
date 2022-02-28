using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{

    public static class CourseStatus
    {
        public const string New = "New";

        public const string Review = "Review";

        public const string Active = "Active";

        public const string Closed = "Closed";
    }

    public static class ServiceLevel
    {
        public const string Free = "Free";
        public const string Standard = "Standard";
        public const string Premium = "Premium";
    }

    public class Course
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }
        public int? ImageResourceId { get; set; }
        public Resource ImageResource { get; set; }

        [DisplayName("Video Url")]
        public string VideoUrl { get; set; }

        [DisplayName("Chat Channel")]
        public string ChatChannel { get; set; }

        public string Tags { get; set; }

        public int? Duration { get; set; }

        public List<Class> Classes { get; set; }

        public string Status { get; set; } = CourseStatus.New;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }
    }
}
