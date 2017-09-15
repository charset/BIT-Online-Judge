namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Models;
    using System.Web.Mvc;

    public class StatusController : Controller
    {
        private const int ItemsPerPage = 50;

        // GET: Status?username={Username}&problemId={ProblemID}
        public ActionResult Index()
        {
            // 检查查询参数。
            SubmissionQueryParameter query = new SubmissionQueryParameter()
            {
                ContestId = -1,
                QueryByContestId = true,
                OrderByDescending = true,
            };
            if (!string.IsNullOrEmpty(Request.QueryString["username"]))
            {
                query.QueryByUsername = true;
                query.Username = Request.QueryString["username"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["problemId"]))
            {
                query.QueryByProblemId = true;
                query.ProblemId = Request.QueryString["problemId"];
            }

            StatusModel model = new StatusModel();
            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                int.TryParse(Request.QueryString["page"], out page);
            }

            // 执行查询。
            IPageableQueryResult<SubmissionHandle> result = SubmissionManager.Default.QuerySubmissions(query);
            model.TotalPages = result.GetTotalPages(ItemsPerPage);

            foreach (SubmissionHandle item in result.Page(page, ItemsPerPage))
            {
                model.Submissions.Add(SubmissionBriefModel.FromSubmissionHandle(item));
            }

            return View(model);
        }
    }
}