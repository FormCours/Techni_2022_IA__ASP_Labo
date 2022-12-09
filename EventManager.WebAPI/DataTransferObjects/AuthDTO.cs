using System.ComponentModel.DataAnnotations;

namespace EventManager.WebAPI.DataTransferObjects
{
    public class AuthLoginDTO
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthRegisterDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Pseudo { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).{5,}")]
        public string Password { get; set; }
    }

    public class AuthTokenDTO
    {
        public string Token { get; set; }
    }
}
