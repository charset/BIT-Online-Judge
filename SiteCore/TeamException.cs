namespace BITOJ.Core
{
    using System;

    /// <summary>
    /// 表示与队伍系统有关的异常。
    /// </summary>
    public class TeamException : Exception
    {
        /// <summary>
        /// 获取或设置队伍句柄。
        /// </summary>
        public TeamHandle Handle { get; private set; }

        /// <summary>
        /// 使用给定的队伍句柄创建 TeamException 类的新实例。
        /// </summary>
        /// <param name="handle">队伍句柄。</param>
        public TeamException(TeamHandle handle) : base($"队伍 {handle.TeamId} 触发了异常。")
        {
            Handle = handle;
        }

        /// <summary>
        /// 使用给定的队伍句柄和异常消息创建 TeamException 类的新实例。
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="message"></param>
        public TeamException(TeamHandle handle, string message) : base(message)
        {
            Handle = handle;
        }
    }
}
