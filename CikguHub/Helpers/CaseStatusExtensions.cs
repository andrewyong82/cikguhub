using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public static class CaseStatusExtensions
    {
        public static string GetDescription(this string status)
        {
            switch (status)
            {
                case CourseStatus.New:
                    return "Setup incomplete";
                case CourseStatus.Review:
                    return "Pending payment";
                case CourseStatus.Active:
                    return "In progress";
                default:
                    return "Case closed";
            }
        }

        public static string GetLabel(this string status)
        {
            switch (status)
            {
                case CourseStatus.New:
                    return "primary";
                case CourseStatus.Review:
                    return "warning";
                case CourseStatus.Active:
                    return "success";
                case CourseStatus.Closed:
                    return "danger";
                default:
                    return "";
            }
        }
    }
}
