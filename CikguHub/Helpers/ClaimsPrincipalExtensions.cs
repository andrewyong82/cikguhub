using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CikguHub.Api.Model;

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

        public static List<EnrolmentModel> GetUserEnrolments(this ClaimsPrincipal principal)
        {
            var claims = principal.FindAll("Enrolment");

            List<EnrolmentModel> enrolments = new List<EnrolmentModel>();
            foreach (var claim in claims)
            {
                enrolments.Add((EnrolmentModel)JsonSerializer.Deserialize(claim.Value, typeof(EnrolmentModel)));
            }

            return enrolments;
        }

        public static bool HasClass(this ClaimsPrincipal principal, int classId)
        {
            var cases = principal.GetUserEnrolments();
            return cases.Exists(c => c.ClassId == classId);
        }

        public static string GetFolderName(this ClaimsPrincipal principal)
        {
            return "user-" + principal.GetUserId().ToString("D6");
        }
    }
}
