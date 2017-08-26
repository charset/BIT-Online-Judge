namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ArchieveController : Controller
    {
        private const int ItemsPerPage = 50;

        // GET: Archieve
        public ActionResult Index(int page = 1)
        {
            // 在数据库中查询所有题目来源为 BITOJ 的题目。
            ArchieveModel model = new ArchieveModel()
            {
                Catalog = ArchieveCatalog.Local,
                CurrentPage = page,
            };

            ProblemArchieveQueryParameter query = new ProblemArchieveQueryParameter()
            {
                QueryByOrigin = true,
                Origin = OJSystem.BIT,
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

        // GET: Archieve/Add
        public ActionResult Add()
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                // 用户权限不足。
                return Redirect("~/Error/AccessDenied");
            }

            return View(new AddProblemModel());
        }

        // POST: Archieve/Add
        [HttpPost]
        public ActionResult Add(AddProblemModel model)
        {
            if (!UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserSession.GetUserGroup(Session)))
            {
                // 用户权限不足。
                return Redirect("~/Error/AccessDenied");
            }

            // 检查数据模型验证状态。
            model.ResetErrorMessages();
            bool hasError = false;

            TryValidateModel(model);
            if (ModelState.ContainsKey("Id") && ModelState["Id"].Errors.Count > 0)
            {
                hasError = true;
                model.IdErrorMessage = ModelState["Id"].Errors[0].ErrorMessage;
            }
            if (ModelState.ContainsKey("Title") && ModelState["Title"].Errors.Count > 0)
            {
                hasError = true;
                model.TitleErrorMessage = ModelState["Title"].Errors[0].ErrorMessage;
            }

            if (hasError)
            {
                return View(model);
            }

            model.ReplaceNullStringsToEmptyStrings();

            string problemId = "BIT" + model.Id;
            if (ProblemArchieveManager.Default.IsProblemExist(problemId))
            {
                model.IdErrorMessage = "Problem with the same ID already exist in the archieve.";
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
            return View();
        }
    }
}