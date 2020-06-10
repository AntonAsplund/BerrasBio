﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using System.Security.Claims;

namespace BerrasBioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewingsApiController : ControllerBase
    {
        private readonly TeaterDbContext _context;

        public ViewingsApiController(TeaterDbContext context)
        {
            _context = context;
        }

        // GET: api/ViewingsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Viewing>>> GetViewings()
        {
            return await _context.Viewings.ToListAsync();
        }

        // GET: api/ViewingsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Viewing>> GetViewing(int id)
        {
            var viewing = await _context.Viewings.FindAsync(id);

            if (viewing == null)
            {
                return NotFound();
            }

            return viewing;
        }

        // PUT: api/ViewingsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutViewing(int id, Viewing viewing)
        {
            if (id != viewing.ViewingId)
            {
                return BadRequest();
            }

            _context.Entry(viewing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViewingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

      

        // POST: api/ViewingsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Viewing>> PostViewing(Viewing viewing)
        {
            _context.Viewings.Add(viewing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetViewing", new { id = viewing.ViewingId }, viewing);
        }

        // DELETE: api/ViewingsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Viewing>> DeleteViewing(int id)
        {
            var viewing = await _context.Viewings.FindAsync(id);
            if (viewing == null)
            {
                return NotFound();
            }

            _context.Viewings.Remove(viewing);
            await _context.SaveChangesAsync();

            return viewing;
        }

        private bool ViewingExists(int id)
        {
            return _context.Viewings.Any(e => e.ViewingId == id);
        }
    }
}