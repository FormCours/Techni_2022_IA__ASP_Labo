using EventManager.Domain.Entities;

namespace EventManager.WebAPI.DataTransferObjects.Mappers
{
    public class MemberRegistrationDTO
    {
        public Member Member { get; set; }
        public int NbGuest { get; set; }
    }
}
