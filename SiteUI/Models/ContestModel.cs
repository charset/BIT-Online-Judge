namespace BITOJ.SiteUI.Models
{
    using System.Collections.Generic;
    
    /// <summary>
    /// 为比赛列表页面提供数据模型。
    /// </summary>
    public class ContestModel
    {
        /// <summary>
        /// 获取或设置比赛总页数。
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// 获取或设置当前页编号。页编号从 1 开始。
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 获取或设置当前比赛目录信息。
        /// </summary>
        public ContestCatalog Catalog { get; set; }

        /// <summary>
        /// 获取或设置当前 Contest 视图中应当呈现的题目。
        /// </summary>
        public ICollection<ContestBriefModel> Contests { get; set; }

        /// <summary>
        /// 创建 ContestModel 类的新实例。
        /// </summary>
        public ContestModel()
        {
            Pages = 0;
            CurrentPage = 0;
            Catalog = ContestCatalog.All;
            Contests = new List<ContestBriefModel>();
        }
    }
}