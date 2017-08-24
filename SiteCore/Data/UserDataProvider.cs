namespace BITOJ.Core.Data
{
    using BITOJ.Core;
    using BITOJ.Data.Entities;
    using System;
    using CoreUserGroup = UserGroup;
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;
    using CoreUserSex = UserSex;
    using NativeUserSex = BITOJ.Data.Entities.UserSex;

    public sealed class UserDataProvider : IUserDataProvider
    {
        /// <summary>
        /// 创建给定用户的 UserDataProvider 对象。
        /// </summary>
        /// <param name="handle">用户句柄对象。</param>
        /// <param name="isReadOnly">一个值，该值指示创建的 UserDataProvider 对象是否应仅具有读权限。</param>
        /// <returns>绑定到给定用户的 UserDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UserNotFoundException"/>
        public static UserDataProvider Create(UserHandle handle, bool isReadOnly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            // 在用户数据库中查询对应的用户实体对象。
            UserProfileEntity entity = UserManager.Default.DataContext.QueryUserProfileEntity(handle.Username);
            if (entity == null)
                throw new UserNotFoundException(handle.Username);

            return new UserDataProvider(entity, isReadOnly);
        }

        private UserProfileEntity m_entity;             // 用户数据实体对象。
        private bool m_readonly;
        private bool m_disposed;

        /// <summary>
        /// 初始化 UserDataProvider 类的新实例。
        /// </summary>
        /// <param name="entity">用户数据实体对象。</param>
        /// <param name="isReadOnly">一个值，该值指示当前对象是否为只读。</param>
        /// <exception cref="ArgumentNullException"/>
        private UserDataProvider(UserProfileEntity entity, bool isReadOnly)
        {
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_readonly = isReadOnly;
            m_disposed = false;
        }

        /// <summary>
        /// 检查当前对象的 Dispose 状态以及写权限，并在必要情况下抛出异常。
        /// </summary>
        private void CheckAccess()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (m_readonly)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// 获取用户的用户名。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Username
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return m_entity.Username;
            }
        }

        /// <summary>
        /// 获取或设置用户的组织名称。
        /// </summary>
        public string Organization
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return m_entity.Organization;
            }
            set
            {
                CheckAccess();

                m_entity.Organization = value;
            }
        }

        /// <summary>
        /// 获取或设置用户的性别。
        /// </summary>
        public CoreUserSex Sex
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return (CoreUserSex)m_entity.Sex;
            }
            set
            {
                CheckAccess();

                m_entity.Sex = (NativeUserSex)value;
            }
        }

        /// <summary>
        /// 获取或设置用户的权限集。
        /// </summary>
        public CoreUserGroup UserGroup
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return (CoreUserGroup)m_entity.UserGroup;
            }
            set
            {
                CheckAccess();

                m_entity.UserGroup = (NativeUserGroup)value;
            }
        }

        /// <summary>
        /// 获取用户的提交统计数据。
        /// </summary>
        public UserSubmissionStatistics SubmissionStatistics
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 获取或设置用户的密码哈希值。
        /// </summary>
        internal byte[] PasswordHash
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return m_entity.PasswordHash;
            }
            set
            {
                CheckAccess();

                m_entity.PasswordHash = value;
            }
        }

        /// <summary>
        /// 释放由当前对象占有的所有资源。
        /// </summary>
        public void Dispose()
        {
            UserManager.Default.DataContext.SaveChanges();
            m_disposed = true;
        }
    }
}
