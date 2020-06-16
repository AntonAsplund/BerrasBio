using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace BerrasBio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesAPIController : ControllerBase
    {
        private readonly ISqlTheaterData db;

        public MoviesAPIController(ISqlTheaterData sqlTheaterData)
        {
            db = sqlTheaterData;
        }
        // GET: api/MoviesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await db.OnGetMovies(true);
        }

        // GET: api/MoviesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await db.OnGetMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            IActionResult response = Unauthorized();

            if (IsAdmin())
            {
                bool deleted = await db.OnDeleteMovie(id);

                if (deleted == false)
                    return NotFound();

                else
                    return Ok();
            }

            return response;
        }

        private bool IsAdmin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            bool isAdmin = claim[2].Value == "1" ? true : false;
            return isAdmin;
        }
    }
}
