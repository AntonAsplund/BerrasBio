using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.ViewModels;

namespace BerrasBio.Controllers
{
    public class ViewingsController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;

        public ViewingsController(ISqlTheaterData sqlTheaterData)
        {
            this.sqlTheaterData = sqlTheaterData;
        }

        public async Task<IActionResult> Index(int? id, string order)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Viewing> viewings = await sqlTheaterData.GetViewingsById((int)id, order);

            if(viewings.Count == 0)
            {
                return NotFound();
            }
            return base.View(viewings);
        }
        public IActionResult Book(int? id)
        {
            string url = String.Format($"../../Seats/index/{id}");
            return base.Redirect(url);
        }


        public IActionResult CheckView(int? id, string order)
        {
            return Redirect(String.Format($"../../Viewings/index?id={id}&order={order}"));
            //return Redirect(String.Format($"../../Viewings/index/{id}/{order}"));
        }

        public async Task<IActionResult> Create()
        {
            List<Movie> movies = await sqlTheaterData.OnGetMovies(includeOld: true);
            List<Salon> salons = sqlTheaterData.GetSalons();
            var model = new CreateViewingViewModel { Movies = movies, Salons = salons };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(int movieId, int salonId, DateTime startTime)
        {
            var viewing = new Viewing { MovieId = movieId, SalonId = salonId, StartTime = startTime };
            sqlTheaterData.CreateViewing(viewing);


            TempData["Message"] = "Viewing was successfully added";
            return RedirectToAction("Create");
        }
    }
}
