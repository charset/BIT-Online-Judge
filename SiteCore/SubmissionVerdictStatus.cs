namespace BITOJ.Core
{
    /// <summary>
    /// 编码用户提交的判题状态。
    /// </summary>
    public enum SubmissionVerdictStatus : int
    {
        /// <summary>
        /// 用户提交正在等待被发送至判题器系统。
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 用户提交已经提交到判题器系统。正在判题器系统内部的判题队列中等待。
        /// </summary>
        Submitted = 1,

        /// <summary>
        /// 用户提交正在判题系统上被编译。
        /// </summary>
        Compiling = 2,

        /// <summary>
        /// 用户提交正在判题系统上运行。
        /// </summary>
        Running = 3,

        /// <summary>
        /// 用户提交判题过程已结束。
        /// </summary>
        Completed = 4,
    }
}
