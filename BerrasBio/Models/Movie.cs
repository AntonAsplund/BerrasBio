using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{

    public class Movie
        
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int LengthMinute { get; set; }
        public bool IsPlaying { get; set; }
        public string Cathegory { get; set; }
        public int ReleaseYear { get; set; }
        public string Plot { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public int PerentalGuidance { get; set; }
    }
}
