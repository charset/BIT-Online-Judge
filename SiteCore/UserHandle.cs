namespace BITOJ.Core
{
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 表示用户标识句柄。
    /// </summary>
    public sealed class UserHandle
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// 使用给定的用户名创建 UserHandle 类的新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <exception cref="ArgumentNullException"/>
        public UserHandle(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }

        /// <summary>
        /// 从给定的 UserProfileEntity 对象创建 UserHandle 对象。
        /// </summary>
        /// <param name="entity">用户信息实体对象。</param>
        /// <returns>UserHandle 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        internal static UserHandle FromUserProfileEntity(UserProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new UserHandle(entity.Username);
        }
    }
}
