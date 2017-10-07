namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 提供题目概要数据模型。
    /// </summary>
    public class ProblemBriefModel
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        [JsonProperty("problemId")]
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目类别。
        /// </summary>
        [JsonProperty("categories")]
        public ICollection<string> Categories { get; set; }

        /// <summary>
        /// 获取本题目的全部提交数目。
        /// </summary>
        [JsonProperty("totalsub")]
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取本题目的 AC 提交数目。
        /// </summary>
        [JsonProperty("acsub")]
        public int TotalAccepted { get; set; }

        /// <summary>
        /// 获取或设置能够访问当前题目的最低用户权限。
        /// </summary>
        [JsonProperty("auth")]
        public UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 获取或设置当前题目的解决状态。
        /// </summary>
        [JsonProperty("solution")]
        public ProblemSolutionStatus SolutionStatus { get; set; }

        /// <summary>
        /// 创建 ProblemInfoModel 类的新实例。
        /// </summary>
        public ProblemBriefModel()
        {
            ProblemId = string.Empty;
            Title = string.Empty;
            Categories = new List<string>();
            TotalSubmissions = 0;
            TotalAccepted = 0;
            AuthorizationGroup = UserGroup.Guests;
            SolutionStatus = ProblemSolutionStatus.None;
        }

        /// <summary>
        /// 返回当前数据模型在呈现时应该对 row 元素应用的样式类。
        /// </summary>
        /// <returns>当前数据模型在呈现时应该对 row 元素应用的样式类。</returns>
        public string GetTableRowClassName()
        {
            switch (SolutionStatus)
            {
                case ProblemSolutionStatus.Attempted:
                    return "warning";
                case ProblemSolutionStatus.Solved:
                    return "success";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 从给定的题目句柄创建 ProblemInfoModel 对象。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <returns>从给定的题目句柄创建的 ProblemInfoModel 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ProblemBriefModel FromProblemHandle(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));
            
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
            {
                return new ProblemBriefModel()
                {
                    ProblemId = handle.ProblemId,
                    Title = data.Title,
                    Categories = data.GetCategories(),
                    TotalSubmissions = data.TotalSubmissions,
                    TotalAccepted = data.AcceptedSubmissions,
                    AuthorizationGroup = data.AuthorizationGroup,
                };
            }
        }
    }
}