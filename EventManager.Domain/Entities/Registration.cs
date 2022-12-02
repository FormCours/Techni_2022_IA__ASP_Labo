namespace EventManager.Domain.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int MemberId { get; set; }
        public int NbGuest { get; set; }
        
        public Activity Activity { get; set; }
        public Member Member { get; set; }

        public override string? ToString()
        {
            return "Registration {" +
                "\n\t Id: " + Id +
                "\n\t ActivityId: " + ActivityId +
                "\n\t MemberId: " + MemberId +
                "\n\t NbGuest: " + NbGuest +
            "\n}";
        }
    }
}
