namespace BITOJ.Core.Data
{
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 提供队伍数据。
    /// </summary>
    public sealed class TeamDataProvider : IDisposable
    {
        /// <summary>
        /// 使用给定的队伍句柄和一个指示当前对象是否为只读对象的值创建一个新的 TeamDataProvider。
        /// </summary>
        /// <param name="handle">队伍句柄。</param>
        /// <param name="isReadonly">一个值，该值指示当前对象是否为只读。</param>
        /// <returns>绑定到给定队伍句柄的 TeamDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TeamNotFoundException"/>
        public static TeamDataProvider Create(TeamHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            // 从数据库中查询队伍数据。
            TeamProfileEntity entity = UserManager.Default.DataContext.QueryTeamProfileEntity(handle.TeamId);
            if (entity == null)
                throw new TeamNotFoundException(handle);

            return new TeamDataProvider(entity, isReadonly);
        }

        private TeamProfileEntity m_entity;
        private bool m_isReadOnly;
        private bool m_disposed;

        /// <summary>
        /// 使用给定的队伍信息实体对象
        /// </summary>
        /// <param name="entity">队伍信息实体对象。</param>
        /// <param name="isReadOnly">一个值，该值指示当前对象是否为只读。</param>
        /// <exception cref="ArgumentNullException"/>
        private TeamDataProvider(TeamProfileEntity entity, bool isReadOnly)
        {
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_isReadOnly = isReadOnly;
            m_disposed = false;
        }

        ~TeamDataProvider()
        {
            Dispose();
        }

        /// <summary>
        /// 检查当前对象是否具有写权限。
        /// </summary>
        private void CheckAccess()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (m_isReadOnly)
                throw new InvalidOperationException("当前对象为只读对象。");
        }

        /// <summary>
        /// 获取队伍 ID。
        /// </summary>
        public int TeamId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Id;
        }

        /// <summary>
        /// 获取当前队伍中的所有成员。
        /// </summary>
        /// <returns>当前队伍中的所有成员。</returns>
        /// <exception cref="ObjectDisposedException"/>
        public UserHandle[] GetMembers()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);

            UserHandle[] handles = new UserHandle[m_entity.Members.Count];
            int i = 0;
            foreach (UserProfileEntity entity in m_entity.Members)
            {
                handles[i++] = UserHandle.FromUserProfileEntity(entity);
            }

            return handles;
        }

        // TODO: 实现队伍成员增删操作。

        /// <summary>
        /// 获取或设置队伍名称。
        /// </summary>
        public string Name
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Name;
            set
            {
                CheckAccess();
                m_entity.Name = value;
            }
        }

        /// <summary>
        /// 将挂起的更改写入数据库。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && !m_isReadOnly)
            {
                UserManager.Default.DataContext.SaveChanges();
            }
        }

        /// <summary>
        /// 释放当前对象占有的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                Save();
                m_disposed = true;
            }
        }
    }
}
