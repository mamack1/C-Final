using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace C_DiscApp.Models
{
    public class Disc
    {
        public int DiscID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // e.g., Driver, Midrange, Putter

        [Required]
        public string Weight { get; set; } // Weight of the disc in grams

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(30)]
        public string Color { get; set; }

        public string ImageUrl { get; set; } // URL to an image of the disc

        [Range(1, 15)]
        public int Speed { get; set; } // Flight number: Speed

        [Range(1, 7)]
        public int Glide { get; set; } // Flight number: Glide

        [Range(-5, 5)]
        public int Turn { get; set; } // Flight number: Turn

        [Range(0, 5)]
        public int Fade { get; set; } // Flight number: Fade

        [StringLength(500)]
        public string Description { get; set; }

        // Foreign Key for User
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
