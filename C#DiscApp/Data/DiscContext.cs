using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using C_DiscApp.Models;
using Microsoft.EntityFrameworkCore;

namespace C_DiscApp.Data
{
    public class DiscContext : IdentityDbContext<User>
    {
        public DiscContext(DbContextOptions<DiscContext> options) : base(options)
        {
        }

        public DbSet<Disc> Discs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Disc>()
                .HasKey(d => d.DiscID);

            modelBuilder.Entity<Disc>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Disc>()
                .Property(d => d.Type)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Disc>()
                .Property(d => d.Weight)
                .IsRequired();

            modelBuilder.Entity<Disc>()
                .Property(d => d.Brand)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Disc>()
                .Property(d => d.Color)
                .HasMaxLength(30);

            modelBuilder.Entity<Disc>()
                .Property(d => d.ImageUrl)
                .HasMaxLength(2083);

            modelBuilder.Entity<Disc>()
                .Property(d => d.Speed)
                .IsRequired();

            modelBuilder.Entity<Disc>()
                .Property(d => d.Glide)
                .IsRequired();

            modelBuilder.Entity<Disc>()
                .Property(d => d.Turn)
                .IsRequired();

            modelBuilder.Entity<Disc>()
                .Property(d => d.Fade)
                .IsRequired();

            modelBuilder.Entity<Disc>()
                .Property(d => d.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Disc>()
                .Property(d => d.UserId)
                .IsRequired();
        }
    }
}