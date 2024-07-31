namespace C_DiscApp.Models
{
    public class GameHistory
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public DateTime DatePlayed { get; set; }
        public int NumberOfHoles { get; set; }
        public int TotalParThrows { get; set; }
        public int TotalThrows { get; set; }
        public string UserId { get; set; }

        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }
    }
}