using EventManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventManager.WebAPI.DataTransferObjects
{
    public class ActivityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxGuest { get; set; }
        public bool IsCancel { get; set; }
        public MemberDTO? Creator { get; set; }
    }

    public class ActivityDataDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, 1_000_000)]
        public int? MaxGuest { get; set; }
    }

    public class ActivityImageDataDTO
    {
        [Required]
        public IFormFile ActivityImage { get; set; }
    }

    public class ActivityImageDTO
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
