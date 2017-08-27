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
                model.Problems.Add(ProblemInfoModel.FromProblemHandle(handle));
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

            return View(new ProblemDetailModel());
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

            model.ReplaceNullStringsToEmptyStrings();

            string problemId = "BIT" + model.Id;
            if (ProblemArchieveManager.Default.IsProblemExist(problemId))
            {
                ViewBag.IdErrorMessage = "Problem with the same ID already exist in the archieve.";
                return View(model);
            }

            // 在题目库中创建新题目。
            ProblemHandle handle = ProblemArchieveManager.Default.CreateProblem(problemId);
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, false))
            {
                data.Title = model.Title;
                data.Description = model.Description;
                data.InputDescription = model.InputDescription;
                data.OutputDescription = model.OutputDescription;
                data.InputExample = model.InputExample;
                data.OutputExample = model.OutputExample;
                data.Source = model.Source;
                data.Author = model.Author;
                data.Hints = model.Hint;
                data.AuthorizationGroup = UsergroupConvert.ConvertFromString(model.UserGroupName);
            }

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

                ProblemDetailModel model = new ProblemDetailModel();
                using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
                {
                    model.Id = handle.ProblemId;
                    model.Title = data.Title;
                    model.Description = data.Description;
                    model.InputDescription = data.InputDescription;
                    model.OutputDescription = data.OutputDescription;
                    model.InputExample = data.InputExample;
                    model.OutputExample = data.OutputExample;
                    model.Hint = data.Hints;
                    model.Source = data.Source;
                    model.Author = data.Author;
                    model.TimeLimit = data.TimeLimit;
                    model.MemoryLimit = data.MemoryLimit;
                    model.IsSpecialJudge = data.IsSpecialJudge;
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