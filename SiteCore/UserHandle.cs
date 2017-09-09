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
        /// 确定当前的用户句柄是否与另外一个对象等同。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>一个值，该值指示当前的用户句柄是否与另外一个对象等同。</returns>
        public override bool Equals(object obj)
        {
            UserHandle another = obj as UserHandle;
            if (another == null)
            {
                return false;
            }
            else
            {
                return string.Compare(Username, another.Username, false) == 0;
            }
        }

        /// <summary>
        /// 获取当前对象的哈希值。
        /// </summary>
        /// <returns>当前对象的哈希值。</returns>
        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

        /// <summary>
        /// 确定两个用户句柄是否相同。
        /// </summary>
        /// <param name="handle1">第一个用户句柄。</param>
        /// <param name="handle2">第二个用户句柄。</param>
        /// <returns>一个指示两用户句柄是否等同的值。</returns>
        public static bool operator ==(UserHandle handle1, UserHandle handle2)
        {
            return Equals(handle1, handle2);
        }

        /// <summary>
        /// 确定两个用户句柄是否不相同。
        /// </summary>
        /// <param name="handle1">第一个用户句柄。</param>
        /// <param name="handle2">第二个用户句柄。</param>
        /// <returns>一个指示两用户句柄是否不同的值。</returns>
        public static bool operator !=(UserHandle handle1, UserHandle handle2)
        {
            return !(handle1 == handle2);
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
