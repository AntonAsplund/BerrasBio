using BerrasBio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Data
{
    public class SqlTheaterData : ISqlTheaterData
    {
        private readonly TeaterDbContext _context;
        public List<Seat> Seats { get; set; }

        public int ViewingId { get; private set; }

        public SqlTheaterData(TeaterDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> OnGetMovie(int? id)
        {
            Movie movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            return movie;
        }

        public async Task<List<Movie>> OnGetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        public int CreateOrder(List<int> seatIds, int viewingId)
        {
            Order order = new Order();
            order.CustomerName = "Unknown";
            //_context.Add(ticket);
            order.Tickets = new List<Ticket>();
            _context.Add(order);
            int numSaved = _context.SaveChanges();
            foreach (int id in seatIds)
            {
                Ticket ticket = new Ticket();
                ticket.Date = DateTime.Now;
                ticket.SeatId = id;
                ticket.ViewingId = viewingId;
                ticket.OrderId = order.OrderId;
                _context.Add(ticket);
            }
            int ticketsSaved = _context.SaveChanges();
            return order.OrderId;
        }
        public async Task<int> Update(Order order)
        {
            _context.Update(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrder(int Id)
        {
            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == Id);
            _context.Entry(order).Collection(s => s.Tickets).Load();
            foreach (Ticket ticket in order.Tickets)
            {
                _context.Entry(ticket).Reference(x => x.Seat).Load();
                _context.Entry(ticket).Reference(x => x.Viewing).Load();


                _context.Entry(ticket.Viewing).Reference(v => v.Movie).Load();
                _context.Entry(ticket.Viewing).Reference(v => v.Salon).Load();
                ticket.Viewing.FormatedStartTime = ticket.Viewing.StartTime.ToString("dddd HH:mm");

            }
            return order;
        }

        public async Task<List<Viewing>> GetViewingsById(int movieId)
        {
            List<Viewing> viewings = await _context.Viewings.Where(x => x.MovieId == movieId).ToListAsync();
            foreach (Viewing viewing in viewings)
            {
                _context.Entry(viewing).Reference(v => v.Movie).Load();
                _context.Entry(viewing).Reference(v => v.Salon).Load();
                _context.Entry(viewing).Collection(v => v.Tickets).Load();
                viewing.SeatsLeft = 50 - viewing.Tickets.Count;
                viewing.FormatedStartTime = viewing.StartTime.ToString("dddd HH:mm");
            }
            return viewings;
        }

        public async Task<List<Seat>> FindSeats(int id)
        {

            ViewingId = (int)id;

            var seatIds = _context.Tickets.Where(x => x.ViewingId == id).Select(x => x.SeatId);
            int salonId = _context.Viewings.Find(id).SalonId;
            List<Seat> seats = await _context.Seats.Where(x => seatIds.Contains(x.SeatId)).ToListAsync<Seat>();
            Seats = await _context.Seats.Where(x => x.SalonId == salonId).ToListAsync();
            foreach (var item in Seats)
            {
                item.ViewingId = id;
                foreach (var seat in seats)
                {
                    if (item.SeatId == seat.SeatId)
                    {
                        item.Booked = false;
                    }
                }
            }
            
            return Seats;
        }


        public bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
