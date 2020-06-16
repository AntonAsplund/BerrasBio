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
using BerrasBio.ViewModels;

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
                List<Movie> movies = await sqlTheaterData.OnGetMovies(includeOld: false);
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
            var viewings = await sqlTheaterData.GetViewingsById((int)id, "Num");
            if (movie == null)
            {
                return NotFound();
            }
            var model = new DetailsViewModel { Movie = movie, Viewings = viewings };
            return View(model);
        }

        public IActionResult CheckView(int? id, string order)
        {
            return Redirect(String.Format($"../../Viewings/index?id={id}&order={order}"));
        }
    }
}
