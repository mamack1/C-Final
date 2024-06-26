using System.ComponentModel.DataAnnotations;

namespace C_DiscApp.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Range(1, 10)]
        public int Ranking { get; set; } // Course ranking from 1 to 10

        public virtual ICollection<GameHistory> GameHistories { get; set; }
    }
}
