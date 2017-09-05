﻿namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class InternalController : Controller
    {
        private ContentResult NewtonsoftJsonResult(object model)
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(model)
            };
        }

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
                    return NewtonsoftJsonResult(SubmissionFetchResponseModel.Empty);
                }
            }
            
            // 对客户端进行身份验证。
            if (requestPack == null || string.IsNullOrEmpty(requestPack.Password) ||
                !VerdictAuthorization.CheckAuthorization(VerdictAuthorization.GetHashBytes(requestPack.Password)))
            {
                // 客户端身份验证失败。
                return NewtonsoftJsonResult(SubmissionFetchResponseModel.Empty);
            }

            SubmissionHandle submission = SubmissionManager.Default.GetPendingListFront();
            if (submission == null)
            {
                return NewtonsoftJsonResult(SubmissionFetchResponseModel.Empty);
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

                return NewtonsoftJsonResult(responsePack);
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
                    return NewtonsoftJsonResult(SubmissionUpdateRespondModel.Failed);
                }
            }

            // 对客户端进行身份验证。
            if (requestPack == null || string.IsNullOrEmpty(requestPack.Password) ||
                !VerdictAuthorization.CheckAuthorization(VerdictAuthorization.GetHashBytes(requestPack.Password)))
            {
                // 身份验证失败。
                return NewtonsoftJsonResult(SubmissionUpdateRespondModel.Failed);
            }

            // 执行更新操作。
            if (!SubmissionManager.Default.IsSubmissionExist(requestPack.SubmissionId))
            {
                return NewtonsoftJsonResult(SubmissionUpdateRespondModel.Failed);
            }

            SubmissionHandle handle = new SubmissionHandle(requestPack.SubmissionId);
            using (SubmissionDataProvider data = SubmissionDataProvider.Create(handle, false))
            {
                if (data.VerdictStatus == SubmissionVerdictStatus.Competed)
                {
                    // 当前用户提交已经处于处理完毕状态。不执行任何操作。
                    return NewtonsoftJsonResult(SubmissionUpdateRespondModel.Succeed);
                }

                data.ExecutionTime = requestPack.ExecutionTime;
                data.ExecutionMemory = requestPack.ExecutionMemory;
                data.Verdict = (SubmissionVerdict)requestPack.Verdict;
                data.VerdictMessage = requestPack.VerdictMessage;
                data.VerdictStatus = SubmissionVerdictStatus.Competed;
            }

            return NewtonsoftJsonResult(SubmissionUpdateRespondModel.Succeed);
        }
    }
}