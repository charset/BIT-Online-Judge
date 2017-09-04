namespace BITOJ.Core
{
    using System;

    /// <summary>
    /// 表示用户提交系统异常。
    /// </summary>
    public class SubmissionException : Exception
    {
        /// <summary>
        /// 获取引发异常的用户提交句柄。
        /// </summary>
        public SubmissionHandle Handle { get; private set; }

        /// <summary>
        /// 使用给定的用户提交句柄创建 SubmissionException 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        public SubmissionException(SubmissionHandle handle) : base($"用户提交 {handle.SubmissionId} 引发异常。")
        {
            Handle = handle;
        }

        /// <summary>
        /// 使用给定到的用户提交句柄与异常信息创建 SubmissionException 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        /// <param name="message">异常消息。</param>
        public SubmissionException(SubmissionHandle handle, string message) : base(message)
        {
            Handle = handle;
        }
    }
}
