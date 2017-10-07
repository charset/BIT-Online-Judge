namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Controllers.Extensions;
    using BITOJ.SiteUI.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class InternalController : Controller
    {
        // GET: /Internal/FetchSubmissions
        public ActionResult FetchSubmissions()
        {
            SubmissionFetchRequestModel requestPack = null;
            using (StreamReader reader = new StreamReader(Request.GetBufferedInputStream()))
            {
                try
                {
                    requestPack = JsonConvert.DeserializeObject<SubmissionFetchRequestModel>(reader.ReadToEnd());
                }
                catch
                {
                    return this.NewtonsoftJson(SubmissionFetchResponseModel.Empty);
                }
            }
            
            // 对客户端进行身份验证。
            if (requestPack == null || string.IsNullOrEmpty(requestPack.Password) ||
                !VerdictAuthorization.CheckAuthorization(VerdictAuthorization.GetHashBytes(requestPack.Password)))
            {
                // 客户端身份验证失败。
                return this.NewtonsoftJson(SubmissionFetchResponseModel.Empty);
            }

            SubmissionHandle submission = SubmissionManager.Default.GetPendingListFront();
            if (submission == null)
            {
                return this.NewtonsoftJson(SubmissionFetchResponseModel.Empty);
            }
            else
            {
                SubmissionFetchResponseModel responsePack = 
                    SubmissionFetchResponseModel.FromSubmissionHandle(submission);

                // 更新数据库信息。
                using (SubmissionDataProvider data = SubmissionDataProvider.Create(submission, false))
                {
                    data.VerdictStatus = SubmissionVerdictStatus.Submitted;
                    data.VerdictTimeStamp = DateTime.Now;
                }

                return this.NewtonsoftJson(responsePack);
            }
        }

        // POST: /Internal/UpdateSubmission
        [HttpPost]
        public ActionResult UpdateSubmission()
        {
            SubmissionUpdateRequestModel requestPack = null;
            using (StreamReader reader = new StreamReader(Request.GetBufferedInputStream()))
            {
                try
                {
                    requestPack = JsonConvert.DeserializeObject<SubmissionUpdateRequestModel>(reader.ReadToEnd());
                }
                catch
                {
                    return this.NewtonsoftJson(SubmissionUpdateRespondModel.Failed);
                }
            }

            // 对客户端进行身份验证。
            if (requestPack == null || string.IsNullOrEmpty(requestPack.Password) ||
                !VerdictAuthorization.CheckAuthorization(VerdictAuthorization.GetHashBytes(requestPack.Password)))
            {
                // 身份验证失败。
                return this.NewtonsoftJson(SubmissionUpdateRespondModel.Failed);
            }

            // 执行更新操作。
            if (!SubmissionManager.Default.IsSubmissionExist(requestPack.SubmissionId))
            {
                return this.NewtonsoftJson(SubmissionUpdateRespondModel.Failed);
            }

            SubmissionHandle handle = new SubmissionHandle(requestPack.SubmissionId);
            using (SubmissionDataProvider data = SubmissionDataProvider.Create(handle, false))
            {
                if (data.VerdictStatus == SubmissionVerdictStatus.Completed)
                {
                    // 当前用户提交已经处于处理完毕状态。不执行任何操作。
                    return this.NewtonsoftJson(SubmissionUpdateRespondModel.Succeed);
                }

                data.ExecutionTime = requestPack.ExecutionTime;
                data.ExecutionMemory = requestPack.ExecutionMemory;
                data.Verdict = (SubmissionVerdict)requestPack.Verdict;
                data.VerdictMessage = requestPack.VerdictMessage;
                data.VerdictStatus = SubmissionVerdictStatus.Completed;
            }

            return this.NewtonsoftJson(SubmissionUpdateRespondModel.Succeed);
        }
    }
}