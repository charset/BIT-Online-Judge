namespace BITOJ.Core
{
    using System;

    /// <summary>
    /// 当给定的题目不存在时引发此异常。
    /// </summary>
    public sealed class ProblemNotExistException : Exception
    {
        private ProblemHandle m_handle;
        
        /// <summary>
        /// 使用给定的题目句柄创建 ProblemNotExistException 类的新实例。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        public ProblemNotExistException(ProblemHandle handle) : base("给定的题目不存在。")
        {
            m_handle = handle;
        }

        /// <summary>
        /// 使用指定的题目句柄和描述信息创建 ProblemNotExistException 类的新实例。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <param name="message">描述信息。</param>
        public ProblemNotExistException(ProblemHandle handle, string message) : base(message)
        {
            m_handle = handle;
        }

        /// <summary>
        /// 获取题目句柄。
        /// </summary>
        public ProblemHandle Handle
        {
            get { return m_handle; }
        }
    }
}
