using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Security;
using BerrasBio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerrasBio.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;
        public AdminController(ISqlTheaterData sqlTeaterData)
        {
            this.sqlTheaterData = sqlTeaterData;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
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

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            bool isAdmin = AuthHandler.CheckIfAdmin(this);
            if (isAdmin)
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
            else
            {
                return StatusCode(403);
            }
            
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

        [Authorize]
        public async Task<IActionResult> CreateViewing(int id)
        {
            bool isAdmin = AuthHandler.CheckIfAdmin(this);
            if (isAdmin)
            {
                Movie movie = await sqlTheaterData.OnGetMovie(id);
                List<Salon> salons = sqlTheaterData.GetSalons();
                var model = new CreateViewingViewModel { MovieId = id, Movie = movie, Salons = salons };
                return View(model);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        public IActionResult CreateViewing(int movieId, int salonId, DateTime startTime)
        {
            var viewing = new Viewing { MovieId = movieId, SalonId = salonId, StartTime = startTime };
            sqlTheaterData.CreateViewing(viewing);


            TempData["Message"] = "Viewing was successfully added";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            bool isAdmin = AuthHandler.CheckIfAdmin(this);
            if (isAdmin)
            {
                Movie movie = await sqlTheaterData.OnGetMovie(id);
                return View(movie);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    await sqlTheaterData.UpdateMovie(movie);
                    TempData["Message"] = "Movie was successfully updated";
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", new { movie.MovieId});
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            bool isAdmin = claim[1].Value == "Admin";

            if (isAdmin)
            {
                var model = await sqlTheaterData.OnGetMovie(id);
                return View(model);
            }

            else
            {
                return StatusCode(403);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            bool isAdmin = claim[1].Value == "Admin";

            if (isAdmin)
            {
                await sqlTheaterData.OnDeleteMovie(id);
                TempData["Message"] = "Movie was successfully deleted";
                return RedirectToAction("Index");
            }

            else
            {
                return StatusCode(403);
            }
        }

        private bool MovieExists(int id)
        {
            return sqlTheaterData.DoesMovieExist(id);
        }
    }
}