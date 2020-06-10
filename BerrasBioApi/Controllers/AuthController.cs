using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BerrasBioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public ActionResult GetToken()
        {


            // security key
            string securityKey = "strong_key";
            // symetrick security key
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            // creating token
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            // signing credentials
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "reader",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            // return token
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return Ok(handler.WriteToken(token));
        }


    }
}
