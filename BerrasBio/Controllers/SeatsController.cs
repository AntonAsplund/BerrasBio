using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Migrations;

namespace BerrasBio.Controllers
{
    public class SeatsController : Controller
    {
        public Viewing Viewing { get; set; }
        public List<Seat> Seats { get; set; }

        private readonly TeaterDbContext _context;
        /// <summary>
        /// Will add everytime user click a seat.
        /// </summary>
        public int NumBookedSeats { get; set; }
        // todo: Check how to add a button and method to controll it.
        public SeatsController(TeaterDbContext context)
        {
            _context = context;
        }

        // GET: Seats
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Seats = await FindSeats((int) id);
                return View(Seats);
            }
        }

        public IActionResult Booked(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Console.WriteLine("checked");
            Seat seat = _context.Seats.Find(id);
            seat.Booked = seat.Booked!;
            Order order = new Order();
            order.CustomerName = "Unknown";
            Ticket ticket = new Ticket();
            ticket.Date = DateTime.Now;
            ticket.SeatId = (int) id;


            order.Tickets = new List<Ticket>();
            var orderId = _context.Add(order);
            //orderId.
            //_context.SaveChangesAsync();
            string url = String.Format($"../../Orders/index");
            return Redirect(url);

        }

        private async Task<List<Seat>> FindSeats(int id)
        {
            var seatIds = _context.Tickets.Where(x => x.ViewingId == id).Select(x => x.SeatId);
            int salonId = _context.Viewings.Find(id).SalonId;
            List<Seat> seats = await _context.Seats.Where(x => seatIds.Contains(x.SeatId)).ToListAsync<Seat>();
            List<Seat> salonSeats = await _context.Seats.Where(x => x.SalonId == salonId).ToListAsync();
            foreach (var item in salonSeats)
            {
                foreach (var seat in seats)
                {
                    if (item.SeatId == seat.SeatId)
                    {
                        item.Booked = false;
                    }
                }
            }
            return salonSeats;
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .FirstOrDefaultAsync(m => m.SeatId == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatId,SalonId,SeatNumber")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seat);
        }

        // GET: Seats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeatId,SalonId,SeatNumber")] Seat seat)
        {
            if (id != seat.SeatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.SeatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seat);
        }

        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .FirstOrDefaultAsync(m => m.SeatId == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.SeatId == id);
        }
    }
}
