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

        public override string? ToString()
        {
            return "Member {" +
                "\n\t Id: " + Id +
                "\n\t Pseudo: " + Pseudo +
                "\n\t Email: " + Email +
                "\n\t HashPwd: " + HashPwd +
                "\n\t Firstname: " + Firstname +
                "\n\t Lastname: " + Lastname +
                "\n\t Birthdate: " + Birthdate +
            "\n}";
        }
    }
}
