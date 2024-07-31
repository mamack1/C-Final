namespace C_DiscApp.Models
{
    public class GameHistoryViewModel
    {
        public IEnumerable<GameHistory> GameHistories { get; set; }
        public bool IsAdmin { get; set; }
    }
}
