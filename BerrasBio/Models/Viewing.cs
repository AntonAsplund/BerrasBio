using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{

    public class Viewing
    {
        public int ViewingId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        [NotMapped]
        public int SeatsLeft { get; set; }
    }
}
