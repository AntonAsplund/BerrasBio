using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Salon
    {
        public int SalonId { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
