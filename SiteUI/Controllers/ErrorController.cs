using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BITOJ.SiteUI.Controllers
{
    public class ErrorController : Controller
    {
        // Get: Error/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        // GET: Error/ProblemNotExist
        public ActionResult ProblemNotExist()
        {
            return View();
        }
    }
}