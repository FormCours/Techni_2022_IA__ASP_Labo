using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.BLL.Interfaces
{
    public interface IActivityService
    {
        IEnumerable<Activity> GetFutureActivities();
        IEnumerable<Activity> GetMemberActivities(int memberId);

        Activity? GetActivity(int activityId);
        IEnumerable<Registration> GetActivityRegistrations(int activityId);

        Activity CreateActivity(Activity activity);
        Activity UpdateActivity(Activity activity);
        bool CancelActivity(int activityId);
        bool DeleteActivity(int activityId);

        bool RejoinActivity(int activityId, int memberId, int nbGuest);
        bool LeaveActivity(int activityId, int memberId);
    }
}
