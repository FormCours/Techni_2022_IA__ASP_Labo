using EventManager.Domain.Entities;

namespace EventManager.WebAPI.DataTransferObjects.Mappers
{
    public static class RegistrationMapper
    {

        public static MemberRegistrationDTO ToDTO(this Registration registration)
        {
            return new MemberRegistrationDTO()
            {
                Member = registration.Member.ToDTO(),
                NbGuest = registration.NbGuest
            };
        }
    }
}
