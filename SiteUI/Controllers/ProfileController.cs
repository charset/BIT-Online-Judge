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

        // GET: Profile/ShowUser
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

        // POST: Profile/ShowUser
        [HttpPost]
        public ActionResult ShowUser(FormCollection form, UserProfileModel model)
        {
            // 更新用户密码。
            // 验证操作权限。
            if (!UserSession.IsAuthorized(Session) ||
                string.Compare(UserSession.GetUsername(Session), Request.QueryString["username"], false) != 0)
            {
                return Redirect("~/Error/AccessDenied");
            }

            // 验证用户输入。
            if (string.IsNullOrEmpty(form["old"]))
            {
                ViewBag.PasswordErrorMessage = "Old password is required.";
                return View(model);
            }
            if (string.IsNullOrEmpty(form["new"]))
            {
                ViewBag.PasswordErrorMessage = "New password is required.";
                return View(model);
            }
            if (form["new"].Length < 6)
            {
                ViewBag.PasswordErrorMessage = "New password is too short.";
                return View(model);
            }
            if (string.Compare(form["new"], form["confirm"], false) != 0)
            {
                ViewBag.PasswordErrorMessage = "Confirmed password is not the same as the new password.";
                return View(model);
            }

            // 验证旧密码。
            if (!UserAuthorization.CheckAuthorization(Request.QueryString["username"], form["old"]))
            {
                ViewBag.PasswordErrorMessage = "Old password is incorrect.";
                return View(model);
            }

            // 更新用户密码。
            UserAuthorization.UpdatePassword(Request.QueryString["username"], form["new"]);
            return View(model);
        }
    }
}