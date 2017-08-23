namespace BITOJ.Core.Data
{
    using System;

    /// <summary>
    /// 为用户数据提供器提供接口。
    /// </summary>
    public interface IUserDataProvider : IDisposable
    {
        /// <summary>
        /// 获取用户名。
        /// </summary>
        string Username { get; }

        /// <summary>
        /// 获取或设置用户所属组织名称。
        /// </summary>
        string Organization { get; set; }

        /// <summary>
        /// 获取或设置用户性别。
        /// </summary>
        UserSex Sex { get; set; }

        /// <summary>
        /// 获取或设置用户权限集。
        /// </summary>
        UserGroup UserGroup { get; set; }

        /// <summary>
        /// 获取或设置用户提交统计数据。
        /// </summary>
        UserSubmissionStatistics SubmissionStatistics { get; }
    }
}
