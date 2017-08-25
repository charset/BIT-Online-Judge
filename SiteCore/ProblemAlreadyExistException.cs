namespace BITOJ.Core
{
    /// <summary>
    /// 表示题目已经存在于题目库中的异常。此异常通常在创建一个新问题时抛出。
    /// </summary>
    public sealed class ProblemAlreadyExistException : ProblemException
    {
        /// <summary>
        /// 使用触发异常的题目句柄初始化 ProblemAlreadyExistException 类的新实例。
        /// </summary>
        /// <param name="entry">触发此异常的题目句柄。</param>
        public ProblemAlreadyExistException(ProblemHandle entry) 
            : base(entry, $"题目 \"{entry.ProblemId}\" 已经存在于数据库中。")
        { }

        /// <summary>
        /// 使用触发异常的题目句柄以及自定义异常消息初始化 ProblemAlreadyExistException 类的新实例。
        /// </summary>
        /// <param name="entry">触发此异常的题目句柄。</param>
        /// <param name="message">自定义异常消息。</param>
        public ProblemAlreadyExistException(ProblemHandle entry, string message)
            : base(entry, message)
        { }
    }
}
