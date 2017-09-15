namespace BITOJ.SiteUI.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 对 Status 视图提供数据模型。
    /// </summary>
    public class StatusModel
    {
        /// <summary>
        /// 获取或设置当前的提交页。
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 获取或设置总提交页数。
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 获取或设置所有提交。
        /// </summary>
        public ICollection<SubmissionBriefModel> Submissions { get; set; }
        
        /// <summary>
        /// 创建 StatusModel 类的新实例。
        /// </summary>
        public StatusModel()
        {
            Page = 1;
            TotalPages = 1;
            Submissions = new List<SubmissionBriefModel>();
        }
    }
}