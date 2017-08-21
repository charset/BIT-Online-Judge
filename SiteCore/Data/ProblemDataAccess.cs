namespace BITOJ.Core.Data
{
    /// <summary>
    /// 编码题目数据访问权限。
    /// </summary>
    public enum ProblemDataAccess : int
    {
        /// <summary>
        /// 只读。
        /// </summary>
        ReadOnly = 0,
        
        /// <summary>
        /// 读写。
        /// </summary>
        ReadWrite = 1,
    }
}