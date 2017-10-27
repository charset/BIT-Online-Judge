namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Models;
    using BITOJ.SiteUI.Models.Validation;
    using System;
    using System.Web.Mvc;

    public class ContestController : Controller
    {
        private const int ItemsPerPage = 50;

        private ContestStatus[] CatalogStatus = new ContestStatus[]
        {
            ContestStatus.Pending,
            ContestStatus.Running,
            ContestStatus.Ended
        };

        /// <summary>
        /// 验证 ContestDetailModel 数据模型。
        /// </summary>
        /// <param name="model">要验证的数据模型。</param>
        /// <returns>一个指示验证是否通过的值。</returns>
        /// <exception cref="ArgumentNullException"/>
        [NonAction]
        private bool ValidateContestDetailModel(ContestDetailModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            
            bool validationPassed = TryValidateModel(model);
            if (!validationPassed)
            {
                ViewBag.TitleErrorMessage = ModelStateHelper.GetFirstError(ModelState, "Title");
                ViewBag.CreatorErrorMessage = ModelStateHelper.GetFirstError(ModelState, "Creator");
                ViewBag.UsergroupNameErrorMessage = ModelStateHelper.GetFirstError(ModelState, "UsergroupName");
                ViewBag.StartTimeStringErrorMessage = ModelStateHelper.GetFirstError(ModelState, "StartTimeString");
                ViewBag.EndTimeStringErrorMessage = ModelStateHelper.GetFirstError(ModelState, "EndTimeString");
                ViewBag.ParticipationModeNameErrorMessage = ModelStateHelper.GetFirstError(ModelState, "ParticipationModeName");
                ViewBag.AuthorizationModeNameErrorMessage = ModelStateHelper.GetFirstError(ModelState, "AuthorizationModeName");
            }

            if (string.Compare(model.AuthorizationModeName, "Protected", true) == 0 &&
                (string.IsNullOrEmpty(model.Password) || model.Password.Length < 6))
            {
                validationPassed = false;
                ViewBag.PasswordErrorMessage = "Password should be at least 6 characters long.";
            }

            DateTime startTime = DateTime.Parse(model.StartTimeString);
            DateTime endTime = DateTime.Parse(model.EndTimeString);
            if (endTime <= startTime)
            {
                validationPassed = false;
                ViewBag.EndTimeStringErrorMessage = "End time must be later than start time.";
            }

            return validationPassed;
        }

        // GET: Contest
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Contest/Add
        [HttpGet]
        public ActionResult Add()
        {
            // 执行用户身份验证。
            if (!UserSession.IsAuthorized(Session) || 
                !UserAuthorization.CheckAccessRights(UserGroup.Insiders, UserSession.GetUserGroup(Session)))
            {
                return Redirect("~/Error/AccessDenied");
            }

            ContestDetailModel model = new ContestDetailModel()
            {
                Creator = UserSession.GetUsername(Session)
            };

            DateTime defaultStartTime = DateTime.Now.AddMinutes(15D);
            model.StartTimeString = defaultStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            model.EndTimeString = defaultStartTime.AddHours(2D).ToString("yyyy-MM-dd HH:mm:ss");

            return View(model);
        }

        // POST: Contest/Add
        [HttpPost]
        public ActionResult Add(ContestDetailModel model)
        {
            // 执行用户身份验证。
            if (!UserSession.IsAuthorized(Session) || !UserAuthorization.CheckAccessRights(UserGroup.Insiders,
                UserSession.GetUserGroup(Session)))
            {
                return Redirect("~/Error/AccessDenied");
            }

            // 验证数据模型。
            if (!ValidateContestDetailModel(model))
            {
                return View(model);
            }

            // 创建比赛。
            ContestHandle handle = ContestManager.Default.CreateContest();
            model.SaveTo(handle);

            return Redirect("~/Contest");
        }

        // GET: Contest/AddProblem?id={ContestID}
        [HttpGet]
        public ActionResult AddProblem()
        {
            // 检查查询参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Contest");
            }

            int contestId;
            if (!int.TryParse(Request.QueryString["id"], out contestId))
            {
                return Redirect("~/Contest");
            }

            if (!ContestManager.Default.IsContestExist(contestId))
            {
                return Redirect("~/Contest");
            }

            ContestHandle handle = new ContestHandle(contestId);
            using (ContestDataProvider data = ContestDataProvider.Create(handle, true))
            {
                // 检查用户操作权限。
                if (!UserSession.IsAuthorized(Session) ||
                    string.Compare(data.Creator, UserSession.GetUsername(Session), false) != 0)
                {
                    return Redirect("~/Error/AccessDenied");
                }
            }

            ProblemDetailModel model = new ProblemDetailModel();
            return View(model);
        }

        // POST: Contest/AddProblem?id={ContestID}
        [HttpPost]
        public ActionResult AddProblem(ProblemDetailModel model)
        {
            // 检查参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Contest");
            }

            int contestId;
            if (!int.TryParse(Request.QueryString["id"], out contestId))
            {
                return Redirect("~/Contest");
            }

            if (!ContestManager.Default.IsContestExist(contestId))
            {
                return Redirect("~/Contest");
            }

            // 验证数据模型。
            if (!TryValidateModel(model))
            {
                ViewBag.TitleErrorMessage = ModelStateHelper.GetFirstError(ModelState, "Title");
                return View(model);
            }

            ContestHandle contestHandle = new ContestHandle(contestId);
            using (ContestDataProvider contestData = ContestDataProvider.Create(contestHandle, false))
            {
                // 验证用户操作权限。
                if (!UserSession.IsAuthorized(Session) ||
                    string.Compare(contestData.Creator, UserSession.GetUsername(Session), false) != 0)
                {
                    return Redirect("~/Error/AccessDenied");
                }

                // 在题目库中创建题目。
                ProblemHandle problemHandle = ProblemArchieveManager.Default.CreateProblem(
                    contestData.PeekNextProblemId());

                model.ResetNullStrings();
                model.SaveTo(problemHandle);            // 为避免死锁，该句不应放置在下面的 using 块中。

                using (ProblemDataProvider problemData = ProblemDataProvider.Create(problemHandle, false))
                {
                    problemData.ContestId = contestId;
                    problemData.AuthorizationGroup = UserGroup.Guests;
                }

                // 将创建的题目添加至当前比赛中。
                contestData.AddProblem(problemHandle);
            }

            return Redirect($"~/Contest/Show?id={contestId}");
        }

        // POST: Contest/AddArchieveProblem?contestId={contestID}&problemId={problemID}
        [HttpPost]
        public ActionResult AddArchieveProblem(FormCollection form)
        {
            // 验证参数。
            if (string.IsNullOrEmpty(Request.QueryString["contestId"]) ||
                string.IsNullOrEmpty(form["ProblemId"]))
            {
                return new ContentResult();
            }

            string problemId = form["ProblemId"];

            int contestId;
            if (!int.TryParse(Request.QueryString["contestId"], out contestId))
            {
                return new ContentResult();
            }

            if (!ProblemArchieveManager.Default.IsProblemExist(problemId) ||
                !ContestManager.Default.IsContestExist(contestId))
            {
                return new ContentResult();
            }

            using (ContestDataProvider contestData = ContestDataProvider.Create(new ContestHandle(contestId), false))
            {
                // 检查用户操作权限。
                if (!UserSession.IsAuthorized(Session) ||
                    string.Compare(contestData.Creator, UserSession.GetUsername(Session), false) != 0)
                {
                    return new ContentResult();
                }

                // 执行操作。
                // 创建源题目的精确副本到当前比赛中。
                ProblemHandle newProblemHandle = ProblemArchieveManager.Default.CloneProblem(
                    contestData.PeekNextProblemId(), problemId);
                using (ProblemDataProvider problemData = ProblemDataProvider.Create(newProblemHandle, false))
                {
                    problemData.ContestId = contestId;
                }

                contestData.AddProblem(newProblemHandle);
            }

            return new ContentResult();
        }

        // GET: Contest/Verify?id={ContestID}
        [HttpGet]
        public ActionResult Verify()
        {
            // 检查用户操作权限。
            if (!UserSession.IsAuthorized(Session))
            {
                return LoginController.RequestForLogin(this);
            }

            // 检查 URL 查询参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Contest");
            }

            int id;
            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                return Redirect("~/Contest");
            }

            ContestHandle contest = new ContestHandle(id);
            UserHandle user = new UserHandle(UserSession.GetUsername(Session));
            ContestAuthorizationState state = ContestAuthorization.GetUserAuthorizationState(contest, user);
            
            switch (state.RegisterState)
            {
                case ContestRegisterState.IndividualRegistered:
                case ContestRegisterState.TeamRegistered:
                    return Redirect($"~/Contest/Show?id={id}");

                case ContestRegisterState.PasswordRequired:
                    return View();

                case ContestRegisterState.NotRegistered:
                default:
                    ViewBag.Failed = true;
                    return View();
            }
        }

        // POST: Contest/Verify?id={ContestID}
        [HttpPost]
        public ActionResult Verify(FormCollection form)
        {
            string contestIdString = Request.QueryString["id"];
            if (string.IsNullOrEmpty(contestIdString))
            {
                return Redirect("~/Contest");
            }

            int contestId;
            if (!int.TryParse(Request.QueryString["id"], out contestId))
            {
                return Redirect("~/Contest");
            }

            if (!ContestManager.Default.IsContestExist(contestId))
            {
                return Redirect("~/Contest");
            }

            ContestHandle contest = new ContestHandle(contestId);
            if (!ContestAuthorization.CheckContestPassword(contest, form["Password"]))
            {
                ViewBag.PasswordError = true;
                return View();
            }
            else
            {
                // 检查注册参赛者身份。
                string participant = form["Participant"];
                if (string.IsNullOrEmpty(participant))
                {
                    ViewBag.ParticipantError = true;
                    return View();
                }

                if (participant.StartsWith("(Individual)", StringComparison.CurrentCultureIgnoreCase))
                {
                    string username = participant.Substring("(Individual)".Length);
                    if (!UserManager.Default.IsUserExist(username))
                    {
                        ViewBag.ParticipantError = true;
                        return View();
                    }

                    ContestAuthorization.Register(contest, new UserHandle(username));
                }
                else if (participant.StartsWith("(Teamwork)", StringComparison.CurrentCultureIgnoreCase))
                {
                    int teamId;
                    if (!int.TryParse(participant.Substring("(Teamwork)".Length), out teamId))
                    {
                        ViewBag.ParticipantError = true;
                        return View();
                    }

                    if (!UserManager.Default.IsTeamExist(teamId))
                    {
                        ViewBag.ParticipantError = true;
                        return View();
                    }

                    ContestAuthorization.Register(contest, new TeamHandle(teamId));
                }
                else
                {
                    ViewBag.ParticipantError = true;
                    return View();
                }

                return Redirect(string.Format("~/Contest/Show?id={0}", contestId));
            }
        }

        // GET: Contest/Show?id={ContestID}
        [HttpGet]
        public ActionResult Show()
        {
            // 检查 URL 查询参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Contest");
            }

            int id;
            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                return Redirect("~/Contest");
            }
            ViewBag.ContestId = id;

            // 检查用户权限。
            ContestHandle contest = new ContestHandle(id);
            UserHandle user = UserSession.GetUserHandle(Session);

            switch (ContestAuthorization.GetUserAuthorizationState(contest, user).RegisterState)
            {
                case ContestRegisterState.IndividualRegistered:
                case ContestRegisterState.TeamRegistered:
                    return View();

                case ContestRegisterState.PasswordRequired:
                    return Redirect($"~/Contest/Verify?id={id}");

                case ContestRegisterState.NotRegistered:
                default:
                    return Redirect("~/Contest");
            }
        }
        
        // GET: Contest/Edit?id={ContestID}
        [HttpGet]
        public ActionResult Edit()
        {
            // 检查查询参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Contest");
            }

            int contestId;
            if (!int.TryParse(Request.QueryString["id"], out contestId))
            {
                return Redirect("~/Contest");
            }

            if (!ContestManager.Default.IsContestExist(contestId))
            {
                return Redirect("~/Contest");
            }

            ContestHandle contest = new ContestHandle(contestId);
            using (ContestDataProvider data = ContestDataProvider.Create(contest, false))
            {
                // 验证用户身份权限。
                if (!UserSession.IsAuthorized(Session) ||
                    string.Compare(data.Creator, UserSession.GetUsername(Session), false) != 0)
                {
                    return Redirect("~/Error/AccessDenied");
                }
            }

            ContestDetailModel model = ContestDetailModel.FromContestHandle(contest);
            return View(model);
        }

        // POST: Contest/Edit
        [HttpPost]
        public ActionResult Edit(ContestDetailModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // 验证数据模型。
            if (!ValidateContestDetailModel(model))
            {
                return View(model);
            }

            // 执行用户身份验证。
            ContestHandle handle = new ContestHandle(model.ContestId);
            using (ContestDataProvider data = ContestDataProvider.Create(handle, true))
            {
                if (!UserSession.IsAuthorized(Session) ||
                    string.Compare(data.Creator, UserSession.GetUsername(Session), false) != 0)
                {
                    return Redirect("~/Error/AccessDenied");
                }
            }

            model.SaveTo(handle);
            return Redirect($"~/Contest/Show?id={model.ContestId}");
        }

        // POST: Contest/SetAnnouncement?id={ContestID}
        [HttpPost]
        public ActionResult SetAnnouncement(FormCollection form)
        {
            string contestIdString = Request.QueryString["id"];
            if (string.IsNullOrEmpty(contestIdString))
            {
                return new ContentResult();
            }

            int contestId;
            if (!int.TryParse(contestIdString, out contestId))
            {
                return new ContentResult();
            }

            string announcement = form["announcementContent"];
            if (string.IsNullOrEmpty(announcement))
            {
                return new ContentResult();
            }

            // 检查用户身份权限。
            ContestHandle contest = new ContestHandle(contestId);
            UserHandle user = UserSession.GetUserHandle(Session);
            if (!ContestAuthorization.GetUserAccess(contest, user).HasFlag(DataAccess.Write))
            {
                // 用户对当前的比赛没有写权限。
                return new ContentResult();
            }

            using (ContestDataProvider contestData = ContestDataProvider.Create(contest, false))
            {
                contestData.Announcement = announcement;
            }

            return new ContentResult();
        }

        // POST: Contest/Delete?id={ContestID}
        [HttpPost]
        public ActionResult Delete()
        {
            string contestIdString = Request.QueryString["id"];
            if (string.IsNullOrEmpty(contestIdString))
            {
                return new ContentResult();
            }

            int contestId;
            if (!int.TryParse(contestIdString, out contestId))
            {
                return new ContentResult();
            }

            ContestHandle contest = new ContestHandle(contestId);
            UserHandle user = UserSession.GetUserHandle(Session);

            // 检查用户操作权限。
            if (!ContestAuthorization.GetUserAccess(contest, user).HasFlag(DataAccess.Write))
            {
                // 用户对该比赛没有写权限。
                return new ContentResult();
            }

            ContestManager.Default.RemoveContest(contestId);
            return new ContentResult();
        }
    }
}