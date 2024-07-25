using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net.Sockets;

namespace C_DiscApp.Models
{
    public class ConfigureDiscs : IEntityTypeConfiguration<Disc>
    {

        public void Configure(EntityTypeBuilder<Disc> builder)
        {
            // Configure Name property
            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Configure Type property
            builder.Property(d => d.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            // Configure Weight property
            builder.Property(d => d.Weight)
                   .IsRequired()
                   .HasMaxLength(10);

            // Configure Brand property
            builder.Property(d => d.Brand)
                   .IsRequired()
                   .HasMaxLength(50);

            // Configure Color property
            builder.Property(d => d.Color)
                   .HasMaxLength(30);

            // Configure Speed property
            builder.Property(d => d.Speed)
                   .IsRequired();
            builder.HasCheckConstraint("CK_Disc_Speed", "[Speed] >= 1 AND [Speed] <= 15");

            // Configure Glide property
            builder.Property(d => d.Glide)
                   .IsRequired();
            builder.HasCheckConstraint("CK_Disc_Glide", "[Glide] >= 1 AND [Glide] <= 7");

            // Configure Turn property
            builder.Property(d => d.Turn)
                   .IsRequired();
            builder.HasCheckConstraint("CK_Disc_Turn", "[Turn] >= -5 AND [Turn] <= 5");

            // Configure Fade property
            builder.Property(d => d.Fade)
                   .IsRequired();
            builder.HasCheckConstraint("CK_Disc_Fade", "[Fade] >= 0 AND [Fade] <= 5");

            // Configure Description property
            builder.Property(d => d.Description)
                   .HasMaxLength(500);
        }

    }
}
