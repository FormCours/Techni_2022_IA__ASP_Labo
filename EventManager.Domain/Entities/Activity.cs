namespace EventManager.Domain.Entities
{
    public class Activity
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? ImageName { get; set; }
		public string? ImageSrc { get; set; }
		public int? MaxGuest { get; set; }
		public int CreatorId { get; set; }
		public bool IsCancel { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		public Member Creator { get; set; }

        public override string? ToString()
        {
            return "Activity {" +
                "\n\t Id: " + Id +
                "\n\t Name: " + Name +
                "\n\t Description: " + Description +
                "\n\t StartDate: " + StartDate +
                "\n\t EndDate: " + EndDate +
                "\n\t ImageName: " + ImageName +
                "\n\t ImageSrc: " + ImageSrc +
                "\n\t MaxGuest: " + MaxGuest +
                "\n\t CreatorId: " + CreatorId +
                "\n\t IsCancel: " + IsCancel +
                "\n\t CreateDate: " + CreateDate +
                "\n\t UpdateDate: " + UpdateDate +
            "\n}";
        }
    }
}
