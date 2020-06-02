
using BerrasBio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BerrasBio.Security
{
    public class AutorizationHandler
    {

        //private bool IsCorrectUser(int customerId)
        //{
            
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
            
        //    IList<Claim> claim = identity.Claims.ToList();
        //    int userId = Convert.ToInt32(claim[2].Value);
        //    bool isAdmin = claim[3].Value == "1" ? true : false;
        //    if (!isAdmin | userId != customerId)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
