using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BITOJ.SiteUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RedirectById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("Index");
            }
            else
            {
                // Test code below.
                return new RedirectResult("http://acm.hdu.edu.cn/");
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Instruction()
        {
            return View();
        }
    }
}