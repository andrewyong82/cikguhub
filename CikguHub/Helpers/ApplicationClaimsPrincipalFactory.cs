using CikguHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using CikguHub.Api.Model;

namespace CikguHub.Helpers
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>
    {
        private readonly ApplicationDbContext _context;

        public ApplicationClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            ApplicationDbContext context) : base(userManager, roleManager, optionsAccessor)
        {
            _context = context;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            //Add enrolled classes
            var enrolments = await _context.Enrolments.Where(c => c.UserId == user.Id)
                .Select(c => new EnrolmentModel()
                {
                    UserId = c.UserId,
                    ClassId = c.ClassId,
                    ClassName = c.Class.Name,
                    ClassStartTime = c.Class.StartTime,
                    CourseId = c.Class.CourseId,
                    CourseCode = c.Class.Course.Code,
                    CourseName = c.Class.Course.Name,
                    Status = c.Status
                }).ToListAsync();

            List<Claim> claims = new List<Claim>();
            foreach (EnrolmentModel c in enrolments)
            {
                Claim claim = new Claim("Enrolment", JsonSerializer.Serialize(c, typeof(EnrolmentModel)));
                claims.Add(claim);
            }

            //add subscription status
            if (!String.IsNullOrWhiteSpace(user.SubscriptionStatus))
            {
                Claim status = new Claim("SubscriptionStatus", user.SubscriptionStatus);
                claims.Add(status);
            }

            //add stripe customerid
            if (!String.IsNullOrWhiteSpace(user.CustomerId))
            {
                Claim customerId = new Claim("CustomerId", user.CustomerId);
                claims.Add(customerId);
            }

            ((ClaimsIdentity)principal.Identity).AddClaims(claims);

            return principal;
        }
    }
}
