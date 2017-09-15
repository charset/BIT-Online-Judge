namespace BITOJ.Core
{
    /// <summary>
    /// 表示题目的解决状态。
    /// </summary>
    public enum ProblemSolutionStatus
    {
        /// <summary>
        /// 无解决状态。
        /// </summary>
        None,

        /// <summary>
        /// 用户已经尝试但尚未通过。
        /// </summary>
        Attempted,

        /// <summary>
        /// 用户已经顺利通过。
        /// </summary>
        Solved
    }
}
