namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 提供题目概要数据模型。
    /// </summary>
    public class ProblemInfoModel
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目类别。
        /// </summary>
        public ICollection<string> Categories { get; set; }

        /// <summary>
        /// 获取本题目的全部提交数目。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取本题目的 AC 提交数目。
        /// </summary>
        public int TotalAccepted { get; set; }

        /// <summary>
        /// 获取或设置能够访问当前题目的最低用户权限。
        /// </summary>
        public UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 创建 ProblemInfoModel 类的新实例。
        /// </summary>
        public ProblemInfoModel()
        {
            ProblemId = string.Empty;
            Title = string.Empty;
            Categories = new List<string>();
            TotalSubmissions = 0;
            TotalAccepted = 0;
            AuthorizationGroup = UserGroup.Guests;
        }

        /// <summary>
        /// 从给定的题目句柄创建 ProblemInfoModel 对象。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <returns>从给定的题目句柄创建的 ProblemInfoModel 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ProblemInfoModel FromProblemHandle(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));
            
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
            {
                return new ProblemInfoModel()
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