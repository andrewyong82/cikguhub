using CikguHub.Data;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Api.Model
{
    public class CourseModel
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string VideoUrl { get; set; }

        public string ChatChannel { get; set; }

        public string Tags { get; set; }

        public int? Duration { get; set; }

        public List<Class> Classes { get; set; }

        public CourseModel() { }

    }
}
