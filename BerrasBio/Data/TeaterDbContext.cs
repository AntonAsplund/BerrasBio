using BerrasBio.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Data
{
    public class TeaterDbContext : DbContext
    {
        public List<Movie> Movies { get; set; }
        public List<Salon> Salons { get; set; }
        public List<Seat> Seats { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Viewing> Viewings { get; set; }

    }
}
