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

            //var cases = await _context.Cases.Where(c => c.ClientId == user.Id).ToListAsync();
            //List<Claim> caseClaims = new List<Claim>();
            //foreach (Case c in cases)
            //{
            //    Claim caseClaim = new Claim("Case", JsonSerializer.Serialize(c, typeof(Case)));
            //    caseClaims.Add(caseClaim);
            //}

            //((ClaimsIdentity)principal.Identity).AddClaims(caseClaims);

            return principal;
        }
    }
}
