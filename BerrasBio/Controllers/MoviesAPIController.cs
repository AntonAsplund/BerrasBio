using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;

namespace BerrasBio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesAPIController : ControllerBase
    {
        private readonly TeaterDbContext _context;

        public MoviesAPIController(TeaterDbContext context)
        {
            _context = context;
        }
        // GET: api/MoviesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET: api/MoviesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }
    }
}
