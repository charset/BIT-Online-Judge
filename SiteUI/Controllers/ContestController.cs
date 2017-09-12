namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Models;
    using BITOJ.SiteUI.Models.Validation;
    using System;
    using System.Text;
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

            return validationPassed;
        }

        // GET: Contest
        public ActionResult Index()
        {
            // 构造比赛查询参数。
            ContestQueryParameter query = new ContestQueryParameter()
            {
                Title = Request.QueryString["title"],
                Creator = Request.QueryString["creator"]
            };
            if (!string.IsNullOrEmpty(query.Title))
            {
                query.QueryByTitle = true;
            }
            if (!string.IsNullOrEmpty(query.Creator))
            {
                query.QueryByCreator = true;
            }

            int catalogIndex = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["catalog"]))
            {
                if (int.TryParse(Request.QueryString["catalog"], out catalogIndex))
                {
                    if (catalogIndex > 0 && catalogIndex < 4)
                    {
                        query.QueryByStatus = true;
                        query.Status = CatalogStatus[catalogIndex - 1];
                    }
                }
            }

            // 执行查询。
            IPageableQueryResult<ContestHandle> contests = ContestManager.Default.QueryContests(query);

            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                int.TryParse(Request.QueryString["page"], out page);
            }

            ContestModel model = new ContestModel()
            {
                Pages = contests.GetTotalPages(ItemsPerPage),
                CurrentPage = page,
                Catalog = (ContestCatalog)catalogIndex
            };

            // 执行分页。
            foreach (ContestHandle handle in contests.Page(page, ItemsPerPage))
            {
                model.Contests.Add(ContestBriefModel.FromContestHandle(handle));
            }

            return View(model);
        }

        // POST: Contest/Query
        [HttpPost]
        public ActionResult Query(FormCollection form)
        {
            StringBuilder queryBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Request.QueryString["catalog"]))
            {
                queryBuilder.AppendFormat("catalog={0}&", Request.QueryString["catalog"]);
            }
            if (!string.IsNullOrEmpty(form["Title"]))
            {
                queryBuilder.AppendFormat("title={0}&", form["Title"]);
            }
            if (!string.IsNullOrEmpty(form["Creator"]))
            {
                queryBuilder.AppendFormat("creator={0}&", form["Creator"]);
            }

            return Redirect("~/Contest?" + queryBuilder.ToString());
        }

        // GET: Contest/Add
        public ActionResult Add()
        {
            // 执行用户身份验证。
            if (!UserSession.IsAuthorized(Session) || !UserAuthorization.CheckAccessRights(UserGroup.Insiders,
                UserSession.GetUserGroup(Session)))
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

        // GET: Contest/Verify?id={ContestID}
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
            ContestAuthorizationState state = ContestAuthorization.GetAuthorizationState(contest, user);
            
            switch (state)
            {
                case ContestAuthorizationState.Authorized:
                    return Redirect($"~/Contest/Show?id={id}");

                case ContestAuthorizationState.AuthorizationRequired:
                    return View();

                case ContestAuthorizationState.AuthorizationFailed:
                default:
                    ViewBag.Failed = true;
                    return View();
            }
        }

        // GET: Contest/Show?id={ContestID}
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

            // 检查用户权限。
            ContestHandle contest = new ContestHandle(id);
            UserHandle user = new UserHandle(UserSession.GetUsername(Session));

            switch (ContestAuthorization.GetAuthorizationState(contest, user))
            {
                case ContestAuthorizationState.Authorized:
                    return View(ContestDisplayModel.FromContestHandle(contest));

                case ContestAuthorizationState.AuthorizationRequired:
                    return Redirect($"~/Contest/Verify?id={id}");

                case ContestAuthorizationState.AuthorizationFailed:
                default:
                    return Redirect("~/Contest");
            }
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
        
        // GET: Contest/Edit?id={ContestID}
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

        // GET: Contest/AddProblem?id={ContestID}
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
    }
}