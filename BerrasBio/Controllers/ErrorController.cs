using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerrasBio.Data;
using BerrasBio.Models;
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
            return View(await GetRandomMovie());
        }


        [Route("403")]
        public async Task<IActionResult> Forbidden(string path)
        {
            return View(await GetRandomMovie());
        }

        [Route("401")]
        public async Task<IActionResult> NotAuthorized(string path)
        {
            return View(await GetRandomMovie());
        }

        private async Task<Movie> GetRandomMovie()
        {
            var movies = await sqlTheaterData.OnGetMovies(false);

            Random rnd = new Random();
            var movieToDisplay = movies[rnd.Next(1, movies.Count)];

            var viewing = await sqlTheaterData.GetViewingsById(movieToDisplay.MovieId, "");

            TempData["viewing"] = viewing.FirstOrDefault().StartTime.ToString();

            return movieToDisplay;
        }
    }
}