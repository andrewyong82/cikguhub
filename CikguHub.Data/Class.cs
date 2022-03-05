using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CikguHub.Data
{
    public static class ClassStatus
    {
        public const string New = "New";

        public const string Review = "Review";

        public const string Active = "Active";

        public const string Closed = "Closed";
    }
    public static class Language
    {
        public const string EN = "EN";

        public const string BM = "BM";
    }

    public class Class
    {
        public int ClassId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

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

        public List<Enrolment> Enrolments { get; set; }

        public string Status { get; set; } = ClassStatus.New;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }
    }
}
