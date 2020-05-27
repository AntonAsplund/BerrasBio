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
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        [NotMapped]
        public int SeatsLeft { get; set; }
        public List<Ticket> Tickets { get; set; }
        [ForeignKey("Salon")]
        public int SalonId { get; set; }
    }
}
