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
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Order { get; set; }


        public TeaterDbContext(DbContextOptions<TeaterDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne<Viewing>(s => s.Viewing)
                .WithMany(ta => ta.Tickets)
                .HasForeignKey(u => u.ViewingId)
                .OnDelete(DeleteBehavior.NoAction);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetForeignKeys())
        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            base.OnModelCreating(modelBuilder);
        }

    }
}
