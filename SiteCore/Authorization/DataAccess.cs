namespace BITOJ.Core.Authorization
{
    using System;

    /// <summary>
    /// 表示数据访问权限。
    /// </summary>
    [Flags]
    public enum DataAccess : int
    {
        /// <summary>
        /// 无数据访问权限。
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// 只读访问。
        /// </summary>
        Read = 0x00000001,

        /// <summary>
        /// 只写访问。
        /// </summary>
        Write = 0x00000002,
        
        /// <summary>
        /// 可读可写访问。
        /// </summary>
        ReadWrite = Read | Write
    }
}