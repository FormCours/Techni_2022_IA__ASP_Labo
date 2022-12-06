using EventManager.Domain.Entities;

namespace EventManager.WebAPI.DataTransferObjects.Mappers
{
    public static class ActivityMapper
    {

        public static ActivityDTO ToDTO(this Activity activity)
        {
            return new ActivityDTO()
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                IsCancel = activity.IsCancel,
                MaxGuest = activity.MaxGuest,
                Creator = activity.Creator?.ToDTO()
            };
        }

        public static Activity ToEntity(this ActivityDataDTO activity, int creatorId)
        {
            return new Activity()
            {
                Name = activity.Name,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                ImageName = null,
                ImageSrc = null,
                IsCancel = false,
                MaxGuest = activity.MaxGuest,
                CreatorId = creatorId
            };
        }
    }
}
