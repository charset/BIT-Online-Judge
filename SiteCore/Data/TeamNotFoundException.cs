namespace BITOJ.Core.Data
{
    using BITOJ.Core;

    /// <summary>
    /// 表示队伍不存在异常。
    /// </summary>
    public sealed class TeamNotFoundException : TeamException
    {
        /// <summary>
        /// 使用给定的队伍句柄创建 TeamNotFoundException 类的新实例。
        /// </summary>
        /// <param name="handle">队伍句柄。</param>
        public TeamNotFoundException(TeamHandle handle) : base(handle, "给定的队伍不存在。")
        { }

        /// <summary>
        /// 使用给定的队伍句柄和异常消息创建 TeamNotFoundException 类的新实例。
        /// </summary>
        /// <param name="handle">队伍句柄。</param>
        /// <param name="message">异常消息。</param>
        public TeamNotFoundException(TeamHandle handle, string message) : base(handle, message)
        { }
    }
}
