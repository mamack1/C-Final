using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace C_DiscApp.Models
{
    public class Disc
    {
        public int DiscID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [Range(1, 200)]
        public int Weight { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(30)]
        public string Color { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Range(1, 15)]
        public int Speed { get; set; }

        [Range(1, 7)]
        public int Glide { get; set; }

        [Range(-5, 5)]
        public int Turn { get; set; }

        [Range(0, 5)]
        public int Fade { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}