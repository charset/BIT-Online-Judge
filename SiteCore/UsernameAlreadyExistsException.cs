namespace BITOJ.Core
{
    /// <summary>
    /// 表示在创建新用户时用户名已经被占用的异常。
    /// </summary>
    public sealed class UsernameAlreadyExistsException : UserException
    {
        /// <summary>
        /// 根据指定的用户句柄创建 UsernameAlreadyExistsException 类的新实例。
        /// </summary>
        /// <param name="user">用户句柄。</param>
        public UsernameAlreadyExistsException(UserHandle user) : base(user, $"用户名 \" {user.Username} \" 已经被占用。")
        { }

        /// <summary>
        /// 根据指定的用户句柄和异常消息创建 UsernameAlreadyExistsException 类的新实例。
        /// </summary>
        /// <param name="user">用户句柄。</param>
        /// <param name="message">异常消息。</param>
        public UsernameAlreadyExistsException(UserHandle user, string message) : base(user, message)
        { }
    }
}
