namespace BITOJ.Core.Data
{
    using BITOJ.Core;

    /// <summary>
    /// 表示某场比赛不存在的异常。
    /// </summary>
    public sealed class ContestNotFoundException : ContestException
    {
        /// <summary>
        /// 使用给定的比赛句柄创建 ContestNotFoundException 类的新实例。
        /// </summary>
        /// <param name="handle"></param>
        public ContestNotFoundException(ContestHandle handle) : base(handle, $"比赛 {handle.ContestId} 未找到。")
        { }

        /// <summary>
        /// 使用给定的比赛句柄以及异常消息创建 ContestNotFoundException 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <param name="message">异常消息。</param>
        public ContestNotFoundException(ContestHandle handle, string message) : base(handle, message)
        { }
    }
}
