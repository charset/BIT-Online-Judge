namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
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

        // GET: Archieve/ShowProblem?id=...
        public ActionResult ShowProblem(string id = null)
        {
            return View();
        }
    }
}