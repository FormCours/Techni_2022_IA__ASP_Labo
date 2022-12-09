using System.ComponentModel.DataAnnotations;

namespace EventManager.WebAPI.DataTransferObjects
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
    }

    public class MemberDataDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Pseudo { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(250)]
        public string Email { get; set; }

        [MinLength(1)]
        [MaxLength(50)]
        public string? Firstname { get; set; }

        [MinLength(1)]
        [MaxLength(50)]
        public string? Lastname { get; set; }

        public DateTime? Birthdate { get; set; }
    }
}
