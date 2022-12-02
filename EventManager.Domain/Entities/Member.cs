namespace EventManager.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Email { get; set; }
        public string HashPwd { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
