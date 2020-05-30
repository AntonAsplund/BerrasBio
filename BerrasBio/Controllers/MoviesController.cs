using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;

namespace BerrasBio.Controllers
{
    public class MoviesController : Controller
    {
        /*

        private readonly TeaterDbContext _context;

        public MoviesController(TeaterDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }


        public IActionResult CheckView(int? id)
        {
            return Redirect(String.Format($"../../Viewings/index/{id}"));
        }



        */
        private readonly ISqlTheaterData sqlTheaterData;
        public MoviesController(ISqlTheaterData sqlTeaterData)
        {
            this.sqlTheaterData = sqlTeaterData;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await sqlTheaterData.OnGetMovies();
            return base.View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var movie = await sqlTheaterData.OnGetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }


        public IActionResult CheckView(int? id)
        {
            return Redirect(String.Format($"../../Viewings/index/{id}"));
        }




















        /*

                // GET: Movies/Create
                public IActionResult Create()
                {
                    return View();
                }

                // POST: Movies/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to, for 
                // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("MovieId,Title,LengthMinute,IsPlaying,Cathegory")] Movie movie)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(movie);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(movie);
                }

                // GET: Movies/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var movie = await _context.Movies.FindAsync(id);
                    if (movie == null)
                    {
                        return NotFound();
                    }
                    return View(movie);
                }

                // POST: Movies/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to, for 
                // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,LengthMinute,IsPlaying,Cathegory")] Movie movie)
                {
                    if (id != movie.MovieId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(movie);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!MovieExists(movie.MovieId))
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
                    return View(movie);
                }



                // POST: Movies/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    var movie = await _context.Movies.FindAsync(id);
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                private bool MovieExists(int id)
                {
                    return _context.Movies.Any(e => e.MovieId == id);
                }
        */
    }
}
