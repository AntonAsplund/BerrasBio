using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BerrasBio.Controllers
{
    public class SignUpController : Controller
    {
        private readonly TeaterDbContext _context;

        public SignUpController(TeaterDbContext context)
        {
            _context = context;
        }
        // GET: SignUpController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SignUpController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SignUpController/Create
        public ActionResult Create()
        {
            return View();
        }


        //[HttpPost("CreateUser")]
        //[Authorize]
        //public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        //{
        //    if (!IsAdmin())
        //    {
        //        return Unauthorized("Only administrators is allowed to create users.");
        //    }
        //    IActionResult response = Unauthorized();
        //    if (user != null)
        //    {
        //        user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
        //        //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
        //        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = _context.Users.Add(user);
        //        await _context.SaveChangesAsync();
        //        return Ok("Success!");
        //    }
        //    return response;
        //}


        // POST: SignUpController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignUpController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SignUpController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignUpController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SignUpController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
