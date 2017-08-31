namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.SiteUI.Models;
    using System.Web.Mvc;

    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        
        // POST: Profile
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["username"]))
            {
                ViewBag.Error = true;
                return View();
            }

            // 检查输入的用户名是否存在。
            if (!UserManager.Default.IsUserExist(form["username"]))
            {
                ViewBag.Error = true;
                return View();
            }

            string url = string.Format("/Profile/ShowUser?username={0}", form["username"]);
            return Redirect(url);
        }

        // GET: Profile/ShowProfile
        public ActionResult ShowUser()
        {
            if (string.IsNullOrEmpty(Request.QueryString["username"]))
            {
                return Redirect("/Profile");
            }

            UserHandle handle = UserManager.Default.QueryUserByName(Request.QueryString["username"]);
            if (handle == null)
            {
                // 用户不存在。
                return Redirect("/Profile");
            }

            UserProfileModel model = UserProfileModel.FromUserHandle(handle);
            return View(model);
        }
    }
}