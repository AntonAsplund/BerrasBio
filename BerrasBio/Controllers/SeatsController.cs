using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BerrasBio.Controllers
{
    public class SeatsController : Controller
    {



        private readonly ISqlTheaterData sqlTheaterData;
        /// <summary>
        /// Will add everytime user click a seat.
        /// </summary>
        public int NumBookedSeats { get; set; }
        // todo: Check how to add a button and method to controll it.
        public SeatsController(ISqlTheaterData sqlTheaterData)
        {
            this.sqlTheaterData = sqlTheaterData;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                return View(await sqlTheaterData.FindSeats((int) id));
            }
        }
        
        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Choose(IFormCollection data)
        {
            List<int> seatIds = new List<int>();
            StringValues viewingIdString;
            data.TryGetValue("viewingId", out viewingIdString);
            int viewingId = 0;
            Int32.TryParse(viewingIdString, out viewingId);
            foreach (var key in data.Keys)
            {
                int seatId = 0;
                if (Int32.TryParse(key, out seatId))
                {
                    seatIds.Add(seatId);
                }
            }
            Order order = sqlTheaterData.CreateOrder(seatIds, viewingId);
            if (!TryValidateModel(order))
            {
                return BadRequest(ModelState);
            }
            sqlTheaterData.Update();

            string url = String.Format($"../../Orders/Edit/{order.OrderId}");
            return Redirect(url);
        }
    }
}
