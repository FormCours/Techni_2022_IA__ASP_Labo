using EventManager.Domain.Entities;

namespace EventManager.WebAPI.DataTransferObjects.Mappers
{
    public static class MemberMapper
    {
        public static MemberDTO ToDTO(this Member member)
        {
            return new MemberDTO()
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                Email = member.Email,
                Firstname = member.Firstname,
                Lastname = member.Lastname,
                Birthdate = member.Birthdate
            };
        }

        public static Member ToEntity(this MemberDataDTO member)
        {
            return new Member()
            {
                Id = 0,
                Pseudo = member.Pseudo,
                Email = member.Email,
                Firstname = member.Firstname,
                Lastname = member.Lastname,
                Birthdate = member.Birthdate
            };
        }
    }
}
