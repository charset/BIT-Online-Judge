namespace BITOJ.Core.Data
{
    /// <summary>
    /// 表示用户提交未找到的异常。
    /// </summary>
    public sealed class SubmissionNotExistException : SubmissionException
    {
        /// <summary>
        /// 使用给定的用户提交句柄创建 SubmissionNotExistException 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        public SubmissionNotExistException(SubmissionHandle handle) : base(handle, "用户提交未找到。")
        { }

        /// <summary>
        /// 使用给定的用户提交句柄和异常消息创建 SubmissionNotExistException 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        /// <param name="message">异常消息。</param>
        public SubmissionNotExistException(SubmissionHandle handle, string message) : base(handle, message)
        { }
    }
}
