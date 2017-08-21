namespace BITOJ.Core.Data
{
    using BITOJ.Core;

	/// <summary>
    /// 表示给定的用户未在用户数据库中找到的异常。
    /// </summary>
    public sealed class UserNotFoundException : UserException
    {
		/// <summary>
        /// 使用给定的用户名创建 UserNotFoundException 类的一个新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
		public UserNotFoundException(string username) : base(username, $"用户 \" {username} \" 不存在。")
        { }

		/// <summary>
        /// 使用给定的用户名和异常消息创建 UserNotFoundException 类的一个新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="message">异常消息。</param>
		public UserNotFoundException(string username, string message) : base(username, message)
        { }
    }
}
