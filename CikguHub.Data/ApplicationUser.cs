using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Organisation { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string BannerUrl { get; set; }

        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
    }
}
