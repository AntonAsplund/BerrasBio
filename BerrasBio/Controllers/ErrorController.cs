using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BerrasBio.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {

        [Route("404")]
        public IActionResult PageNotFound(string path)
        {
            TempData["path"] = path;

            return View();
        }

        [Route("403")]
        public IActionResult NotAuthorized(string path)
        {
            TempData["path"] = path;

            return View();
        }

        [Route("401")]
        public IActionResult Forbidden(string path)
        {
            TempData["path"] = path;

            return View();
        }
    }
}