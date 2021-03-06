﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using BerrasBio.Data;
using BerrasBio.Models;
using BerrasBio.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BerrasBio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private IConfiguration configuration;
        public TeaterDbContext context { get; set; }
        public LoginApiController(IConfiguration configuration, TeaterDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            User userLoging = new User();
            userLoging.UserName = username;
            userLoging.Password = password;
            IActionResult response = Unauthorized();
            User user = await AuthenticateUserAsync(userLoging);
            if (user != null)
            {
                string tokenStr = GenerateWebToken(user);
                return Ok(new { token = tokenStr });
            }
            return response;
        }
        private User GetUser(string username)
        {
            IQueryable<User> queryForUsername = context.Users
                .Where(credential => credential.UserName == username);
            User credentials = null;
            foreach (User item in queryForUsername)
            {
                credentials = item;
            }
            return credentials;
        }
        private string GenerateWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            int isAdmin = 0;
            if (user.IsAdmin)
            {
                isAdmin = 1;
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Acr, isAdmin.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            string encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
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
        [HttpPost("CreateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            IActionResult response = Unauthorized();
            if (user != null)
            {
                if (!IsAdmin())
                {
                    user.IsAdmin = false;
                }
                user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entityEntry = context.Users.Add(user);
                await context.SaveChangesAsync();
                return Ok("Success!");
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
