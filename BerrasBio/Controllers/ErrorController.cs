using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerrasBio.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BerrasBio.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;
        public ErrorController(ISqlTheaterData sqlTeaterData)
        {
            this.sqlTheaterData = sqlTeaterData;
        }

        [Route("404")]
        public async Task<IActionResult> PageNotFound(string path)
        {
            TempData["path"] = path;

            var movies = await sqlTheaterData.OnGetMovies(false);

            Random rnd = new Random();
            int randomNumber = rnd.Next(1, movies.Count);

            var movieToDisplay = movies[randomNumber];

            var viewing = await sqlTheaterData.GetViewingsById(movieToDisplay.MovieId, "");

            TempData["viewing"] = viewing.FirstOrDefault().StartTime.ToString();

            return View(movieToDisplay);
        }

        [Route("403")]
        public IActionResult Forbidden(string path)
        {
            TempData["path"] = path;

            return View();
        }

        [Route("401")]
        public IActionResult NotAuthorized(string path)
        {
            TempData["path"] = path;

            return View();
        }
    }
}