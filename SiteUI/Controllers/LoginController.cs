﻿namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Models;
    using System;
    using System.Web.Mvc;

    public class LoginController : Controller
    {
        /// <summary>
        /// 当控制器请求用户登录验证时，调用此方法向用户返回结果。
        /// </summary>
        /// <param name="invoker">调用视图。</param>
        /// <returns>向用户返回的操作结果。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ActionResult RequestForLogin(Controller invoker)
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            return new RedirectResult($"~/Login/Login?request={invoker.Request.Url}");
        }

        // GET: Login
        public ActionResult Index()
        {
            string requestUrl = Request.QueryString["request"] ?? "/Home";
            return Redirect($"~/Login/Login?request={requestUrl}");
        }

        // GET: Login/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Login/Login
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // 尝试进行登录验证。
            if (UserSession.Authorize(Session, form["username"], form["password"]))
            {
                string requestUrl = Request.QueryString["request"] ?? "~/Home";
                return Redirect(requestUrl);
            }
            else
            {
                ViewData.Add("HasError", true);
                return View();
            }
        }

        // GET: Login/Logout
        public ActionResult Logout()
        {
            UserSession.Deauthorize(Session);
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Login/Register
        public ActionResult Register()
        {
            return View(new UserRegisterModel());
        }

        // POST: Login/Register
        [HttpPost]
        public ActionResult Register(UserRegisterModel model)
        {
            // Validate user input.
            model.ResetErrorMessages();

            bool hasError = false;
            if (ModelState.ContainsKey("Username") && ModelState["Username"].Errors.Count > 0)
            {
                hasError = true;
                model.UsernameErrorMessage = ModelState["Username"].Errors[0].ErrorMessage;
            }
            if (ModelState.ContainsKey("Password") && ModelState["Password"].Errors.Count > 0)
            {
                hasError = true;
                model.PasswordErrorMessage = ModelState["Password"].Errors[0].ErrorMessage;
            }
            if (ModelState.ContainsKey("PasswordConfirmation") && ModelState["PasswordConfirmation"].Errors.Count > 0)
            {
                hasError = true;
                model.PasswordConfirmationErrorMessage = ModelState["PasswordConfirmation"].Errors[0].ErrorMessage;
            }

            if (hasError)
            {
                return View(model);
            }

            if (string.Compare(model.Password, model.PasswordConfirmation, false) != 0)
            {
                model.PasswordConfirmationErrorMessage = "Password and its confirmation is not the same.";
                return View(model);
            }

            if (DoRegister(model))
            {
                return View("Succeed", model);
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 执行注册操作，并更新数据模型中的错误消息。
        /// </summary>
        /// <param name="model">用户注册数据模型。</param>
        [NonAction]
        private bool DoRegister(UserRegisterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (UserManager.Default.IsUserExist(model.Username))
            {
                model.UsernameErrorMessage = "Username already exist.";
                return false;
            }

            UserHandle handle = UserManager.Default.CreateUser(model.Username);
            UserAuthorization.UpdatePassword(model.Username, model.Password);

            using (UserDataProvider data = UserDataProvider.Create(handle, false))
            {
                data.Sex = SexConvert.ConvertFromString(model.Sex);
                data.UserGroup = UsergroupConvert.ConvertFromString(model.UserGroupName);
            }

            return true;
        }
    }
}