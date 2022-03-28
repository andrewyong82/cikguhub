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

        public static string Payment => "Payment";

        public static string Browse => "Browse";

        public static string Courses => "Courses";

        public static string Classes => "Classes";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Home);

        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

        public static string PaymentNavClass(ViewContext viewContext) => PageNavClass(viewContext, Payment);

        public static string BrowseNavClass(ViewContext viewContext) => PageNavClass(viewContext, Browse);

        public static string CoursesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Courses);

        public static string ClassesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Classes);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "menu-item-active" : null;
        }

        public static string CaseNavClass(ViewContext viewContext, int classId)
        {
            var activeClass = viewContext.ViewData["ActiveClass"] as int?;
            return activeClass == classId ? "menu-item-active" : null;
        }
    }
}
