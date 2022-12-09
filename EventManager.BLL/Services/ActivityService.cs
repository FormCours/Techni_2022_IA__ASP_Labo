using EventManager.BLL.Interfaces;
using EventManager.DAL.Interfaces;
using EventManager.Domain.Entities;
using EventManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.BLL.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly IRegistrationRepository registrationRepository;

        public ActivityService(IActivityRepository activityRepository, IRegistrationRepository registrationRepository)
        {
            this.activityRepository = activityRepository;
            this.registrationRepository = registrationRepository;
        }


        public IEnumerable<Activity> GetFutureActivities()
        {
            return activityRepository.GetAll(false);
        }

        public IEnumerable<Activity> GetMemberActivities(int memberId)
        {
            return registrationRepository.GetByMember(memberId).Select(reg => reg.Activity);
        }


        public Activity? GetActivity(int activityId)
        {
            return activityRepository.GetById(activityId);
        }

        public IEnumerable<Registration> GetActivityRegistrations(int activityId)
        {
            return registrationRepository.GetByActivity(activityId);
        }


        public Activity CreateActivity(Activity activity)
        {
            int activityId = activityRepository.Insert(activity);
            return activityRepository.GetById(activityId)!;
        }

        public Activity UpdateActivity(Activity activity)
        {
            activityRepository.Update(activity);
            return activityRepository.GetById(activity.Id)!;
        }

        public bool CancelActivity(int activityId)
        {
            Activity? activity = activityRepository.GetById(activityId);

            if (activity is null)
            {
                return false;
            }

            activity.IsCancel = true;

            return activityRepository.Update(activity);
        }

        public bool DeleteActivity(int activityId)
        {
            return activityRepository.Delete(activityId);
        }


        public RegistrationResult RejoinActivity(int activityId, int memberId, int nbGuest)
        {
            Registration? previousReg = registrationRepository.GetByActivityAndMember(activityId, memberId);
            if(previousReg != null)
            {
                return RegistrationResult.AlreadyExists;
            }

            Activity? activity = activityRepository.GetById(activityId);
            if (activity == null)
            {
                return RegistrationResult.ActivityNotFound;
            }

            int currentNbGuest = registrationRepository.GetByActivity(activityId).Select(r => r.NbGuest).Sum();
            if(activity.MaxGuest < currentNbGuest + nbGuest)
            {
                return RegistrationResult.TooManyGuest;
            }

            registrationRepository.Insert(new Registration()
            {
                ActivityId = activityId,
                MemberId = memberId,
                NbGuest = nbGuest
            });
            return RegistrationResult.Success;
        }

        public RegistrationResult LeaveActivity(int activityId, int memberId)
        {
            Registration? previousReg = registrationRepository.GetByActivityAndMember(activityId, memberId);
            if (previousReg == null)
            {
                return RegistrationResult.NotExists;
            }

            if(registrationRepository.Delete(previousReg.Id))
            {
                return RegistrationResult.Success;
            }
            return RegistrationResult.Error;
        }
    }
}
