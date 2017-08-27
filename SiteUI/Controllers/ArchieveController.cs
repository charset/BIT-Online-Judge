namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Core.Resolvers;
    using BITOJ.SiteUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ArchieveController : Controller
    {
        private const int ItemsPerPage = 50;

        // GET: Archieve
        public ActionResult Index()
        {
            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                if (!int.TryParse(Request.QueryString["page"], out page))
                {
                    page = 1;
                }
            }

            // 在数据库中查询符合查询条件的题目。
            ArchieveModel model = new ArchieveModel()
            {
                Catalog = ArchieveCatalog.Local,
                CurrentPage = page,
            };

            ProblemArchieveQueryParameter query = new ProblemArchieveQueryParameter()
            {
                QueryByTitle = !string.IsNullOrEmpty(Request.QueryString["title"]),
                Title = Request.QueryString["title"],
                QueryBySource = !string.IsNullOrEmpty(Request.QueryString["source"]),
                Source = Request.QueryString["source"],
                QueryByAuthor = !string.IsNullOrEmpty(Request.QueryString["author"]),
                Author = Request.QueryString["author"],
                QueryByOrigin = true,
                Origin = OJSystemConvert.ConvertFromString(Request.QueryString["origin"] ?? "BIT"),
                Page = new PagedQueryParameters(page, ItemsPerPage),
            };

            ICollection<ProblemHandle> result = ProblemArchieveManager.Default.QueryProblems(query);
            foreach (ProblemHandle handle in result)
            {
                model.Problems.Add(ProblemBriefModel.FromProblemHandle(handle));
            }

            model.Pages = result.Count / ItemsPerPage + Math.Sign(result.Count % ItemsPerPage);
            return View(model);
        }

        // POST: Archieve
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string url = string.Format("/Archieve?title={0}&source={1}&author={2}&category={3}&origin={4}",
                form["title"] ?? string.Empty, form["source"] ?? string.Empty, form["author"] ?? string.Empty,
                form["category"] ?? string.Empty, form["origin"] ?? "BIT");
            return Redirect(url);
        }

        // GET: Archieve/Add
        public ActionResult Add()
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                // 用户权限不足。
                return Redirect("~/Error/AccessDenied");
            }

            return View(new ProblemDisplayModel());
        }

        // POST: Archieve/Add
        [HttpPost]
        public ActionResult Add(ProblemDetailModel model)
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                // 用户权限不足。
                return Redirect("~/Error/AccessDenied");
            }

            // 检查数据模型验证状态。
            bool hasError = false;
            TryValidateModel(model);
            if (ModelState.ContainsKey("Id") && ModelState["Id"].Errors.Count > 0)
            {
                hasError = true;
                ViewBag.IdErrorMessage = ModelState["Id"].Errors[0].ErrorMessage;
            }
            if (ModelState.ContainsKey("Title") && ModelState["Title"].Errors.Count > 0)
            {
                hasError = true;
                ViewBag.TitleErrorMessage = ModelState["Title"].Errors[0].ErrorMessage;
            }

            if (hasError)
            {
                return View(model);
            }

            string problemId = "BIT" + model.ProblemId;
            if (ProblemArchieveManager.Default.IsProblemExist(problemId))
            {
                ViewBag.IdErrorMessage = "Problem with the same ID already exist in the archieve.";
                return View(model);
            }

            // 在题目库中创建新题目。
            ProblemHandle handle = ProblemArchieveManager.Default.CreateProblem(problemId);
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, false))
            {
                model.SaveToProblemDataProvider(data);
            }

            return Redirect("~/Archieve");
        }

        // GET: Archieve/Modify
        public ActionResult Modify()
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                return Redirect("~/Error/AcceesDenied");
            }
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                // 缺少题目 ID。
                return Redirect("~/Error/ProblemNotExist");
            };

            ProblemDetailModel model = new ProblemDetailModel();

            // 查询题目信息。
            ProblemHandle handle = ProblemArchieveManager.Default.GetProblemById(Request.QueryString["id"]);
            if (handle == null)
            {
                // 题目不存在于数据库中。
                return Redirect("~/Error/ProblemNotExist");
            }

            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
            {
                model.LoadFromProblemDataProvider(data);
            }

            return View(model);
        }

        // POST: Archieve/Modify
        [HttpPost]
        public ActionResult Modify(ProblemDetailModel model)
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                return Redirect("~/Error/AccessDenied");
            }

            // 验证模型。
            bool hasError = false;
            TryValidateModel(model);
            if (ModelState["Title"] != null && ModelState["Title"].Errors.Count > 0)
            {
                hasError = true;
                ViewBag.TitleErrorMessage = ModelState["Title"].Errors[0].ErrorMessage;
            }

            if (hasError)
            {
                return View(model);
            }

            // 查询题目实体。
            ProblemHandle handle = ProblemArchieveManager.Default.GetProblemById(model.ProblemId);
            if (handle == null)
            {
                return Redirect("~/Error/ProblemNotExist");
            }
            
            // 写入修改后的数据。
            model.ResetNullStrings();
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, false))
            {
                model.SaveToProblemDataProvider(data);
            }

            return Redirect("~/Archieve");
        }

        // GET: Archieve/Delete
        public ActionResult Delete()
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                return Redirect("~/Error/AccessDenied");
            }
            if (Request.QueryString["id"] == null)
            {
                return Redirect("~/Error/ProblemNotExist");
            }

            ProblemArchieveManager.Default.RemoveProblem(Request.QueryString["id"]);
            return Redirect("~/Archieve");
        }

        // GET: Archieve/ShowProblem?id=...
        public ActionResult ShowProblem(string id = null)
        {
            if (id == null)
            {
                return Redirect("~/Archieve");
            }

            ProblemHandle handle = new ProblemHandle(id);
            if (handle.IsNativeProblem)
            {
                // 在数据库中查询题目信息。
                handle = ProblemArchieveManager.Default.GetProblemById(id);
                if (handle == null)
                {
                    // 题目不存在。
                    return Redirect("~/Error/ProblemNotExist");
                }

                ProblemDisplayModel model = new ProblemDisplayModel();
                using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
                {
                    model.LoadFromProblemDataProvider(data);
                }

                return View(model);
            }
            else
            {
                // 解析远程 OJ 题目 URL。
                IProblemUrlResolver resolver = ProblemUrlResolverFactory.GetUrlResolverFor(handle);
                if (resolver == null)
                {
                    // 没有此题目来源 OJ 的 URL 解析器。
                    return Redirect("~/Error/OJNotSupported");
                }
                else
                {
                    return Redirect(resolver.Resolve(handle));
                }
            }
        }
    }
}