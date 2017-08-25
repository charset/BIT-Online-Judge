namespace BITOJ.SiteUI.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 为 Archieve 视图提供数据模型。
    /// </summary>
    public class ArchieveModel
    {
        /// <summary>
        /// 获取或设置当前 Archieve 视图中应当呈现的题目。
        /// </summary>
        public ICollection<ProblemInfoModel> Problems { get; set; }

        /// <summary>
        /// 获取或设置题目库总页数。
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// 获取或设置当前页编号。页编号从 1 开始。（万恶之源）
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 获取或设置题目库目录信息。
        /// </summary>
        public ArchieveCatalog Catalog { get; set; }

        /// <summary>
        /// 创建 ArchieveModel 类的新实例。
        /// </summary>
        public ArchieveModel()
        {
            Problems = new List<ProblemInfoModel>();
            Pages = 0;
            CurrentPage = 0;
            Catalog = ArchieveCatalog.Local;
        }
    }
}