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
        public IActionResult CheckView(int? id, string order)
        {
            return Redirect(String.Format($"../../Viewings/index?id={id}&order={order}"));
            //return Redirect(String.Format($"../../Viewings/index/{id}/{order}"));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            sqlTheaterData.AddMovie(movie);
            TempData["Message"] = "Movie was added successfully to the database!";
            return RedirectToAction("Create");
        }
    }
}
