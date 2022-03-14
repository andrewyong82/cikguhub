using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Api.Model
{
    public class EnrolmentModel
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime ClassStartTime { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
    }
}
