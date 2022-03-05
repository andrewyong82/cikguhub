using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public interface IActivityLogger
    {
        Task<Activity> LogActivityAsync(EntityType entityType, int entityId, ActivityType activityType, string data = null, int? userId = null);
        List<Activity> GetActivities(EntityType entityType, int entityId);
        void DeleteActivity(int activityId);
    }

    public class ActivityLogger : IActivityLogger
    {
        ApplicationDbContext _context;

        public ActivityLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> LogActivityAsync(EntityType entityType, int entityId, ActivityType activityType, string data = null, int? userId = null)
        {
            Activity activity = new Activity();
            activity.EntityType = entityType;
            activity.SubjectId = entityId;
            activity.ActivityType = activityType;
            activity.Data = data;
            activity.ActorId = userId;

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return activity;
        }

        public List<Activity> GetActivities(EntityType entityType, int entityId)
        {
            var activities = _context.Activities.Where(a => a.EntityType == entityType && a.SubjectId == entityId && !a.Deleted).OrderBy(a => a.Created).ToList();

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
