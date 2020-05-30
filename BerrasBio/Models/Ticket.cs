using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Seat")]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        [ForeignKey("Viewing")]
        public int ViewingId { get; set; }
        public Viewing Viewing { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
