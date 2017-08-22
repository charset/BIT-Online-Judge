
namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core.Authorization;
    using System.Web.Mvc;

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            // 尝试进行登录验证。
            if (UserSession.Authorize(Session, username, password))
            {
                return new RedirectResult("~/Home/Index");
            }
            else
            {
                ViewData.Add("HasError", true);
                return View("Index");
            }
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}