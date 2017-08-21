namespace BITOJ.Core
{
    using System;

    /// <summary>
    /// 表示与用户系统有关的异常。
    /// </summary>
    public class UserException : Exception
    {
        /// <summary>
        /// 获取引发异常的用户名。
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// 使用给定的用户名创建 UserException 类的一个新实例。
        /// </summary>
        /// <param name="username">引发异常的用户名。</param>
        public UserException(string username) : base($"BITOJ 用户 \"{username}\" 引发了一个异常。")
        {
            Username = username;
        }

        /// <summary>
        /// 使用给定的用户名和异常消息创建 UserException 类的一个新实例。
        /// </summary>
        /// <param name="username">引发异常的用户名。</param>
        /// <param name="message">异常消息。</param>
        public UserException(string username, string message) : base(message)
        {
            Username = username;
        }
    }
}
