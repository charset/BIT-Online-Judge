namespace BITOJ.SiteUI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 为题目查询提供数据模型。
    /// </summary>
    public class ProblemQueryModel
    {
        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置题目的 Tag。
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 创建 ProblemQueryModel 类的新实例。
        /// </summary>
        public ProblemQueryModel()
        {
            Title = string.Empty;
            Source = string.Empty;
            Author = string.Empty;
            Tag = string.Empty;
        }
    }
}