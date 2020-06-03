using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestSharp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BerrasBio.Controllers
{
    public class UsersController : Controller
    {
        private readonly TeaterDbContext _context;
        private readonly ISqlTheaterData sqlTheaterData;
        public IConfiguration configuration{ get; set; }

        public UsersController(IConfiguration configuration,TeaterDbContext context, ISqlTheaterData sqlTheaterData)
        {
            this.sqlTheaterData = sqlTheaterData;
            _context = context;
            this.configuration = configuration;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<User> users = await sqlTheaterData.OnGetUsers();
            return base.View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await sqlTheaterData.OnGetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //[HttpGet]
        //public async Task<IActionResult> LoginAsync(string username, string password)
        //{
        //    User userLoging = new User();
        //    userLoging.UserName = username;
        //    userLoging.Password = password;
        //    IActionResult response = Unauthorized();
        //    User user = await AuthenticateUserAsync(userLoging);
        //    if (user != null)
        //    {
        //        string tokenStr = GenerateWebToken(user);
        //        return Ok(new { token = tokenStr });
        //    }
        //    return response;
        //}



        private async Task<User> AuthenticateUserAsync(User userLoging)
        {
            User credentials = GetUser(userLoging.UserName);

            User user = null;
            if (userLoging.UserName == credentials.UserName || Encryption.DecryptString("kljsdkkdlo4454GG00155sajuklmbkdl", userLoging.Password) == credentials.Password)
            {
                user = new User()
                {
                    UserId = credentials.UserId,
                    Password = credentials.Password,
                    UserName = credentials.UserName,
                    IsAdmin = credentials.IsAdmin
                };
            }
            return user;
        }

        private bool IsCorrectUser(int customerId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId = Convert.ToInt32(claim[2].Value);
            bool isAdmin = claim[3].Value == "1" ? true : false;
            if (!isAdmin | userId != customerId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public User GetUser(string username)
        {
            IQueryable<User> queryForUsername = _context.Users
                .Where(credential => credential.UserName == username);
            User credentials = null;
            foreach (User item in queryForUsername)
            {
                credentials = item;
            }

            return credentials;

            //return context.UserCredential
            //    .Where(credential => credential.UserName == "Daniel");

        }

        // GET: Users/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,IsAdmin,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
                //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = sqlTheaterData.AddUser(user);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public IActionResult Login()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserId,UserName,Password,IsAdmin,PhoneNumber")] User user)
        {
        //    if (ModelState.IsValid)
        //    {
        //        user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
        //        //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
        //        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = sqlTheaterData.AddUser(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);



            //User userLoging = new User();
            //userLoging.UserName = username;
            //userLoging.Password = password;
            IActionResult response = Unauthorized();
            User atuenticatedUser = await AuthenticateUserAsync(user);
            if (user != null)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "MyUserNameOrID"),
                    new Claim(ClaimTypes.Role, "SomeRoleName") };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return Redirect(String.Format($"../../Users/Accepted"));

            }
            return response;
        }

        public IActionResult Accepted()
        {
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,IsAdmin,PhoneNumber")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
