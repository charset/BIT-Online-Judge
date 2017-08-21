namespace BITOJ.Core
{
    /// <summary>
    /// 表示在创建新用户时用户名已经被占用的异常。
    /// </summary>
    public sealed class UsernameAlreadyExistsException : UserException
    {
        /// <summary>
        /// 根据指定的用户名创建 UsernameAlreadyExistsException 类的新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
        public UsernameAlreadyExistsException(string username) : base(username, $"用户名 \" {username} \" 已经被占用。")
        { }

        /// <summary>
        /// 根据指定的用户名和异常消息创建 UsernameAlreadyExistsException 类的新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="message">异常消息。</param>
        public UsernameAlreadyExistsException(string username, string message) : base(username, message)
        { }
    }
}
