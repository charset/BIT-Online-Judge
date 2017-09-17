namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Controllers.Extensions;
    using BITOJ.SiteUI.Models;
    using BITOJ.SiteUI.Models.Ajax;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class StatusController : Controller
    {
        private const int ItemsPerPage = 50;

        // GET: /Status?username={Username}&problemId={ProblemID}
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

            /* Test code start */
            model.Submissions.Add(new SubmissionBriefModel()
            {
                SubmissionId = 19980325,
                Username = "Lancern",
                ProblemId = "BIT1000",
                ProblemTitle = "bibibibi and his demonstratable interval tree",
                ExecutionTime = 100,
                ExecutionMemory = 40,
                Language = SubmissionLanguage.GnuCPlusPlus11,
                VerdictStatus = SubmissionVerdictStatus.Completed,
                Verdict = SubmissionVerdict.MemoryLimitExceeded,
                CreationTime = System.DateTime.Now
            });
            /* Test code end */

            return View(model);
        }

        // POST: /Status
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            // 检查查询参数。
            List<string> parameters = new List<string>();
            if (!string.IsNullOrEmpty(form["username"]))
            {
                parameters.Add($"username={form["username"]}");
            }
            if (!string.IsNullOrEmpty(form["problemId"]))
            {
                parameters.Add($"problemId={form["problemId"]}");
            }
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                parameters.Add($"page={Request.QueryString["page"]}");
            }

            return Redirect($"~/Status?{string.Join("&", parameters)}");
        }

        // GET: /Status/ShowSubmission?id={SubmissionID}
        public ActionResult ShowSubmission()
        {
            // 检查查询参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return Redirect("~/Status");
            }

            int submissionId;
            if (!int.TryParse(Request.QueryString["id"], out submissionId))
            {
                return Redirect("~/Status");
            }

            SubmissionHandle handle = SubmissionManager.Default.QuerySubmissionById(submissionId);
            if (handle == null)
            {
                // 给定的用户提交不存在。
                return Redirect("~/Status");
            }

            SubmissionDetailModel model = SubmissionDetailModel.FromSubmissionHandle(handle);
            return View(model);
        }

        // POST: Status/QueryStatus?id={SubmissionID}
        [HttpPost]
        public ActionResult QueryStatus()
        {
            // 检查参数。
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                return new ContentResult();
            }

            int submissionId;
            if (!int.TryParse(Request.QueryString["id"], out submissionId))
            {
                return new ContentResult();
            }

            SubmissionHandle handle = new SubmissionHandle(submissionId);
            return this.NewtonsoftJson(SubmissionVerdictRequestModel.FromSubmissionHandle(handle));
        }
    }
}