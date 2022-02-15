using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CikguHub.Pages
{
    public static class NavPages
    {
        public static string Home => "Home";

        public static string Profile => "Profile";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Home);

        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "menu-item-active" : null;
        }

        public static string CaseNavClass(ViewContext viewContext, int caseId)
        {
            var activeCase = viewContext.ViewData["ActiveCase"] as int?;
            return activeCase == caseId ? "menu-item-active" : null;
        }
    }
}
