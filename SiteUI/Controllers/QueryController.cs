﻿namespace BITOJ.SiteUI.Controllers
{
    using BITOJ.Core;
    using BITOJ.Core.Authorization;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data.Queries;
    using BITOJ.SiteUI.Controllers.Extensions;
    using BITOJ.SiteUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// 为 BITOJ 异步查询提供接口控制器。
    /// </summary>
    public class QueryController : Controller
    {
        private static readonly int ProblemListItemsPerPage = 50;
        private static readonly int ContestListItemsPerPage = 20;

        /// <summary>
        /// 返回表示发生错误的查询结果。
        /// </summary>
        /// <param name="errorCode">错误代码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>表示发生错误的查询结果。</returns>
        [NonAction]
        protected ActionResult QueryResult(int errorCode, string message)
        {
            return this.NewtonsoftJson(new QueryModel()
            {
                ErrorCode = errorCode,
                ErrorMessage = message
            });
        }

        /// <summary>
        /// 返回表示查询成功的查询结果。
        /// </summary>
        /// <param name="model">查询结果数据模型。</param>
        /// <returns>表示查询成功的查询结果。</returns>
        [NonAction]
        protected ActionResult QueryResult(object model)
        {
            QueryModel retModel = new QueryModel
            {
                ErrorCode = 0,
                Data = model
            };

            return this.NewtonsoftJson(retModel);
        }

        /// <summary>
        /// 根据当前的 HttpRequest 创建题目查询对象。
        /// </summary>
        /// <returns>根据当前的 HttpRequest 创建的题目查询对象。</returns>
        [NonAction]
        protected ProblemArchieveQueryParameter CreateProblemArchieveQueryParameter()
        {
            ProblemArchieveQueryParameter query = new ProblemArchieveQueryParameter();
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                query.Title = Request.QueryString["title"];
                query.QueryByTitle = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["author"]))
            {
                query.Author = Request.QueryString["author"];
                query.QueryByAuthor = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["source"]))
            {
                query.Source = Request.QueryString["source"];
                query.QueryBySource = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["origin"]))
            {
                try
                {
                    query.Origin = OJSystemConvert.ConvertFromString(Request.QueryString["origin"]);
                    query.QueryByOrigin = true;
                }
                catch (ArgumentException)
                { }
            }

            query.ContestId = -1;
            query.QueryByContestId = true;
            return query;
        }

        /// <summary>
        /// 根据当前 HttpRequest 创建比赛查询对象。
        /// </summary>
        /// <returns>根据当前的 HttpRequest 创建的题目查询对象。</returns>
        [NonAction]
        protected ContestQueryParameter CreateContestQueryParameter()
        {
            ContestQueryParameter query = new ContestQueryParameter();
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                query.Title = Request.QueryString["title"];
                query.QueryByTitle = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["author"]))
            {
                query.Creator = Request.QueryString["author"];
                query.QueryByCreator = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["status"]))
            {
                try
                {
                    query.Status = ContestStatusConvert.ConvertFromString(Request.QueryString["status"]);
                    query.QueryByStatus = true;
                }
                catch (ArgumentException)
                { }
            }

            return query;
        }

        // GET: /Query/ProblemListPages?title={Title}&author={Author}&source={Source}&origin={Origin}
        [HttpGet]
        public ActionResult ProblemListPages()
        {
            ProblemArchieveQueryParameter query = CreateProblemArchieveQueryParameter();
            query.EnablePageQuery = true;
            query.PageQuery = new PageQueryParameter(1, ProblemListItemsPerPage);

            return QueryResult(ProblemArchieveManager.Default.QueryPages(query));
        }

        // GET: /Query/ProblemList?title={Title}&author={Author}&source={Source}&origin={Origin}&page={Page}
        [HttpGet]
        public ActionResult ProblemList()
        {
            // 获取查询参数。
            ProblemArchieveQueryParameter query = CreateProblemArchieveQueryParameter();

            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                if (!int.TryParse(Request.QueryString["page"], out page))
                {
                    page = 1;
                }
            }

            query.EnablePageQuery = true;
            query.PageQuery = new PageQueryParameter(page, ProblemListItemsPerPage);

            // 执行分页查询。
            IEnumerable<ProblemHandle> queryResult = ProblemArchieveManager.Default.QueryProblems(query);

            List<ProblemBriefModel> problems = new List<ProblemBriefModel>();
            foreach (ProblemHandle handle in queryResult)
            {
                problems.Add(ProblemBriefModel.FromProblemHandle(handle));
            }

            return QueryResult(problems);
        }

        // GET: /Query/ProblemDetail?id={ProblemID}
        [HttpGet]
        public ActionResult ProblemDetail()
        {
            string problemId = Request.QueryString["id"];
            if (string.IsNullOrEmpty(problemId))
            {
                return QueryResult(1, "Problem ID required.");
            }

            ProblemHandle problemHandle = ProblemArchieveManager.Default.GetProblemById(problemId);
            if (problemHandle == null)
            {
                return QueryResult(2, "No such problem.");
            }

            // 检查用户操作权限。
            UserHandle userHandle = UserSession.IsAuthorized(Session)
                ? new UserHandle(UserSession.GetUsername(Session))
                : null;
            if (!ProblemAuthorization.Check(problemHandle, userHandle).HasFlag(DataAccess.Read))
            {
                return QueryResult(3, "Access denied.");
            }

            return QueryResult(ProblemDisplayModel.FromProblemHandle(problemHandle));
        }

        // GET: /Query/ContestListPages?title={Title}&creator={Creator}&status={ContestStatus}
        [HttpGet]
        public ActionResult ContestListPages()
        {
            ContestQueryParameter query = CreateContestQueryParameter();
            query.EnablePagedQuery = true;
            query.PageQuery = new PageQueryParameter(1, ContestListItemsPerPage);

            return QueryResult(ContestManager.Default.QueryPages(query));
        }

        // GET: /Query/ContestList?title={Title}&creator={Creator}&status={ContestStatus}&page={Page}
        [HttpGet]
        public ActionResult ContestList()
        {
            // 获取查询参数。
            ContestQueryParameter query = CreateContestQueryParameter();

            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                if (!int.TryParse(Request.QueryString["page"], out page))
                {
                    page = 1;
                }
            }

            query.EnablePagedQuery = true;
            query.PageQuery = new PageQueryParameter(page, ContestListItemsPerPage);

            // 执行分页查询。
            IEnumerable<ContestHandle> queryResult = ContestManager.Default.QueryContests(query);

            List<ContestBriefModel> contests = new List<ContestBriefModel>();
            foreach (ContestHandle handle in queryResult)
            {
                contests.Add(ContestBriefModel.FromContestHandle(handle));
            }

            return QueryResult(contests);
        }
    }
}