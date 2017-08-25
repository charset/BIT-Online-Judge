using System.Web.Mvc;

namespace BITOJ.SiteUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/About
        public ActionResult About()
        {
            return View();
        }

        // GET: /Home/Instruction
        public ActionResult Instruction()
        {
            return View();
        }
    }
}