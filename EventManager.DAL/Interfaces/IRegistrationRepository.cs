using EventManager.Domain.Entities;

namespace EventManager.DAL.Interfaces
{
    public interface IRegistrationRepository : ICrudRepository<int, Registration>
    {
        IEnumerable<Registration> GetByMember(int memberId);
        IEnumerable<Registration> GetByActivity(int activityId);

        Registration? GetByActivityAndMember(int activityId, int memberId);
    }
}
