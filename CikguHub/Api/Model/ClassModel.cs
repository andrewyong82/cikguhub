using CikguHub.Data;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Api.Model
{
    public class ClassModel
    {
        public int ClassId { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string VideoUrl { get; set; }

        public string ChatChannel { get; set; }

        public string Tags { get; set; }

        public int CourseId { get; set; }

        public ClassModel() { }

    }
}
