using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CikguHub.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            //return principal.FindFirstValue(ClaimTypes.Email);
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return Int32.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static bool IsCurrentUser(this ClaimsPrincipal principal, int id)
        {
            var currentUserId = GetUserId(principal);

            return currentUserId == id;
        }

        public static List<Case> GetUserCases(this ClaimsPrincipal principal)
        {
            var caseClaims = principal.FindAll("Case");

            List<Case> cases = new List<Case>();
            foreach (var caseClaim in caseClaims)
            {
                cases.Add((Case)JsonSerializer.Deserialize(caseClaim.Value, typeof(Case)));
            }

            return cases;
        }

        public static bool HasCase(this ClaimsPrincipal principal, int caseId)
        {
            var cases = principal.GetUserCases();
            return cases.Exists(c => c.CaseId == caseId);
        }

        public static string GetFolderName(this ClaimsPrincipal principal)
        {
            return "user-" + principal.GetUserId().ToString("D8");
        }
    }
}
