
using BerrasBio.Controllers;
using BerrasBio.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BerrasBio.Security
{
    public static class AuthHandler
    {
        internal static bool CheckIfAdmin(Controller moviesController)
        {
            var identity = moviesController.HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            bool isAdmin = false;
            if (claim != null && claim.Count > 0)
            {
                isAdmin = claim[1].Value == "Admin";
            }
            return isAdmin;
        }

        internal static IActionResult RedirectToPage(string page, Controller context)
        {
            if (CheckIfAdmin(context))
            {
                return context.Redirect($"../{page}");

            }
            else
            {
                return context.StatusCode(403);
            }

        }

        internal static IActionResult RedirectToView(Controller context)
        {
            if (CheckIfAdmin(context))
            {
                return context.View();

            }
            else
            {
                return context.StatusCode(403);
            }

        }
    }
}