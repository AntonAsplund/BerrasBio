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
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Viewing> Viewings { get; set; }


        public TeaterDbContext(DbContextOptions<TeaterDbContext> options)
            : base(options)
        {

        }


        public DbSet<BerrasBio.Models.Order> Order { get; set; }


    }
}
