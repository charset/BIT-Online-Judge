namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
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
            return View();
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

            SubmissionHandle submissionHandle = SubmissionManager.Default.QuerySubmissionById(submissionId);
            if (submissionHandle == null)
            {
                // 给定的用户提交不存在。
                return Redirect("~/Status");
            }

            // 验证操作权限。
            using (SubmissionDataProvider submissionData = SubmissionDataProvider.Create(submissionHandle, true))
            {
                if (submissionData.ContestId != -1)
                {
                    ContestHandle contestHandle = new ContestHandle(submissionData.ContestId);
                    using (ContestDataProvider contestData = ContestDataProvider.Create(contestHandle, true))
                    {   
                        if (contestData.Status != ContestStatus.Ended &&
                            string.Compare(submissionData.Username, UserSession.GetUsername(Session)) != 0)
                        {
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                }
            }

            SubmissionDetailModel model = SubmissionDetailModel.FromSubmissionHandle(submissionHandle);
            return View(model);
        }

        // AJAX POST: Status/QueryStatus?id={SubmissionID}
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