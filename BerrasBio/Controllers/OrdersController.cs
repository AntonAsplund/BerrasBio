﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using Microsoft.AspNetCore.Authorization;
using BerrasBio.Security;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BerrasBio.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ISqlTheaterData sqlTheaterData;

        public OrdersController(ISqlTheaterData sqlTheaterData)
        {
            this.sqlTheaterData = sqlTheaterData;
        }

        // GET: Orders
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index(int? Id)
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
            if (!AuthHandler.CheckIfCorrectUser(order.User.UserName, this))
            {
                return StatusCode(403);
            }
            return base.View(order);
        }
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? Id)
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
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerName,Tickets,UserId,User")] Order order)
        {
            order = await sqlTheaterData.GetOrder(order.OrderId);
            //sqlTheaterData.LoadOrder(order);
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await sqlTheaterData.Update(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!sqlTheaterData.OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string url = String.Format($"../../Orders/index/{id}");

                return Redirect(url);
            }
            return View(order);
        }
    }
}
