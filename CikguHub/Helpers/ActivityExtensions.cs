using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public static class ActivityExtensions
    {
        public static string ToCaseMessage(this Activity activity)
        {
            switch (activity.ActivityType)
            {
                case ActivityType.Created:
                    return "Case has been created";
                case ActivityType.Edited:
                    return "Your case details have been updated"; 
                case ActivityType.Status:
                    return "Case status has been changed to " + activity.Data;
                case ActivityType.Pending:
                    return "Case is now pending action from you";
                case ActivityType.Action:
                    return activity.Data;
                case ActivityType.Progress:
                    return "Your case has progressed";
                case ActivityType.Note:
                    return activity.Data;
                default:
                    break;
            }

            return "";
        }

        public static string ToIcon(this ActivityType activityType)
        {
            switch (activityType)
            {
                case ActivityType.Created:
                    return "flaticon2-plus-1 fl text-warning";
                case ActivityType.Edited:
                    return "flaticon2-checking fl text-info";
                case ActivityType.Status:
                    return "flaticon2-heart-rate-monitor fl text-success";
                case ActivityType.Pending:
                    return "flaticon2-notification fl text-primary";
                case ActivityType.Progress:
                    return "flaticon2-chronometer fl text-primary";
                case ActivityType.Action:
                    return "flaticon2-notification fl text-primary";
                case ActivityType.Note:
                    return "flaticon2-notification fl text-primary";
                default:
                    return "flaticon2-notification fl text-primary";
            }
        }
    }
}
