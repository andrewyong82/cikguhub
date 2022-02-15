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
                case CaseStatus.New:
                    return "Setup incomplete";
                case CaseStatus.Review:
                    return "Pending payment";
                case CaseStatus.Active:
                    return "In progress";
                default:
                    return "Case closed";
            }
        }

        public static string GetLabel(this string status)
        {
            switch (status)
            {
                case CaseStatus.New:
                    return "primary";
                case CaseStatus.Review:
                    return "warning";
                case CaseStatus.Active:
                    return "success";
                default:
                    return "";
            }
        }
    }
}
