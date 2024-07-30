using System;
using System.Collections.Generic;

namespace C_DiscApp.Models
{
    public class GameHistory
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public DateTime DatePlayed { get; set; }
        public int NumberOfHoles { get; set; }
        public int TotalParThrows { get; set; }
        public int TotalThrows { get; set; }
    }
}
