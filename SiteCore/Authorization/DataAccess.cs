namespace BITOJ.Core.Authorization
{
    using System;

    /// <summary>
    /// 表示数据访问权限。
    /// </summary>
    [Flags]
    public enum DataAccess
    {
        /// <summary>
        /// 无数据访问权限。
        /// </summary>
        None,

        /// <summary>
        /// 只读访问。
        /// </summary>
        Read,

        /// <summary>
        /// 只写访问。
        /// </summary>
        Write,
        
        /// <summary>
        /// 可读可写访问。
        /// </summary>
        ReadWrite = Read | Write
    }
}