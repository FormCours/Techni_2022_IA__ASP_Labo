using EventManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventManager.WebAPI.DataTransferObjects
{
    public class MemberRegistrationDTO
    {
        public MemberDTO Member { get; set; }
        public int NbGuest { get; set; }
    }


    public class RegistrationGuestDTO
    {
        [Required]
        [Range(1, 1_000)]
        public int? NbGuest { get; set; }
    }
}
