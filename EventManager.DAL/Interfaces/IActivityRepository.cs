using EventManager.Domain.Entities;

namespace EventManager.DAL.Interfaces
{
    public interface IActivityRepository : ICrudRepository<int, Activity>
    {
        IEnumerable<Activity> GetAll(bool includePast);
    }
}
