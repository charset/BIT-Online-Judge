namespace BITOJ.Core
{
    /// <summary>
    /// 表示比赛状态。
    /// </summary>
    public enum ContestStatus
    {
        /// <summary>
        /// 比赛正在等待开始。
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 比赛正在进行。
        /// </summary>
        Running = 1,

        /// <summary>
        /// 比赛已经结束。
        /// </summary>
        Ended = 2
    }
}
