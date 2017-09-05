namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;

    /// <summary>
    /// 编码比赛库目录类别。
    /// </summary>
    public enum ContestCatalog : int
    {
        /// <summary>
        /// 所有比赛。
        /// </summary>
        All = 0,

        /// <summary>
        /// 已创建但未开始的比赛。
        /// </summary>
        Scheduled = 1,

        /// <summary>
        /// 正在开展的比赛。
        /// </summary>
        Running = 2,

        /// <summary>
        /// 已经结束的比赛。
        /// </summary>
        Ended = 3,
    }
}