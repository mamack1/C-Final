namespace C_DiscApp.Models
{
    public class HoleScore
    {
        public int HoleScoreID { get; set; }
        public int GameHistoryID { get; set; }
        public int HoleNumber { get; set; }
        public int Par { get; set; }
        public int Throws { get; set; }
        public virtual GameHistory GameHistory { get; set; }
    }
}
