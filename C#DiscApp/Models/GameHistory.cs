namespace C_DiscApp.Models
{
    public class GameHistory
    {
        public int GameHistoryID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public DateTime DatePlayed { get; set; }
        public int TotalScore { get; set; }
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<HoleScore> HoleScores { get; set; }
    }
}
