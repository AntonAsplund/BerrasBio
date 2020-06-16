using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Security;
using BerrasBio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BerrasBio.Controllers
{
    public class ReserveController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;
        
        public ReserveController(ISqlTheaterData sqlTheaterData)
        {
            this.sqlTheaterData = sqlTheaterData;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["MovieId"] = sqlTheaterData.GetViewingById((int)id).MovieId;
                return View(await sqlTheaterData.FindSeats((int)id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection data)
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

            if(seatIds.Count == 0)
            {
                TempData["Message"] = "You need to choose seats!";
                return RedirectToAction("Index", new { id = viewingId });
            }

            TempData["SeatIds"] = seatIds;
            return RedirectToAction("Confirm", new { id = viewingId});
        }

        public IActionResult Confirm(int id)
        {
            int[] seatIds = (int[])TempData["SeatIds"];
            TempData.Keep("SeatIds");
            List<int> seatNumbers = sqlTheaterData.GetSeatNumbers(seatIds);
            var model = new ConfirmViewModel { Viewing = sqlTheaterData.GetViewingById(id), SeatNumbers = seatNumbers };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int viewingId, IFormCollection form)
        {
            int[] seatIds = (int[])TempData["SeatIds"];

            Order order = sqlTheaterData.CreateOrder(seatIds.ToList(), viewingId, User.Identity.Name);
            sqlTheaterData.Update();
            await sqlTheaterData.Update(order);
            return RedirectToAction("Receipt", new { id = order.OrderId });
        }
        
        [Authorize]
        public async Task<IActionResult> Receipt(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var order = await sqlTheaterData.GetOrder((int)Id);
            if (order == null)
            {
                return NotFound();
            }

            return base.View(order);
        }
    }
}