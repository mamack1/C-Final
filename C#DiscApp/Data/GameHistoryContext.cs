using Microsoft.EntityFrameworkCore;
using C_DiscApp.Models;

namespace C_DiscApp.Data
{
    public class GameHistoryContext : DbContext
    {
        public GameHistoryContext(DbContextOptions<GameHistoryContext> options)
            : base(options)
        {
        }

        public DbSet<GameHistory> GameHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GameHistory>().HasKey(g => g.Id);
        }
    }
}
