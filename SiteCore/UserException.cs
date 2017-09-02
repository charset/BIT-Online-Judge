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
        public UserHandle UserHandle { get; private set; }

        /// <summary>
        /// 使用给定的用户名创建 UserException 类的一个新实例。
        /// </summary>
        /// <param name="user">引发异常的用户句柄。</param>
        public UserException(UserHandle user) : base($"BITOJ 用户 \"{user.Username}\" 引发了一个异常。")
        {
            UserHandle = user;
        }

        /// <summary>
        /// 使用给定的用户名和异常消息创建 UserException 类的一个新实例。
        /// </summary>
        /// <param name="user">引发异常的用户句柄。</param>
        /// <param name="message">异常消息。</param>
        public UserException(UserHandle user, string message) : base(message)
        {
            UserHandle = user;
        }
    }
}
