namespace BITOJ.Core
{
    using System;

    /// <summary>
    /// 表示比赛系统异常。
    /// </summary>
    public class ContestException : Exception
    {
        /// <summary>
        /// 获取引发异常的比赛句柄。
        /// </summary>
        public ContestHandle Handle { get; private set; }

        /// <summary>
        /// 从给定的比赛句柄创建 ContestException 类的新实例。
        /// </summary>
        /// <param name="handle"></param>
        public ContestException(ContestHandle handle) : base("比赛系统发生异常。")
        {
            Handle = handle;
        }

        /// <summary>
        /// 从给定的比赛句柄以及异常消息创建 ContestException 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <param name="message">异常消息。</param>
        public ContestException(ContestHandle handle, string message) : base(message)
        {
            Handle = handle;
        }
    }
}
