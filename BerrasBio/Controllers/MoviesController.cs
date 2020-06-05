using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BerrasBio.Security;

namespace BerrasBio.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;
        public MoviesController(ISqlTheaterData sqlTeaterData)
        {
            this.sqlTheaterData = sqlTeaterData;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            AuthHandler.CheckIfAdmin(this);
            bool isAdmin = AuthHandler.CheckIfAdmin(this);
            if (isAdmin)
            {
                return Redirect(String.Format($"../../Movies/indexAdmin"));

            }
            else
            {
                List<Movie> movies = await sqlTheaterData.OnGetMovies(includeOld: false);
                return base.View(movies);
            }
        }


        // GET: Movies
        [Authorize]
        public async Task<IActionResult> IndexAdmin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            //int userId = Convert.ToInt32(claim[0].Value);
            bool isAdmin = claim[1].Value == "Admin";
            if (isAdmin)
            {
                List<Movie> movies = await sqlTheaterData.OnGetMovies(includeOld: true);
                return base.View(movies);
            }
            else
            {
                return StatusCode(403);
            }
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
        public IActionResult CheckView(int? id, string order)
        {
            return Redirect(String.Format($"../../Viewings/index?id={id}&order={order}"));
            //return Redirect(String.Format($"../../Viewings/index/{id}/{order}"));
        }
        [Authorize]
        public IActionResult Create()
        {
            bool isAdmin = AuthHandler.CheckIfAdmin(this);
            if (isAdmin)
            {
                return View();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            sqlTheaterData.AddMovie(movie);
            TempData["Message"] = "Movie was added successfully to the database!";
            return RedirectToAction("Create");
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await sqlTheaterData.FindMovie(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,PosterURL,Title,LengthMinute,IsPlaying,Cathegory,Director,Actors,ReleaseYear")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await sqlTheaterData.UpdateMovie(movie);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await sqlTheaterData.FindMovie(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await sqlTheaterData.DeleteMovieAt(id);
            return RedirectToAction(nameof(Index));
        }

       

        private bool MovieExists(int id)
        {
            return sqlTheaterData.DoesMovieExist(id);
        }

        
    }
}
