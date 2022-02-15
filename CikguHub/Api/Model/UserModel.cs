using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;

namespace CikguHub.Api.Model
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string BannerUrl { get; set; }

        public string Organisation { get; set; }

        public UserModel()
        {

        }
        public UserModel(ApplicationUser user)
        {
            UserId = user.Id;
            Name = user.Name;
            Description = user.Description;
            ImageUrl = user.ImageUrl;
            BannerUrl = user.BannerUrl;
        }
    }
}
