using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{

    public class Movie
        
    {
        public int MovieId { get; set; }
        public int LengthMinute { get; set; }
        public bool IsPlaying { get; set; }
    }
}
