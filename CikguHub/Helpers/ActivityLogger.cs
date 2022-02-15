using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public interface IActivityLogger
    {
        Task<Activity> LogCaseActivityAsync(int caseId, ActivityType activityType, string data = null, int? userId = null);
        List<Activity> GetCaseActivities(int caseId);
        void DeleteActivity(int activityId);
    }

    public class ActivityLogger : IActivityLogger
    {
        ApplicationDbContext _context;

        public ActivityLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> LogCaseActivityAsync(int caseId, ActivityType activityType, string data = null, int? userId = null)
        {
            Activity activity = new Activity();
            activity.EntityType = EntityType.Case;
            activity.SubjectId = caseId;
            activity.ActivityType = activityType;
            activity.Data = data;
            activity.ActorId = userId;

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return activity;
        }

        public List<Activity> GetCaseActivities(int caseId)
        {
            var activities = _context.Activities.Where(a => a.EntityType == EntityType.Case && a.SubjectId == caseId && !a.Deleted).OrderBy(a => a.Created).ToList();

            return activities;
        }

        public void DeleteActivity(int activityId) 
        {
            Activity activity = new Activity();
            activity.ActivityId = activityId;
            activity.Deleted = true;

            _context.Activities.Attach(activity);
            _context.Entry(activity).Property(a => a.Deleted).IsModified = true;
            _context.SaveChanges();
        }
    }
}
