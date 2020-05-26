using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime Date { get; set; }
        public int SeatId { get; set; }
        public int ViewingId { get; set; }
    }
}
