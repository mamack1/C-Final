namespace C_DiscApp.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Disc> DiscInventory { get; set; }
        public virtual ICollection<GameHistory> GameHistories { get; set; }
    }
}
