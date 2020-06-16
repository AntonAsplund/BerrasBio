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
using System.Net;

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
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
            if (AuthHandler.CheckIfAdmin(this))
            {
                List<User> users = await sqlTheaterData.OnGetUsers();
                return base.View(users);
            }
            else
            {
                return StatusCode(403);
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var context = this;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var thisUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == (claim[0].Value));

            if (AuthHandler.CheckIfAdmin(this))
            {
                TempData["IsAdmin"] = true;
            }
            else if (thisUser.UserId != id) 
            {
                return StatusCode(403);
            }

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

        //Method to acess your own profile page from the navbar button "my profile"
        public async Task<IActionResult> MyDetails()
        {
            if (AuthHandler.CheckIfAdmin(this))
            {
                TempData["IsAdmin"] = true;
            }

            var context = this;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == (claim[0].Value));

            return RedirectToAction("Details", new { id = user.UserId });
        }


        private async Task<User> AuthenticateUserAsync(User userLoging)
        {
            User credentials = GetUser(userLoging.UserName);

            User user = null;
            if (userLoging.UserName == credentials.UserName && Encryption.DecryptString("kljsdkkdlo4454GG00155sajuklmbkdl", credentials.Password) == userLoging.Password)
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

        private User GetUser(string username)
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
        public IActionResult Create()
        {
            if (AuthHandler.CheckIfAdmin(this))
            {
                return Redirect(String.Format($"../../Users/CreateAdmin"));
            }
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,FirstName,LastName,Password,PhoneNumber")] User user)
        {
            bool hasDublicates = _context.Users.Where(u => u.UserName == user.UserName).Any();
            if (TryValidateModel(user) && !hasDublicates)
            {
                user.IsAdmin = false;
                user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
                //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = sqlTheaterData.AddUser(user);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Details), new { id = user.UserId });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        // GET: Users/Create
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult CreateAdmin()
        {
            return AuthHandler.RedirectToView(this);
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateAdmin([Bind("UserId,UserName,FirstName,LastName,Password,IsAdmin,PhoneNumber")] User user)
        {
            bool hasDublicates = _context.Users.Where(u => u.UserName == user.UserName).Any();
            if (TryValidateModel(user) && !hasDublicates)
            {
                user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
                //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = sqlTheaterData.AddUser(user);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Details), new { id = user.UserId });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return Redirect(String.Format($"../../Home"));

        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserId,UserName,Password,IsAdmin,PhoneNumber")] User user)
        {
            User loggedInUser = _context.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (loggedInUser == null)
            {
                TempData["WrongInput"] = "Faulty towers input";
                return Redirect(String.Format($"../../Users/Login"));
            }

            User authenticatedUser = await AuthenticateUserAsync(user);
            if (authenticatedUser != null)
            {

                var claims = new[] { new Claim(ClaimTypes.Name, authenticatedUser.UserName),
                    new Claim(ClaimTypes.Role, authenticatedUser.IsAdmin ? "Admin" : "User")};

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return Redirect(String.Format($"../../Users/Accepted"));

            }
            TempData["WrongInput"] = "Faulty towers input";
            return Redirect(String.Format($"../../Users/Login"));
        }

        public IActionResult Accepted()
        {
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var context = this;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var thisUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == (claim[0].Value));

            if (AuthHandler.CheckIfAdmin(this)) //Adds option to the view for a admin to go back to the full list of users during an edit.
            {
                TempData["IsAdmin"] = true;
            }
            else if (thisUser.UserId != id) //Makes sure that a non admin doesn't get acess to someone elses account.
            {
                return StatusCode(403);
            }

            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

               
            user.Password = Encryption.DecryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
            return View(user);

        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,FirstName,LastName,Password,IsAdmin,PhoneNumber")] User user)
        {

            var context = this;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var thisUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == (claim[0].Value));

            var duplicateUserName = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            

            if (AuthHandler.CheckIfAdmin(this)) //Adds option to the view for a admin to go back to the full list of users during an edit.
            {
                TempData["IsAdmin"] = true;
            }
            else if (thisUser.UserId != id) //Makes sure that a non admin doesn't get acess to someone elses account.
            {
                return StatusCode(403);
            }

            if (duplicateUserName != null && duplicateUserName.UserId != user.UserId) //Checks to see if username already exsists in the database ans is not the users own. To avoid issues with multiple usernames.
            {
                TempData["UserNameTaken"] = "Username taken";
                return View(user);
            }

            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);

                    var oldUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);  //Updates a modified entity without tracking issues

                    oldUser.FirstName = user.FirstName;
                    oldUser.LastName = user.LastName;
                    oldUser.Password = user.Password;
                    oldUser.PhoneNumber = user.PhoneNumber;
                    oldUser.IsAdmin = user.IsAdmin;
                    oldUser.UserName = user.UserName;

                    await _context.SaveChangesAsync();


                    if (thisUser.UserId == user.UserId) 
                    { 
                        var claims = new[] { new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")};

                        var identityClaims = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identityClaims));
                    }
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
                if (AuthHandler.CheckIfAdmin(this)) //If admin redirects to index list
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", new { id = user.UserId }); //If user redirects to details view och that users profile. So a regular user won't be able to see everone elses accounts.
                }
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
