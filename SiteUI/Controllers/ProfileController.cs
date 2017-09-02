namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
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

        // POST: Profile/CreateTeam
        [HttpPost]
        public ActionResult CreateTeam(FormCollection form)
        {
            // AJAX 查询，以 JSON 返回结果。
            // 验证表单参数。
            if (string.IsNullOrEmpty(form["teamName"]) || string.IsNullOrEmpty(form["leader"]))
            {
                return new ContentResult();
            }

            // 创建新的队伍。
            TeamHandle handle = UserManager.Default.CreateTeam();
            using (TeamDataProvider data = TeamDataProvider.Create(handle, false))
            {
                data.Name = form["teamName"];
                data.Leader = form["leader"];

                UserManager.Default.AddUserToTeam(handle, new UserHandle(form["leader"]));
            }

            return new ContentResult();
        }

        // GET: Profile/ShowTeam
        public ActionResult ShowTeam()
        {
            if (string.IsNullOrEmpty(Request.QueryString["teamId"]))
            {
                return Redirect("~/Profile/Index");
            }

            int teamId;
            if (!int.TryParse(Request.QueryString["teamId"], out teamId))
            {
                return Redirect("~/Profile/Index");
            }

            TeamHandle handle = UserManager.Default.QueryTeamById(teamId);
            if (handle == null)
            {
                return Redirect("~/Error/TeamNotFound");
            }

            TeamDetailModel model = TeamDetailModel.FromTeamHandle(handle);
            return View(model);
        }

        // POST: Profile/AddTeamUser
        [HttpPost]
        public ActionResult AddTeamUser()
        {
            // AJAX 查询响应
            if (!UserSession.IsAuthorized(Session))
            {
                return new ContentResult();
            }

            if (string.IsNullOrEmpty(Request.QueryString["teamId"]) || 
                string.IsNullOrEmpty(Request.QueryString["username"]))
            {
                return new ContentResult();
            }

            int teamId;
            if (!int.TryParse(Request.QueryString["teamId"], out teamId))
            {
                return new ContentResult();
            }

            TeamHandle teamHandle = UserManager.Default.QueryTeamById(teamId);
            if (teamHandle == null)
            {
                // 给定的队伍不存在于数据库中。
                return new ContentResult();
            }

            UserHandle userHandle = UserManager.Default.QueryUserByName(Request.QueryString["username"]);
            if (userHandle == null)
            {
                // 给定的用户不存在于数据库中。
                return new ContentResult();
            }

            // 查询队伍信息。
            using (TeamDataProvider data = TeamDataProvider.Create(teamHandle, true))
            {
                // 检查操作权限。
                if (string.Compare(data.Leader, UserSession.GetUsername(Session), false) != 0)
                {
                    return new ContentResult();
                }
            }

            UserManager.Default.AddUserToTeam(teamHandle, userHandle);
            return new ContentResult();
        }

        // POST: Profile/RemoveTeamUser
        [HttpPost]
        public ActionResult RemoveTeamUser()
        {
            // AJAX 查询响应
            if (!UserSession.IsAuthorized(Session))
            {
                return new ContentResult();
            }

            if (string.IsNullOrEmpty(Request.QueryString["teamId"]) ||
                string.IsNullOrEmpty(Request.QueryString["username"]))
            {
                return new ContentResult();
            }

            int teamId;
            if (!int.TryParse(Request.QueryString["teamId"], out teamId))
            {
                return new ContentResult();
            }

            TeamHandle teamHandle = UserManager.Default.QueryTeamById(teamId);
            UserHandle userHandle = UserManager.Default.QueryUserByName(Request.QueryString["username"]);
            if (teamHandle == null || userHandle == null)
            {
                return new ContentResult();
            }

            using (TeamDataProvider data = TeamDataProvider.Create(teamHandle, true))
            {
                // 检查操作权限。
                if (string.Compare(data.Leader, UserSession.GetUsername(Session), false) != 0)
                {
                    return new ContentResult();
                }
            }

            UserManager.Default.RemoveUserFromTeam(teamHandle, userHandle);
            return new ContentResult();
        }

        // POST: Profile/DeleteTeam
        [HttpPost]
        public ActionResult DeleteTeam()
        {
            if (!UserSession.IsAuthorized(Session))
            {
                return new ContentResult();
            }

            if (string.IsNullOrEmpty(Request.QueryString["teamId"]))
            {
                return new ContentResult();
            }

            int teamId;
            if (!int.TryParse(Request.QueryString["teamId"], out teamId))
            {
                return new ContentResult();
            }

            TeamHandle teamHandle = UserManager.Default.QueryTeamById(teamId);
            if (teamHandle == null)
            {
                return new ContentResult();
            }

            using (TeamDataProvider data = TeamDataProvider.Create(teamHandle, true))
            {
                // 检查操作权限。
                if (string.Compare(data.Leader, UserSession.GetUsername(Session), false) != 0)
                {
                    return new ContentResult();
                }
            }

            UserManager.Default.RemoveTeam(teamHandle);
            return new ContentResult();
        }
    }
}