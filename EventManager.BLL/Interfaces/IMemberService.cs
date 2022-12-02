
using EventManager.Domain.Entities;

namespace EventManager.BLL.Interfaces
{
    public interface IMemberService
    {
        Member? Register(Member member);
        Member? Login(string identifier, string pwd);

        Member? GetMember(int id);
        void UpdateMember(Member member);

        bool CheckAvailableEmail(string email);
        bool CheckAvailablePseudo(string pseudo);
    }
}
