using EventManager.Domain.Entities;

namespace EventManager.DAL.Interfaces
{
    public interface IMemberRepository : ICrudRepository<int, Member>
    {
        Member? GetByIdentifier(string identifier);
        string? GetHashPwd(int id);

        bool EmailExists(string email);
        bool PseudoExists(string pseudo);
    }
}
