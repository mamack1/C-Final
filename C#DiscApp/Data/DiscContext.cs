using C_DiscApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace C_DiscApp.Data
{
    public class DiscContext : IdentityDbContext<User>
    {
        public DiscContext(DbContextOptions<DiscContext> options)
        : base(options)
        {
        }

        public DbSet<Disc> Discs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ConfigureDiscs());

            // Configure the one-to-many relationship
            modelBuilder.Entity<Disc>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
