namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Models;
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
            QueryResult<ContestHandle> contests = ContestManager.Default.QueryContests(query);

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
    }
}