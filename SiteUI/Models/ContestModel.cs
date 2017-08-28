
namespace BITOJ.SiteUI.Models
{
    using System.Collections.Generic;
    
    /// <summary>
    /// 
    /// </summary>
    public class ContestModel
    {
        /// <summary>
        /// 获取或设置当前 Contest 视图中应当呈现的题目。
        /// </summary>
        public ICollection<ContestInfoModel> Contests { get; set; }

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

        public ContestModel()
        {
            Contests = new List<ContestInfoModel>();
            Pages = 0;
            CurrentPage = 0;
            Catalog = ContestCatalog.All;
        }
    }
}