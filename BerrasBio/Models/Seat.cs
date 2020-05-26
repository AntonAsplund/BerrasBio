            using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        public int SalonId { get; set; }
        public int SeatNumber { get; set; }
        [NotMapped]
        public bool Booked { get; set; }
    }
}
