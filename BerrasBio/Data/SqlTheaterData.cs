﻿using BerrasBio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<List<Movie>> OnGetMovies(bool includeOld)
        {
            if (includeOld)
            {
                return await _context.Movies.ToListAsync();
            }
            else
            {
                return await _context.Movies.Where(m => m.IsPlaying).ToListAsync();

            }
        }

        public async Task<User> OnGetUser(int? id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task<List<User>> OnGetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public Order CreateOrder(List<int> seatIds, int viewingId, string userName)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            Order order = new Order();
            order.Tickets = new List<Ticket>();
            _context.Add(order);
            order.UserId = user.UserId;
            order.User = user;
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
            return order;
        }

        public List<int> GetSeatNumbers(int[] seatIds)
        {
            List<int> seatNumbers = new List<int>();

            foreach (int id in seatIds)
            {
                seatNumbers.Add(_context.Seats.FirstOrDefault(s => s.SeatId == id).SeatNumber);
            }

            return seatNumbers;
        }

        public void LoadOrder(Order order)
        {
            //  _context.Entry(order).Collection(o => o.Tickets).Load();
            _context.Entry(order).Reference(o => o.User).Load();
        }


        private User CreateUser()
        {
            User user = new User();
            user.Password = "password";
            user.PhoneNumber = "333";
            user.IsAdmin = false;
            user.UserName = "Daniel";
            _context.Add(user);
            _context.SaveChanges();

            return user;
        }

        public int Update()
        {
            int ticketsSaved = _context.SaveChanges();
            return ticketsSaved;
        }
        /// <summary>
        /// Adds user to database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public EntityEntry<User> AddUser(User user)
        {
            return _context.Users.Add(user);
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
            _context.Entry(order).Reference(o => o.User).Load();
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

        public async Task<List<Viewing>> GetViewingsById(int movieId, string order)
        {
            List<Viewing> viewings = await _context.Viewings.Where(x => x.MovieId == movieId).Where(x => x.StartTime > DateTime.UtcNow).ToListAsync();
            foreach (Viewing viewing in viewings)
            {
                _context.Entry(viewing).Reference(v => v.Movie).Load();
                _context.Entry(viewing).Reference(v => v.Salon).Load();
                _context.Entry(viewing).Collection(v => v.Tickets).Load();
                viewing.SeatsLeft = 50 - viewing.Tickets.Count;
                viewing.FormatedStartTime = viewing.StartTime.ToString("dddd HH:mm");
            }
            if (order == "Num")
            {
                return viewings.OrderBy(x => x.SeatsLeft).ToList();
            }
            else
            {
                return viewings.OrderBy(x => x.StartTime).ToList();
            }
        }

        public bool MovieHasViewings(int movieId)
        {
            return _context.Viewings.Where(v => v.MovieId == movieId).Any();

        }

        public async Task<List<Seat>> FindSeats(int id)
        {

            ViewingId = (int)id;

            var seatIds = _context.Tickets.Where(x => x.ViewingId == id).Select(x => x.SeatId);
            int salonId = _context.Viewings.Find(id).SalonId;
            List<Seat> seats = await _context.Seats.Where(x => seatIds.Contains(x.SeatId)).ToListAsync<Seat>();
            Seats = await _context.Seats.Where(x => x.SalonId == salonId).ToListAsync();
            _context.Entry(Seats.First()).Reference(v => v.Salon).Load();

            foreach (var item in Seats)
            {
                item.ViewingId = id;
                foreach (var seat in seats)
                {
                    if (item.SeatId == seat.SeatId)
                    {
                        item.Booked = true;
                    }
                }
            }

            int viewingId = Seats.First().ViewingId;
            // Viewing viewing = _context.Viewings.Find(viewingId);
            // _context.Entry(viewing).Reference(v => v.Movie).Load();
            // Seats.First().MovieTitle = viewing.Movie.Title;

            return Seats;
        }
        public async Task<Movie> FindMovie(int? id)
        {
            return await _context.Movies.FindAsync(id);
        }
        public async Task UpdateMovie(Movie movie)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMovieAt(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            //_context.Movies.Remove(movie);
            movie.IsPlaying = false;
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }
        public bool DoesMovieExist(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
        public bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        public Movie AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie;
        }

        public List<Salon> GetSalons()
        {
            return _context.Salons.ToList();
        }

        public bool CreateViewing(Viewing viewing)
        {
            _context.Viewings.Add(viewing);
            _context.SaveChanges();
            _context.Entry(viewing).Reference(v => v.Movie).Load();
            viewing.Movie.IsPlaying = true;
            _context.SaveChanges();
            return true;
        }

        public Viewing GetViewingById(int id)
        {
            Viewing viewing = _context.Viewings.Include("Movie").Include("Salon").FirstOrDefault(v => v.ViewingId == id);

            viewing.FormatedStartTime = viewing.StartTime.ToString("dddd HH:mm");

            return viewing;
        }

        public async Task<bool> OnDeleteMovie(int id)
        {
            Movie movie = await OnGetMovie(id);

            if (movie == null)
                return false;

            List<Viewing> viewings = _context.Viewings.Include("Movie").Include("Tickets").Where(v => v.MovieId == id).ToList();

            if(viewings.Count > 1)
            {

                foreach(Viewing viewing in viewings)
                {
                    if (viewing.Tickets.Count != 0)
                    {
                        IEnumerable<Ticket> tickets = _context.Tickets.Where(t => t.ViewingId == viewing.ViewingId);
                        _context.Tickets.RemoveRange(tickets);
                        _context.Viewings.Remove(viewing);
                        await _context.SaveChangesAsync();
                    }

                    else
                    {
                        _context.Viewings.Remove(viewing);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }

            else if(viewings.Count == 1)
            {
                if (viewings[0].Tickets.Count != 0)
                {
                    IEnumerable<Ticket> tickets = _context.Tickets.Where(t => t.ViewingId == viewings[0].ViewingId);
                    _context.Tickets.RemoveRange(tickets);
                    _context.Viewings.Remove(viewings[0]);
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    return true;
                }

                else
                {
                    _context.Viewings.Remove(viewings[0]);
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            else
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
