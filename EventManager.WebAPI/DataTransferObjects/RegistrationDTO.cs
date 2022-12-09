using EventManager.Domain.Entities;

namespace EventManager.WebAPI.DataTransferObjects
{
    public class MemberRegistrationDTO
    {
        public MemberDTO Member { get; set; }
        public int NbGuest { get; set; }
    }
}
